using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Models
{
    class ServerBlips
    {
        public partial class Server_Blips
        {
            public string name { get; set; }
            public int color { get; set; }
            public float scale { get; set; }
            public bool shortRange { get; set; }
            public int sprite { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
        }

        public static List<Server_Blips> ServerBlips_ = new List<Server_Blips>();

        public static void AddBlipToList(string name, int color, float scale, bool shortRange, int sprite, float posX, float posY, float posZ)
        {
            try
            {
                ServerBlips_.Add(new Server_Blips
                {
                    name = name,
                    color = color,
                    scale = scale,
                    shortRange = shortRange,
                    sprite = sprite,
                    posX = posX,
                    posY = posY,
                    posZ = posZ
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
