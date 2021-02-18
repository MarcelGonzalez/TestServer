using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerVehicleInfo
    {
        public partial class Server_Vehicle_Info
        {
            public uint hash { get; set; }
            public string displayName { get; set; }
            public float trunk { get; set; }
            public float maxFuel { get; set; }
            public int tax { get; set; }
            public int vehClass { get; set; } //0 Auto | 1 Boot | 2 Flugzeug | 3 Helikopter
        }

        public static List<Server_Vehicle_Info> ServerVehicleInfo_ = new List<Server_Vehicle_Info>();

        public static int GetVehicleClass(uint hash)
        {
            try
            {
                var info = ServerVehicleInfo_.ToList().FirstOrDefault(x => x.hash == hash);
                if (info != null) return info.vehClass;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static string GetDisplayName(uint hash)
        {
            try
            {
                var info = ServerVehicleInfo_.ToList().FirstOrDefault(x => x.hash == hash);
                if (info != null) return info.displayName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static float GetVehicleTrunkLimit(uint hash)
        {
            try
            {
                var info = ServerVehicleInfo_.ToList().FirstOrDefault(x => x.hash == hash);
                if (info != null) return info.trunk;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0f;
        }

        public static float GetVehicleFuelLimit(uint hash)
        {
            try
            {
                var info = ServerVehicleInfo_.ToList().FirstOrDefault(x => x.hash == hash);
                if (info != null) return info.maxFuel;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0f;
        }

        public static void AddVehicleInfoToList(uint hash, string name, float trunk, float maxFuel, int tax, int vehClass)
        {
            try
            {
                ServerVehicleInfo_.Add(new Server_Vehicle_Info
                {
                    hash = hash,
                    displayName = name,
                    tax = tax,
                    trunk = trunk,
                    maxFuel = maxFuel,
                    vehClass = vehClass
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
