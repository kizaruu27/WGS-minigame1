using System.Collections.Generic;

namespace RunMinigames.Services.Http
{
    public static class HttpConfig
    {
        public static readonly string BASE_URL = "https://waffleverse-sol.stagingapps.net";

        public static readonly Dictionary<string, string> ENDPOINT =
            new Dictionary<string, string>()
            {
                {"user", "/api/auth/me"}
            };
    }
}