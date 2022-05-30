using Newtonsoft.Json;

namespace RunMinigames.Models.Http.PlayerInfo
{
    // [Preserve(AllMembers = true)]
    public class MPlayerInfo
    {
        [JsonConstructor]
        public MPlayerInfo() { }
        public Data data { get; set; }
    }

    // [Preserve(AllMembers = true)]
    public class Data
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string uname { get; set; }
        public object email { get; set; }
        public int utype { get; set; }
        public string sol_address { get; set; }
        public object google_id { get; set; }

        [JsonConstructor]
        public Data() { }
    }
}