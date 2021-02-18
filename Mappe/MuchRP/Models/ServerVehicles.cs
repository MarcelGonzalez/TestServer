using AltV.Net;
using AltV.Net.Data;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerVehicles : IScript
    {
        public partial class Server_Vehicles
        {
            public int id { get; set; }
            public int ownerId { get; set; }
            public int secondOwnerId { get; set; }
            public uint hash { get; set; }
            public int faction { get; set; }
            public float fuel { get; set; }
            public float km { get; set; }
            public bool isInGarage { get; set; }
            public int garageId { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float rotX { get; set; }
            public float rotY { get; set; }
            public float rotZ { get; set; }
            public string plate { get; set; }
            public DateTime lastUsage { get; set; }
            public bool isLocked { get; set; } = true;
        }

        public static List<Server_Vehicles> ServerVehicles_ = new List<Server_Vehicles>();

        public static void CreateNewVehicle(int ownerId, int primaryColor, int secondaryColor, int secondOwnerId, uint hash, int faction, float fuel, float km, bool isInGarage, int garageId, Position pos, Rotation rot, string plate, DateTime lastUsage)
        {
            try
            {
                int id = 0;
                if (ServerVehicles_.Any())
                    id = ServerVehicles_.Last().id + 1;

                ServerVehicles_.Add(new Server_Vehicles
                {
                    id = id,
                    ownerId = ownerId,
                    secondOwnerId = secondOwnerId,
                    hash = hash,
                    faction = faction,
                    fuel = fuel,
                    km = km,
                    isInGarage = isInGarage,
                    garageId = garageId,
                    posX = pos.X,
                    posY = pos.Y,
                    posZ = pos.Z,
                    rotX = rot.Pitch,
                    rotY = rot.Roll,
                    rotZ = rot.Yaw,
                    plate = plate,
                    lastUsage = lastUsage
                });

                ServerVehiclesMod.AddVehicleModToL(id, id, primaryColor, secondaryColor, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 0, 0, 0, 0, 0);
                Database.VehicleDatabase.AddVehicleToDB(id, ownerId, secondOwnerId, hash, faction, fuel, km, isInGarage, garageId, pos, rot, plate, lastUsage);
                if (isInGarage) return;
                MuchVehicle veh = HelperMethods.CreateVehicle(hash, pos, rot, 0, plate, (byte)primaryColor, (byte)secondaryColor);
                veh.NumberplateText = plate;
                veh.LockState = AltV.Net.Enums.VehicleLockState.Locked;
                veh.ManualEngineControl = true;
                veh.EngineOn = false;
                veh.vehicleId = id;
                veh.factionId = faction;
                ServerVehiclesMod.SetVehicleModifications(veh);
                ServerVehicles.UpdateVehicleLastPosition(veh);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetVehicleLocked(int vehId, bool isLocked)
        {
            try
            {
                var vehicle = ServerVehicles_.FirstOrDefault(x => x.id == vehId);
                if (vehicle == null) return;
                vehicle.isLocked = isLocked;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetVehicleInGarage(int vehId, bool isInGarage, int garageId)
        {
            try
            {
                var vehicle = ServerVehicles_.FirstOrDefault(x => x.id == vehId);
                if (vehicle == null) return;
                vehicle.isInGarage = isInGarage;
                vehicle.garageId = garageId;
                Database.VehicleDatabase.SetVehicleInGarageDB(vehId, isInGarage, garageId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool IsVehicleLocked(int vehId)
        {
            try
            {
                var vehicle = ServerVehicles_.ToList().FirstOrDefault(x => x.id == vehId);
                if (vehicle != null) return vehicle.isLocked;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static int GetVehicleOwner(int vehId)
        {
            try
            {
                var vehicle = ServerVehicles_.ToList().FirstOrDefault(x => x.id == vehId);
                if (vehicle != null) return vehicle.ownerId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static bool ExistVehiclePlate(string plate)
        {
            try
            {
                var vehicle = ServerVehicles_.ToList().FirstOrDefault(x => x.plate == plate);
                if (vehicle != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        internal static void UpdateVehicleLastPosition(MuchVehicle veh)
        {
            try
            {
                if (veh == null || !veh.Exists || veh.vehicleId <= 0) return;
                var vehicle = ServerVehicles_.FirstOrDefault(x => x.id == veh.vehicleId);
                if (vehicle == null) return;
                vehicle.posX = veh.Position.X;
                vehicle.posY = veh.Position.Y;
                vehicle.posZ = veh.Position.Z;
                vehicle.rotX = veh.Rotation.Pitch;
                vehicle.rotY = veh.Rotation.Roll;
                vehicle.rotZ = veh.Rotation.Yaw;

                Database.VehicleDatabase.UpdateVehicleLastPos(veh.vehicleId, veh.Position, veh.Rotation);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetVehicleSecondaryOwner(int vehId)
        {
            try
            {
                var vehicle = ServerVehicles_.ToList().FirstOrDefault(x => x.id == vehId);
                if (vehicle != null) return vehicle.secondOwnerId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }



        public static void AddVehicleToList(int id, int ownerId, int secondOwnerId, uint hash, int faction, float fuel, float km, bool isInGarage, int garageId, Position pos, Rotation rot, string plate, DateTime lastUsage)
        {
            try
            {
                ServerVehicles_.Add(new Server_Vehicles
                {
                    id = id,
                    ownerId = ownerId,
                    secondOwnerId = secondOwnerId,
                    hash = hash,
                    faction = faction,
                    fuel = fuel,
                    km = km,
                    isInGarage = isInGarage,
                    garageId = garageId,
                    posX = pos.X,
                    posY = pos.Y,
                    posZ = pos.Z,
                    rotX = rot.Pitch,
                    rotY = rot.Roll,
                    rotZ = rot.Yaw,
                    plate = plate,
                    lastUsage = lastUsage
                });

                if (isInGarage) return;
                MuchVehicle veh = HelperMethods.CreateVehicle(hash, pos, rot, 0, plate, 0, 0);
                veh.LockState = AltV.Net.Enums.VehicleLockState.Locked;
                veh.ManualEngineControl = true;
                veh.EngineOn = false;
                veh.NumberplateText = plate;
                veh.vehicleId = id;
                veh.factionId = faction;
                ServerVehiclesMod.SetVehicleModifications(veh);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
