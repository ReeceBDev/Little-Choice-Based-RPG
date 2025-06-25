namespace Little_Choice_Based_RPG.Managers.Server
{
    internal static class SystemTimeController
    {
        private static DateTime systemTime = new();
        public static string GetSystemTime() => systemTime.TimeOfDay.ToString();
    }
}
