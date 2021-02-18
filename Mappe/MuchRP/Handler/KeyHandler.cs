using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using MuchRP.Factories;
using MuchRP.Models;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Handler
{
    class KeyHandler : IScript
    {
        [AsyncClientEvent("Server:Keyhandler:pressE")]
        public async Task ClientEvent_pressE(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                var atm = ServerATM.ServerATM_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1.5f));
                if(atm != null && !player.IsInVehicle)
                {
                    ATMHandler.openATM(player);
                    return;
                }

                var shop = ServerShops.ServerShops_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1.5f));
                if(shop != null && !player.IsInVehicle)
                {
                    ShopHandler.openShop(player, shop);
                    return;
                }

                var tattooShop = ServerTattooShops.ServerTattooShops_.ToList().FirstOrDefault(x => x.owner != 0 && player.Position.IsInRange(new Position(x.pedX, x.pedY, x.pedZ), 2.5f));
                if(tattooShop != null && !player.IsInVehicle)
                {
                    ShopHandler.openTattooShop(player, tattooShop);
                    return;
                }

                var clothesShop = ServerClothesShops.ServerClothesShops_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1.5f));
                if(clothesShop != null && !player.IsInVehicle)
                {
                    ShopHandler.openClothesShop(player, clothesShop.id);
                    return;
                }

                var clothesStorage = ServerClothesStorages.ServerClothesStorages_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1.5f)); //ToDo: Fraktion
                if (clothesStorage != null && !player.IsInVehicle)
                {
                    ClothesHandler.openClothesStorage(player);
                    return;
                }

                var garage = ServerGarages.ServerGarages_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.pedX, x.pedY, x.pedZ), 1.5f));
                if(garage != null && !player.IsInVehicle)
                {
                    GarageHandler.openGarage(player, garage);
                    return;
                }

                var vehicleShop = ServerVehicleShops.ServerVehicleShops_.ToList().FirstOrDefault(x => player.Position.IsInRange(new Position(x.pedX, x.pedY, x.pedZ), 2.5f));
                if(vehicleShop != null && !player.IsInVehicle)
                {
                    ShopHandler.openVehicleShop(player, vehicleShop);
                    return;
                }

                var shopManage = ServerShops.ServerShops_.ToList().FirstOrDefault(x => x.owner != 0 && x.owner == player.accountId && player.Position.IsInRange(new Position(x.manageX, x.manageY, x.manageZ), 1.5f) && x.type == 0);
                if(shopManage != null && !player.IsInVehicle)
                {
                    ShopHandler.openShopManager(player, shopManage.id);
                    return;
                }

                var factionManage = ServerFactions.ServerFactions_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.managePos, 1.5f) && ServerFactions.IsAccountInAnyFaction(player.accountId) && x.id == ServerFactions.GetAccountFaction(player.accountId) && ServerFactions.IsRankALeaderShipment(x.id, ServerFactions.GetAccountRank(player.accountId)));
                if(factionManage != null && !player.IsInVehicle)
                {
                    FactionHandler.openFactionManager(player, factionManage.id);
                    return;
                }

                var laborEntry = ServerFactions.ServerFactions_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.laborPos, 2.5f) && !x.isLaborLocked);
                if(laborEntry != null)
                {
                    player.Dimension = laborEntry.id;
                    player.Position = ServerFactions.GetLaborExitPosition(laborEntry.id);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Keyhandler:pressL")]
        public async Task ClientEvent_pressL(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                var laborEntry = ServerFactions.ServerFactions_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.laborPos, 2.5f));
                if (laborEntry != null && !player.IsInVehicle && ServerFactions.IsAccountInAnyFaction(player.accountId) && ServerFactions.GetAccountFaction(player.accountId) == laborEntry.id)
                {
                    if (laborEntry.isLaborLocked) player.SendCustomNotification(2, 1000, "Du hast das Labor aufgeschlossen.");
                    else player.SendCustomNotification(2, 1000, "Du hast das Labor abgeschlossen.");
                    ServerFactions.SetLaborLocked(laborEntry.id, !laborEntry.isLaborLocked);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
