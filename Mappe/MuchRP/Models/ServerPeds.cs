using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Models
{
    class ServerPeds
    {
        public partial class Server_Peds
        {
            public string pedModel { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float pedRotation { get; set; }
        }

        public static List<Server_Peds> ServerPeds_ = new List<Server_Peds>();

        public static void AddPedToList(string model, float X, float Y, float Z, float rotation)
        {
            try
            {
                ServerPeds_.Add(new Server_Peds
                {
                    pedModel = model,
                    posX = X,
                    posY = Y,
                    posZ = Z,
                    pedRotation = rotation
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
