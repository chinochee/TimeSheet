using Newtonsoft.Json;

namespace Services
{
    public class Configuration
    {
        private static Configuration _configuration;

        public int PageSize { get; set; }

        private Configuration() { }

        public static Configuration GetConfiguration()
        {
            var configurationPath = Path.Combine(Environment.CurrentDirectory, "Configuration.json");
            var jsonText = File.ReadAllText(configurationPath);
            _configuration = JsonConvert.DeserializeObject<Configuration>(jsonText) ?? new Configuration();
            return _configuration;
        }
    }
}
