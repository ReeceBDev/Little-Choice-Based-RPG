using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    public static class GameWorld
    {
        public static List<GameEnvironment> environments = new List<GameEnvironment>();

        public static void AddEnvironment(GameEnvironment environment) => environments.Add(environment);
        public static void RemoveEnvironment(GameEnvironment environment) => environments.Remove(environment);
        public static GameEnvironment FindEnvironmentByID(uint id)
        {
            foreach (GameEnvironment environment in environments)
            {
                if (environment.UniqueID == id)
                    return environment;
            }

            throw new Exception("No environment found with that ID.");
        }
    }
}
