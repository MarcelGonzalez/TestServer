using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Models
{
    class ServerMarker
    {
        public partial class Server_Marker
        {
            public int type { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float scale { get; set; }
            public int red { get; set; }
            public int green { get; set; }
            public int blue { get; set; }
            public int alpha { get; set; }
            public bool bobUpAndDown { get; set; }
        }

        public static List<Server_Marker> ServerMarker_ = new List<Server_Marker>();

        public static void AddMarkerToList(int type, float X, float Y, float Z, float scale, int R, int G, int B, int alpha, bool bobUpAndDown)
        {
            try
            {
                ServerMarker_.Add(new Server_Marker
                {
                    type = type,
                    posX = X,
                    posY = Y,
                    posZ = Z,
                    scale = scale,
                    red = R,
                    green = G,
                    blue = B,
                    alpha = alpha,
                    bobUpAndDown = bobUpAndDown
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
