using AltV.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MuchRP.Utils
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

        public static string connectionString;

        public static void FetchConfiguration()
        {
            string json = File.ReadAllText(@"config.json");
            ConfigurationData cfg = System.Text.Json.JsonSerializer.Deserialize<ConfigurationData>(json);
            connectionString = $"SERVER={cfg.DatabaseHost}; UID={cfg.DatabaseUser}; PASSWORD={cfg.DatabasePass}; DATABASE={cfg.DatabaseDb}; PORT={cfg.DatabasePort}";
        }
    }
}
