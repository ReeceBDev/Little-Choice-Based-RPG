using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Types;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Represents a worker thread to perform service related tasks, sequentially. Allows internal threads to end at services, so that internal threads are never blocked externally.
    /// This is useful when trying to push data such as events out of the service to a listener; the event invoker is never blocked if it passes over the event to this class.
    /// 
    /// Tasks operate sequentially, so can be used for ensuring state lands in the correct order on the client. The alternative, of running Task.Run() individually, would not guarantee order. </summary>
    public sealed class ServiceWorker : IDisposable
    {
        private Channel<Func<CancellationToken, Task<bool>>> pendingTasks = Channel.CreateUnbounded<Func<CancellationToken, Task<bool>>>(); //Supports concurrency as how the API will be used from outside is not determinable.
        private Dictionary<Func<CancellationToken, Task<bool>>, RunningTaskData> runningTasks = new(); //For removing tasks later and preventing duplicate tasks from being initialised.
        private readonly object runningTasksLock = new(); //Must be an object, not a Lock, in order to ensure lock uses monitor.

        private Task<bool> ongoingWorkerLoop;
        private CancellationTokenSource taskReaderCancellationSource = new();

        private int cancellationTimeoutInMS = 180000; //Three minutes. Defines how long a task has to complete its cancellation before throwing an exception.

        public ServiceWorker()
        {
            //Generate the worker thread and set it to execute the task loop
            Task.Run(() => ongoingWorkerLoop = ExecuteTaskSetAsync(taskReaderCancellationSource.Token));
        }

        /// <summary> Allocate a delegate for this worker to run repeatedly. Upon completing, the task will be re-ran. </summary>
        public void AssignTask(Func<CancellationToken, Task<bool>> newTask)
        {
            //Mark the task as pending
            pendingTasks.Writer.TryWrite(newTask);
        }

        /// <summary> 
        /// Removes a task from runningTasks but doesn't ensure it is never added, if it is waiting to be queued by pendingTasks. 
        /// 
        /// Must only be used after taskReaderCancellationSource.Cancel is called and a lock is aquired on runningTasks. 
        /// Doing this is required since there is no way to check for individual tasks in pendingTasks.
        /// Will throw an exception if it wasn't yet added to runningTask, it is also important to note.
        /// 
        /// This method provides enough functionality to work within the current use case, which is this class's Dispose() function, however, does not provide enough to extend to a public method.
        /// </summary>
        private async Task<bool> RemoveTaskAsync(Func<CancellationToken, Task<bool>> task)
        {
            RunningTaskData runningData;

            if (!taskReaderCancellationSource.IsCancellationRequested)
                throw new ArgumentException("taskReaderCancellationSource has not yet been cancelled. This method requires it in order to ensure no new tasks are to be added to runningTasks.");

            if (!Monitor.IsEntered(runningTasksLock))
                throw new Exception("The runningTasksLock as not yet been acquired by this thread. This method requires it in order to ensure ongoing additions to runningTasks are accounted for.");

            if (!runningTasks.ContainsKey(task))
                throw new ArgumentException("Value not found within the runningTasks set.");

            //Cancel its token
            runningTasks.TryGetValue(task, out runningData);
            runningData.CancellationSource.Cancel();

            //Remove the task from runningTasks
            runningTasks.Remove(task);

            //Await its completion
            if (await Task.WhenAny(runningData.Task, Task.Delay(cancellationTimeoutInMS)) != runningData.Task)
                throw new Exception("A worker-thread task which was cancelled has timed out before completing its cancellation!");

            return true;
        }

        public async void Dispose()
        {
            List<Task<bool>> cancellingTasks = new();

            //Cancel the task reader. This stops new tasks being read from pendingTasks, and after anything ongoing, stops new runningTasks being added.
            taskReaderCancellationSource.Cancel();

            lock (runningTasksLock) ;//Must be here to prevent a race condition. Ensures this foreach is never skipped and that it won't begin until the most-recent new-addition finished.
            {
                //Cancel all running tasks
                foreach (var task in runningTasks.Keys)
                {
                    cancellingTasks.Add(RemoveTaskAsync(task));
                }
            }

            //Await their final completions
            await Task.WhenAll(cancellingTasks);

            //Await the tasksetting method
            await ongoingWorkerLoop;

            //Finalize this class //Do i need to indicate that this finalizer need not run instead?
            //~ServiceWorker();
            throw new NotImplementedException();
        }

        private async Task<bool> ExecuteTaskSetAsync(CancellationToken loopCancellation)
        {
            CancellationTokenSource taskCancelSource;

            //Stay here forever muhahaha!
            while (true) //Spin thread
            {
                //Grab new tasks
                Func<CancellationToken, Task<bool>> newTask = await pendingTasks.Reader.ReadAsync(loopCancellation); //Awaiting here ensures this thread won't spin incessantly.

                //Generate a CancellationTokenSource for this task
                taskCancelSource = new();

                lock (runningTasksLock) //Lock. This ensures that the new task being added can be removed, without creating a race-condition out from conconcurrent removes before fully initialised.
                {
                    if (loopCancellation.IsCancellationRequested) //Checking after acquiring the lock ensures no new entries will be added without removal methods knowing about it.
                        break;

                    if (runningTasks.ContainsKey(newTask))
                        throw new Exception("The new task retrieved from pendingTasks was already listed in runningTasks! Ensure tasks are not added twice.");

                    //Run the new task forever
                    Task<bool> runningTask = RunForeverAsync(newTask, taskCancelSource.Token); //Don't await this, it's the follow-up from grabbing the new task. The async logic should only wait for new entries - using them instantly.

                    //Mark task as on-going
                    runningTasks.Add(newTask, new RunningTaskData(runningTask, taskCancelSource));
                }
            }

            return true;
        }

        private async Task<bool> RunForeverAsync(Func<CancellationToken, Task<bool>> task, CancellationToken token)
        {
            //Fire tasks infinitely upon their completion
            while (true)
            {
                await InvokeTaskAsync(task, token);

                if (token.IsCancellationRequested)
                    break;
            }

            return true;
        }

        private async Task<Func<CancellationToken, Task<bool>>> InvokeTaskAsync(Func<CancellationToken, Task<bool>> task, CancellationToken token)
        {
            await task.Invoke(token);

            return task;
        }
    }
}