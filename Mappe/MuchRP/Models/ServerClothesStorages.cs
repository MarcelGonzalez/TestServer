using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerClothesStorages
    {
        public partial class Server_Clothes_Storages
        {
            public int id { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public int faction { get; set; }
        }

        public static List<Server_Clothes_Storages> ServerClothesStorages_ = new List<Server_Clothes_Storages>();
    
        public static void AddEntryToList(int id, float X, float Y, float Z, int faction)
        {
            try
            {
                ServerClothesStorages_.Add(new Server_Clothes_Storages
                {
                    id = id,
                    posX = X,
                    posY = Y,
                    posZ = Z,
                    faction = faction
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetStorageFaction(int id)
        {
            try
            {
                var storage = ServerClothesStorages_.ToList().FirstOrDefault(x => x.id == id);
                if (storage != null) return storage.faction;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }
    }
}
