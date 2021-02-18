using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordBot
{
    public static class Configuration
    {
        public partial class ConfigurationData
        {
            public string DatabaseHost { get; set; }
            public int DatabasePort { get; set; }
            public string DatabaseUser { get; set; }
            public string DatabasePass { get; set; }
            public string DatabaseDb { get; set; }
            public string discordBotToken { get; set; }
            public ulong discordBotChannel { get; set; }
        }

        public static string discordBotToken;
        public static ulong discordBotChannel;

        public static void FetchConfiguration()
        {
            string json = File.ReadAllText(@"config.json");
            ConfigurationData cfg = System.Text.Json.JsonSerializer.Deserialize<ConfigurationData>(json);
            discordBotToken = cfg.discordBotToken;
            discordBotChannel = cfg.discordBotChannel;
        }
    }
}
