namespace Services.Configuration
{
    public class CacheSettings
    {
        public const string Settings = "CacheSettings";
        public int SecondsHoldCache { get; set; }
        public string ApiHostName { get; set; }
    }
}