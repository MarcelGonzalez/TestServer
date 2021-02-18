using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerGarages
    {
        public partial class Server_Garages
        {
            public int id { get; set; }
            public string name { get; set; }
            public int faction { get; set; }
            public int vehClass { get; set; } //0 Auto | 1 Boot | 2 Flugzeug | 3 Helikopter
            public float pedX { get; set; }
            public float pedY { get; set; }
            public float pedZ { get; set; }
            public float pedRot { get; set; }
            public float outX { get; set; }
            public float outY { get; set; }
            public float outZ { get; set; }
            public float outRotZ { get; set; }
        }

        public static List<Server_Garages> ServerGarages_ = new List<Server_Garages>();

        public static void AddGarageToList(int id, string name, int faction, int vehClass, float pedX, float pedY, float pedZ, float pedRot, float outX, float outY, float outZ, float outRotZ)
        {
            try
            {
                ServerGarages_.Add(new Server_Garages
                {
                    id = id,
                    name = name,
                    faction = faction,
                    vehClass = vehClass,
                    pedX = pedX,
                    pedY = pedY,
                    pedZ = pedZ,
                    pedRot = pedRot,
                    outX = outX,
                    outY = outY,
                    outZ = outZ,
                    outRotZ = outRotZ
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetFactionGarage(string factionShortCut, int vehClass)
        {

            try
            {
                var garageItem = ServerGarages_.ToList().FirstOrDefault(x => x.vehClass == vehClass && x.name.Contains(factionShortCut));
                if (garageItem != null) return garageItem.id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static string GetGarageParkedVehicles(int accId, int garageId, int faction)
        {
            try
            {
                if (accId <= 0 || garageId <= 0) return "[]";
                List<ServerVehicles.Server_Vehicles> parkedVehs = new List<ServerVehicles.Server_Vehicles>();
                if (faction > 0) parkedVehs = ServerVehicles.ServerVehicles_.ToList().Where(x => x.faction == faction && x.isInGarage && x.garageId == garageId).ToList();
                else if (faction == 0) parkedVehs = ServerVehicles.ServerVehicles_.ToList().Where(x => x.faction == 0 && x.isInGarage && x.garageId == garageId && (x.ownerId == accId || x.secondOwnerId == accId)).ToList();

                var items = parkedVehs.Select(x => new
                {
                    x.id,
                    x.plate,
                    name = ServerVehicleInfo.GetDisplayName(x.hash),
                }).ToList();
                return System.Text.Json.JsonSerializer.Serialize(items);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }

        public static string GetGarageOutsideVehicles(int accId, int garageId, int faction, int garageType, Position pos)
        {
            try
            {
                if (accId <= 0 || garageId <= 0) return "[]";
                List<IVehicle> outsideVehicles = null;
                if (faction > 0) outsideVehicles = Alt.Server.GetVehicles().Where(x => x != null && x.Exists && ((MuchVehicle)x).vehicleId != 0 && x.Position.IsInRange(pos, 15f) && ((MuchVehicle)x).factionId == faction).ToList();
                else if (faction == 0) outsideVehicles = Alt.Server.GetVehicles().Where(x => x != null && x.Exists && ((MuchVehicle)x).vehicleId != 0 && (ServerVehicles.GetVehicleOwner(((MuchVehicle)x).vehicleId) == accId || ServerVehicles.GetVehicleSecondaryOwner(((MuchVehicle)x).vehicleId) == accId) && x.Position.IsInRange(pos, 15f)).ToList();

                var items = outsideVehicles.Where(x => ServerVehicleInfo.GetVehicleClass(x.Model) == garageType).Select(x => new
                {
                    id = ((MuchVehicle)x).vehicleId,
                    plate = x.NumberplateText,
                    name = ServerVehicleInfo.GetDisplayName(x.Model),
                }).ToList();
                return System.Text.Json.JsonSerializer.Serialize(items);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }
    }
}
