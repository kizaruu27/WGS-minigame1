namespace RunMinigames.Models.Http.PlayerInfo
{
    public class MPlayerInfo
    {
        public Data data { get; set; }
    }


    public class Data
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string uname { get; set; }
        public object email { get; set; }
        public int utype { get; set; }
        public string sol_address { get; set; }
        public object google_id { get; set; }
    }
}