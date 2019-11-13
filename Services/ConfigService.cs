using Newtonsoft.Json;
using StreamMusicBot.Entities;
using System.IO;

namespace StreamMusicBot.Services
{
    public class ConfigService
    {
        private const string configFolder = "Resources";
        private const string configFile = "Config.json";

        public Config GetConfig()
        {
            var file = "Config.json";
            var data = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Config>(data);
        }
    }
}
