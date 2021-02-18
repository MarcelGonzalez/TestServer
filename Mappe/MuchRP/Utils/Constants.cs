using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Utils
{
    class Constants
    {
        public partial class DatabaseConfig
        {
            public static string connectionString = Configuration.connectionString;
        }

        public partial class Positions
        {
            public static readonly Position spawnPosition_Airport = new Position(-1045.2131f, -2750.8748f, 21.360474f);
            public static readonly Position spawnPosition_PaletoBay = new Position(-264.932f, 6621.522f, 7f);

            // Labor Positions
            public static readonly Position methLabor_ExitPosition = new Position(0, 0, 0);
            public static readonly Position weedLabor_ExitPosition = new Position(0, 0, 0);
        }
    }
}
