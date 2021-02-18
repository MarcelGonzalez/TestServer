using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Models
{
    class ServerATM
    {
        public partial class Server_ATM
        {
            public int id { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
        }

        public static List<Server_ATM> ServerATM_ = new List<Server_ATM>();
    }
}
