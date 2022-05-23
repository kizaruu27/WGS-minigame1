using UnityEngine;
using System.Collections.Generic;
using RunMinigames.Models;

namespace RunMinigames.Services
{
    public static class HttpConfig
    {
        public static readonly string baseurl = "https://waffleverse-sol.stagingapps.net";

        public static readonly Dictionary<string, string> endpoint =
            new Dictionary<string, string>()
            {
                {"user", "/api/auth/me"}
            };
    }
}