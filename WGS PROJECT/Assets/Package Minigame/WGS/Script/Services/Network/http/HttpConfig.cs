using UnityEngine;
using System.Collections.Generic;
using RunMinigames.Models;

namespace RunMinigames.Services
{

    public class HttpConfig
    {
        public static string baseurl = "https://waffleverse-sol.stagingapps.net";

        public static Dictionary<string, string> endpoint =
            new Dictionary<string, string>()
            {
                {"user", "/api/auth/me"}
            };

    }
}