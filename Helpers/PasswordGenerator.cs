namespace Star_Security.Helpers
{
    public class PasswordGenerator
    {
        public static string Generate()
        {
            return "St@" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
}
