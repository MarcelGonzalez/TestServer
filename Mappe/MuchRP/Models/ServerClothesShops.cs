using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    public static class ServerClothesShops
    {
        public partial class Server_Clothes_Shops
        {
            public int id { get; set; }
            public string name { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float pedX { get; set; }
            public float pedY { get; set; }
            public float pedZ { get; set; }
            public float pedRot { get; set; }
            public string pedModel { get; set; }
        }

        public partial class Server_Clothes_Shops_Items
        {
            public int id { get; set; }
            public int shopId { get; set; }
            public string clothesName { get; set; }
            public int price { get; set; }
        }

        public static List<Server_Clothes_Shops> ServerClothesShops_ = new List<Server_Clothes_Shops>();
        public static List<Server_Clothes_Shops_Items> ServerClothesShopItems_ = new List<Server_Clothes_Shops_Items>();

        public static void AddShopEntryToList(int id, string name, float posX, float posY, float posZ, float pedX, float pedY, float pedZ, float pedRot, string pedModel)
        {
            try
            {
                ServerClothesShops_.Add(new Server_Clothes_Shops
                {
                    id = id,
                    name = name,
                    posX = posX,
                    posY = posY,
                    posZ = posZ,
                    pedX = pedX,
                    pedY = pedY,
                    pedZ = pedZ,
                    pedModel = pedModel,
                    pedRot = pedRot
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPrice(int shopId, string clothesName)
        {
            try
            {
                var i = ServerClothesShopItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.clothesName == clothesName);
                if (i != null) return i.price;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void AddItemEntryToList(int id, int shopId, string clothesName, int price)
        {
            try
            {
                ServerClothesShopItems_.Add(new Server_Clothes_Shops_Items
                {
                    id = id,
                    shopId = shopId,
                    clothesName = clothesName,
                    price = price
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
