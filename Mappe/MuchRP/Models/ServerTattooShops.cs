using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerTattooShops
    {
        public partial class Server_Tattoo_Shops
        {
            public int id { get; set; }
            public string name { get; set; }
            public int owner { get; set; }
            public int bank { get; set; }
            public int price { get; set; }
            public float pedX { get; set; }
            public float pedY { get; set; }
            public float pedZ { get; set; }
            public string pedModel { get; set; }
            public float pedRot { get; set; }
        }

        public static List<Server_Tattoo_Shops> ServerTattooShops_ = new List<Server_Tattoo_Shops>();

        public static void AddShopToList(int id, string name, int owner, int bank, int price, float pedX, float pedY, float pedZ, string pedModel, float pedRot)
        {
            try
            {
                ServerTattooShops_.Add(new Server_Tattoo_Shops
                {
                    id = id,
                    name = name,
                    owner = owner,
                    bank = bank,
                    price = price,
                   pedX = pedX,
                   pedY = pedY,
                   pedZ = pedZ,
                   pedModel = pedModel,
                   pedRot = pedRot
                });

                ServerBlips.AddBlipToList($"Tattoo Shop: {name}", 0, 0.7f, true, 75, pedX, pedY, pedZ);
                ServerPeds.AddPedToList("u_m_y_tattoo_01", pedX, pedY, pedZ, pedRot);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        public static void SetTattooShopBankMoney(int shopId, int money)
        {
            try
            {
                var tattooShop = ServerTattooShops_.FirstOrDefault(x => x.id == shopId);
                if (tattooShop == null) return;
                tattooShop.bank = money;
                Database.SystemDatabase.UpdateTattooShopBank(shopId, money);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetTattooShopBank(int id)
        {
            try
            {
                var tattooShop = ServerTattooShops_.ToList().FirstOrDefault(x => x.id == id);
                if (tattooShop != null) return tattooShop.bank;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }


        public static bool ExistTattooShop(int id)
        {
            try
            {
                return ServerTattooShops_.ToList().Exists(x => x.id == id);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }
    }
}
