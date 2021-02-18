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
    class ServerVehicleShops : IScript
    {
        public partial class Server_Vehicle_Shops
        {
            public int id { get; set; }
            public string name { get; set; }
            public float pedX { get; set; }
            public float pedY { get; set; }
            public float pedZ { get; set; }
            public float pedRot { get; set; }
            public float outX { get; set; }
            public float outY { get; set; }
            public float outZ { get; set; }
            public float outRotX { get; set; }
            public float outRotY { get; set; }
            public float outRotZ { get; set; }
        }

        public partial class Server_Vehicle_Shops_Items
        {
            public int id { get; set; }
            public int shopId { get; set; }
            public uint hash { get; set; }
            public int price { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float rotX { get; set; }
            public float rotY { get; set; }
            public float rotZ { get; set; }
            public bool previewSpawn { get; set; } // if true the Vehicle spawns on the given coordinates as preview vehicle.
        }

        public static List<Server_Vehicle_Shops> ServerVehicleShops_ = new List<Server_Vehicle_Shops>();
        public static List<Server_Vehicle_Shops_Items> ServerVehicleShopItems_ = new List<Server_Vehicle_Shops_Items>();

        public static void AddVehicleShopItemToList(int id, int shopId, uint hash, int price, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, bool isPreviewVehicle)
        {
            try
            {
                ServerVehicleShopItems_.Add(new Server_Vehicle_Shops_Items
                {
                    id = id,
                    shopId = shopId,
                    hash = hash,
                    price = price,
                    posX = posX,
                    posY = posY,
                    posZ = posZ,
                    rotX = rotX,
                    rotY = rotY,
                    rotZ = rotZ,
                    previewSpawn = isPreviewVehicle
                });

                MuchColshape colshape = HelperMethods.CreateColShapeSphere(new Position(posX, posY, posZ), 0, 2f);
                if (colshape == null) return;
                colshape.colshapeId = 0;
                colshape.isCarDealerShape = true;
                colshape.carDealerHash = hash;
                colshape.carDealerShopId = shopId;

                if (!isPreviewVehicle) return;
                MuchVehicle veh = HelperMethods.CreateVehicle(hash, new Position(posX, posY, posZ), new Rotation(rotX, rotY, rotZ), 0, "CARDEALER", 0, 36);
                if (veh == null) return;
                veh.vehicleId = 0;
                veh.factionId = 0;
                veh.isCardealerVehicle = true;
                veh.LockState = AltV.Net.Enums.VehicleLockState.Locked;
                veh.ManualEngineControl = true;
                veh.EngineOn = false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }


        public static Rotation GetVehicleShopOutRot(int shopId)
        {
            try
            {
                var shop = ServerVehicleShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop != null) return new Rotation(shop.outRotX, shop.outRotY, shop.outRotZ);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return new Rotation(0, 0, 0);
        }

        public static Position GetVehicleShopOutPos(int shopId)
        {
            try
            {
                var shop = ServerVehicleShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop != null) return new Position(shop.outX, shop.outY, shop.outZ);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return new Position(0, 0, 0);
        }

        public static int GetVehiclePrice(int shopId, uint hash)
        {
            try
            {
                var info = ServerVehicleShopItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.hash == hash);
                if (info != null) return info.price;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void AddVehicleShopToList(int id, string name, float pedX, float pedY, float pedZ, float pedRot, float outX, float outY, float outZ, float outRotX, float outRotY, float outRotZ)
        {
            try
            {
                ServerVehicleShops_.Add(new Server_Vehicle_Shops
                {
                    id = id,
                    name = name,
                    pedX = pedX,
                    pedY = pedY,
                    pedZ = pedZ,
                    pedRot = pedRot,
                    outX = outX,
                    outY = outY,
                    outZ = outZ,
                    outRotX = outRotX,
                    outRotY = outRotY,
                    outRotZ = outRotZ
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static string GetVehicleShopItems(int shopId)
        {
            try
            {
                var items = ServerVehicleShopItems_.ToList().Where(x => x.shopId == shopId).Select(x => new
                {
                    name = ServerVehicleInfo.GetDisplayName(x.hash),
                    x.price,
                    hash = x.hash.ToString(),
                });

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
