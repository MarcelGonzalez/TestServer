using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Models
{
    class ServerShops
    {
        public partial class Server_Shops
        {
            public int id { get; set; }
            public string name { get; set; }
            public int owner { get; set; }
            public int bank { get; set; }
            public int price { get; set; }
            public float pedX { get; set; }
            public float pedY { get; set; }
            public float pedZ { get; set; }
            public float posX { get; set; }
            public float posY { get; set; }
            public float posZ { get; set; }
            public float manageX { get; set; }
            public float manageY { get; set; }
            public float manageZ { get; set; }
            public string pedModel { get; set; }
            public float pedRot { get; set; }
            public int blipColor { get; set; }
            public int blipSprite { get; set; }
            public int type { get; set; } // 0 = normal, 1 = Staatsfraktion (ohne Manage), 2 = Badfraktion (ohne Manage)
            public int faction { get; set; }
        }

        public partial class Server_Shops_Items
        {
            public int id { get; set; }
            public int shopId { get; set; }
            public string itemName { get; set; }
            public int itemPrice { get; set; }
            public int itemAmount { get; set; }
        }

        public static List<Server_Shops> ServerShops_ = new List<Server_Shops>();
        public static List<Server_Shops_Items> ServerShopsItems_ = new List<Server_Shops_Items>();

        public static int GetShopItemAmount(int shopId, string itemName)
        {
            try
            {
                var shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem != null) return shopItem.itemAmount;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static bool ExistShopItem(int shopId, string itemName)
        {
            try
            {
                var shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static void RemoveShopItemAmount(int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var shopItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem == null) return;
                shopItem.itemAmount -= itemAmount;
                if (shopItem.itemAmount > 0)
                {
                    //Item existiert noch, updaten.
                    Database.SystemDatabase.UpdateShopItemAmount(shopId, itemName, shopItem.itemAmount);
                }
                else
                {
                    //Item existiert nicht mehr, löschen.
                    Database.SystemDatabase.DeleteShopItem(shopId, itemName);
                    ServerShopsItems_.Remove(shopItem);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddShopItem(int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var existItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if(existItem != null)
                {
                    //Item existiert, Anzahl erhöhen.
                    existItem.itemAmount += itemAmount;
                    Database.SystemDatabase.UpdateShopItemAmount(shopId, itemName, existItem.itemAmount);
                }
                else
                {
                    //Item existiert nicht, neu erstellen.
                    Database.SystemDatabase.AddShopItem(shopId, itemName, itemAmount, 99999);
                    ServerShopsItems_.Add(new Server_Shops_Items
                    {
                        itemName = itemName,
                        itemPrice = 99999,
                        itemAmount = itemAmount,
                        shopId = shopId
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetShopItemPrice(int shopId, string itemName, int newPrice)
        {
            try
            {
                var shopItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem == null || shopItem.itemPrice == newPrice) return;
                shopItem.itemPrice = newPrice;
                Database.SystemDatabase.UpdateShopItemPrice(shopId, itemName, newPrice);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetShopOwner(int shopId)
        {
            try
            {
                var shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop != null) return shop.owner;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetShopBankMoney(int shopId, int money)
        {
            try
            {
                var shop = ServerShops_.FirstOrDefault(x => x.id == shopId);
                if (shop == null) return;
                shop.bank = money;
                Database.SystemDatabase.UpdateShopBank(shopId, money);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetShopBankMoney(int shopId)
        {
            try
            {
                var shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop != null) return shop.bank;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static string GetShopItems(int shopId)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(ServerShopsItems_.ToList().Where(x => x.shopId == shopId).Select(x => new
            {
                x.itemName,
                x.itemPrice,
                x.itemAmount,
                itemPic = ServerItems.GetItemPic(ServerItems.GetNormalItemName(x.itemName)),
            }).ToList());
            return json;
        }

        public static int GetShopType(int shopId)
        {

            try
            {
                var shopItem = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shopItem != null) return shopItem.type;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetShopFaction(int shopId)
        {

            try
            {
                var shopItem = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shopItem != null) return shopItem.faction;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetItemPrice(int shopId, string itemName)
        {
            try
            {
                var shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem != null) return shopItem.itemPrice;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }
    }
}
