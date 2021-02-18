using AltV.Net;
using AltV.Net.Async;
using MuchRP.Factories;
using MuchRP.Models;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Handler
{
    class InventoryHandler : IScript
    {
        [AsyncClientEvent("Server:Inventory:openInventory")]
        public async Task ClientEvent_openInventory(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                string invItems = AccountsInventory.GetAccountInventory(player);
                await HelperEvents.ClientEvent_setCefStatus(player, true);
                HelperMethods.TriggerClientEvent(player, "Client:Inventory:openInventory", AccountsInventory.GetInventoryWeight(player), AccountsInventory.GetInventoryMaxWeight(player), invItems);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:dropItem")]
        public async Task ClientEvent_dropItem(MuchPlayer player, string itemName, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                await HelperEvents.ClientEvent_setCefStatus(player, false);
                if (!AccountsInventory.ExistAccountItem(player, itemName) || AccountsInventory.GetItemAmount(player, itemName) < itemAmount || !ServerItems.IsItemDroppable(ServerItems.GetNormalItemName(itemName))) return;
                AccountsInventory.RemoveCharacterItemAmount(player, itemName, itemAmount);
                HelperMethods.sendDiscordLog("Inventar", $"**{player.accountName}** hat den Gegenstand **{itemName} ({itemAmount}x)** weggeworfen.", "blue");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:useItem")]
        public async Task ClientEvent_useItem(MuchPlayer player, string itemName, int itemAmount)
        {

            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                await HelperEvents.ClientEvent_setCefStatus(player, false);
                itemName = ServerItems.GetNormalItemName(itemName);
                if (!AccountsInventory.ExistAccountItem(player, itemName) || AccountsInventory.GetItemAmount(player, itemName) < itemAmount || !ServerItems.IsItemDroppable(ServerItems.GetNormalItemName(itemName))) return;
                if(itemName == "Laptop")
                {
                    if (player.isLaptopActivated) HelperMethods.TriggerClientEvent(player, "Client:Laptop:activateLaptop", false);
                    else
                    {
                        HelperMethods.TriggerClientEvent(player, "Client:Laptop:updateApps", (ServerFactions.GetAccountFaction(player.accountId) == 1 || ServerFactions.GetAccountFaction(player.accountId) == 2 || ServerFactions.GetAccountFaction(player.accountId) == 3));
                        HelperMethods.TriggerClientEvent(player, "Client:Laptop:activateLaptop", true);
                    }
                    player.isLaptopActivated = !player.isLaptopActivated;
                }
                HelperMethods.sendDiscordLog("Inventar", $"**{player.accountName}** hat den Gegenstand **{itemName} ({itemAmount}x)** benutzt.", "blue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
