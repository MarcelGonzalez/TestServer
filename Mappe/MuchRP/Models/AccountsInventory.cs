using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    public class AccountsInventory
    {
        public partial class Accounts_Inventory
        {
            public int id { get; set; }
            public int accId { get; set; }
            public string itemName { get; set; }
            public int itemAmount { get; set; }
        }

        public static string GetAccountInventory(MuchPlayer player)
        {
            if (player == null || !player.Exists || player.accountId <= 0) return "[]";
            var item = player.AccountInventory_.ToList().Select(x => new
            {
                x.itemName,
                x.itemAmount,
                itemPic = ServerItems.GetItemPic(ServerItems.GetNormalItemName(x.itemName)),
            }).OrderBy(x => x.itemName).ToList();
            return System.Text.Json.JsonSerializer.Serialize(item);
        }

        public static bool ExistAccountItem(MuchPlayer player, string itemName)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName)) return false;
                var item = player.AccountInventory_.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static int GetItemAmount(MuchPlayer player, string itemName)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName)) return 0;
                var item = player.AccountInventory_.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) return item.itemAmount;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static float GetInventoryWeight(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return 0f;
                float invWeight = 0f;
                foreach(var i in player.AccountInventory_.ToList())
                {
                    var sItem = ServerItems.ServerItems_.ToList().FirstOrDefault(x => x.itemName == ServerItems.GetNormalItemName(i.itemName));
                    if (sItem == null) continue;
                    invWeight += sItem.itemWeight * i.itemAmount;
                }
                return invWeight;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0f;
        }

        public static float GetInventoryMaxWeight(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return 15f;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 15f;
        }

        public static void RemoveCharacterItemAmount(MuchPlayer player, string itemName, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var item = player.AccountInventory_.FirstOrDefault(x => x.itemName == itemName);
                if (item == null) return;
                item.itemAmount -= itemAmount;
                if(item.itemAmount > 0)
                {
                    //Item existiert noch, Update!
                    Database.PlayerDatabase.UpdateInventoryItemAmount(player.accountId, itemName, item.itemAmount);
                }
                else
                {
                    //Item existiert nicht mehr, löschen!
                    Database.PlayerDatabase.DeleteInventoryItem(player.accountId, itemName);
                    player.AccountInventory_.Remove(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddCharacterItem(MuchPlayer player, string itemName, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var existItem = player.AccountInventory_.FirstOrDefault(x => x.itemName == itemName);
                if(existItem != null)
                {
                    //Item existiert, Amount erhöhen.
                    existItem.itemAmount += itemAmount;
                    Database.PlayerDatabase.UpdateInventoryItemAmount(player.accountId, itemName, existItem.itemAmount);

                } else
                {
                    //Item existiert nicht, neu erstellen.
                    Database.PlayerDatabase.AddCharacterItem(player.accountId, itemName, itemAmount);
                    player.AccountInventory_.Add(new Accounts_Inventory
                    {
                        accId = player.accountId,
                        itemName = itemName,
                        itemAmount = itemAmount
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
