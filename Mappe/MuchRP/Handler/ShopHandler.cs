using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Resources.Chat.Api;
using MuchRP.Factories;
using MuchRP.Models;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MySql.Data.MySqlClient;

namespace MuchRP.Handler
{
    class ShopHandler : IScript
    {
        #region Shop
        internal static async void openShop(MuchPlayer player, ServerShops.Server_Shops shop)
        {
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || player.IsCefOpen() || shop == null) return;
				if (shop.faction > 0 && ServerFactions.GetAccountFaction(player.accountId) != shop.faction) { player.SendCustomNotification(4, 1500, "Du hast hier keinen Zugriff drauf."); return; }
				if(shop.type == 2 && (ServerFactions.GetAccountFaction(player.accountId) < 10 || ServerFactions.GetAccountFaction(player.accountId) > 25)) { player.SendCustomNotification(4, 1500, "Du hast hier keinen Zugriff drauf."); return; }
				if(shop.type == 1 && (ServerFactions.GetAccountFaction(player.accountId) <= 0 || ServerFactions.GetAccountFaction(player.accountId) > 9)) { player.SendCustomNotification(4, 1500, "Du hast hier keinen Zugriff drauf."); return; }
				await HelperEvents.ClientEvent_setCefStatus(player, true);
				string items = ServerShops.GetShopItems(shop.id);
				HelperMethods.TriggerClientEvent(player, "Client:Shop:openShop", shop.id, items);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
        }

		[AsyncClientEvent("Server:Shop:buyItem")]
		public async Task ClientEvent_buyItem(MuchPlayer player, int shopId, string itemName, int itemAmount)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || shopId <= 0 || itemAmount <= 0) return;
				int itemPrice = ServerShops.GetItemPrice(shopId, itemName) * itemAmount;
				float itemWeight = ServerItems.GetItemWeight(ServerItems.GetNormalItemName(itemName)) * itemAmount;

				if(ServerShops.GetShopItemAmount(shopId, itemName) < itemAmount)
				{
					player.SendCustomNotification(4, 5000, $"Fehler: Dieser Shop hat nurnoch {ServerShops.GetShopItemAmount(shopId, itemName)}x {itemName} auf Lager.");
					return;
				}
								
				if(AccountsInventory.GetInventoryWeight(player) + itemWeight > AccountsInventory.GetInventoryMaxWeight(player))
				{
					player.SendCustomNotification(4, 5000, "Fehler: Du hast nicht genügend Platz für diese Gegenstände.");
					return;
				}

				if(player.cash < itemPrice)
				{
					player.SendCustomNotification(4, 5000, $"Fehler: Du hast nicht genügend Geld dabei ({itemPrice}$).");
					return;
				}

				player.SetPlayerCash(player.cash - itemPrice);
				AccountsInventory.AddCharacterItem(player, itemName, itemAmount);
				ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
				ServerShops.SetShopBankMoney(shopId, ServerShops.GetShopBankMoney(shopId) + itemPrice);
				player.SendCustomNotification(2, 2000, $"{itemAmount}x {itemName} für {itemPrice}$ gekauft.");

				HelperMethods.sendDiscordLog("Shop", $"Der Spieler **{player.accountName}** hat den Gegenstand **{itemName} ({itemAmount}x)** für **{itemPrice}$** gekauft.\nShop-ID: {shopId}", "blue");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}       
        #endregion

        #region Shop Manager
        internal static async void openShopManager(MuchPlayer player, int shopId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0) return;
				await HelperEvents.ClientEvent_setCefStatus(player, true);
				string shopItems = ServerShops.GetShopItems(shopId);
				string inventoryItems = AccountsInventory.GetAccountInventory(player);
				HelperMethods.TriggerClientEvent(player, "Client:Shop:openShopManager", shopId, inventoryItems, shopItems);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:Shop:setItemPrice")]
		public async Task ClientEvent_setItemPrice(MuchPlayer player, int shopId, string itemName, int itemPrice)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemPrice <= 0 || ServerShops.GetShopOwner(shopId) != player.accountId || !ServerShops.ExistShopItem(shopId, itemName)) return;
				ServerShops.SetShopItemPrice(shopId, itemName, itemPrice);
				player.SendCustomNotification(2, 2000, $"Du hast den Gegenstand {itemName} auf den Preis {itemPrice}$ gestellt.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:Shop:depositShopItem")]
		public async Task ClientEvent_depositShopItem(MuchPlayer player, int shopId, string itemName, int itemAmount)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0 || ServerShops.GetShopOwner(shopId) != player.accountId || !AccountsInventory.ExistAccountItem(player, itemName) || AccountsInventory.GetItemAmount(player, itemName) < itemAmount) return;
				AccountsInventory.RemoveCharacterItemAmount(player, itemName, itemAmount);
				ServerShops.AddShopItem(shopId, itemName, itemAmount);
				player.SendCustomNotification(2, 2000, $"Du hast {itemAmount}x {itemName} in das Shop-Lager deponiert.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:Shop:takeShopItem")]
		public async Task ClientEvent_takeShopItem(MuchPlayer player, int shopId, string itemName, int itemAmount)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0 || ServerShops.GetShopOwner(shopId) != player.accountId || !ServerShops.ExistShopItem(shopId, itemName) || ServerShops.GetShopItemAmount(shopId, itemName) < itemAmount) return;
				float itemWeight = ServerItems.GetItemWeight(ServerItems.GetNormalItemName(itemName)) * itemAmount;
				if (AccountsInventory.GetInventoryWeight(player) + itemWeight > AccountsInventory.GetInventoryMaxWeight(player))
				{
					player.SendCustomNotification(4, 5000, "Fehler: Du hast nicht genügend Platz für diese Gegenstände.");
					return;
				}

				AccountsInventory.AddCharacterItem(player, itemName, itemAmount);
				ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
				player.SendCustomNotification(2, 2000, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
		#endregion

		#region VehicleShops 
		internal static async void openVehicleShop(MuchPlayer player, ServerVehicleShops.Server_Vehicle_Shops vehicleShop)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || vehicleShop == null) return;
				await HelperEvents.ClientEvent_setCefStatus(player, true);
				var shopItems = ServerVehicleShops.GetVehicleShopItems(vehicleShop.id);
				HelperMethods.TriggerClientEvent(player, "Client:VehicleShop:openShop", vehicleShop.id, shopItems);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:VehicleShop:buyVehicle")]
		public async Task ClientEvent_buyVehicle(MuchPlayer player, int shopId, string stringifiedHash)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(stringifiedHash)) return;
				uint hash = Convert.ToUInt32(stringifiedHash);
				int price = ServerVehicleShops.GetVehiclePrice(shopId, hash);
				Position parkOut = ServerVehicleShops.GetVehicleShopOutPos(shopId);
				Position parkOutRot = ServerVehicleShops.GetVehicleShopOutRot(shopId);
				MuchVehicle blockingVeh = (MuchVehicle)Alt.Server.GetVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.Position.IsInRange(parkOut, 5f));
				if(blockingVeh != null && blockingVeh.Exists)
				{
					player.SendCustomNotification(4, 1500, "Der Ausparkpunkt ist blockiert.");
					return;
				}

				int randomPlate = new Random().Next(100000, 999999);
				if(ServerVehicles.ExistVehiclePlate($"NL{randomPlate}"))
				{
					ClientEvent_buyVehicle(player, shopId, stringifiedHash);
					return;
				}

				if(player.cash < price)
				{
					player.SendCustomNotification(4, 1500, $"Du hast nicht genügend Geld dabei, benötigt: {price}$");
					return;
				}
				ServerVehicles.CreateNewVehicle(player.accountId, 0, 0, 0, hash, 0, ServerVehicleInfo.GetVehicleFuelLimit(hash), 0, false, 100, parkOut, parkOutRot, $"NL{randomPlate}", DateTime.Now);
				player.SetPlayerCash(player.cash - price);
				player.SendCustomNotification(2, 2500, $"Fahrzeug gekauft, dir wurden {price}$ abgezogen.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
		#endregion

		#region Tattoo Shop
		internal static async void openTattooShop(MuchPlayer player, ServerTattooShops.Server_Tattoo_Shops tattooShop)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || player.IsCefOpen() || tattooShop == null) return;
				await HelperEvents.ClientEvent_setCefStatus(player, true);
				HelperMethods.TriggerClientEvent(player, "Client:TattooShop:openShop", player.gender, tattooShop.id, AccountsTattoos.GetAccountOwnTattoos(player.accountId));
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:TattooShop:buyTattoo")]
		public async Task ClientEvent_buyTattoo(MuchPlayer player, int shopId, int tattooId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || tattooId <= 0 || !ServerTattoos.ExistTattoo(tattooId) || AccountsTattoos.ExistAccountTattoo(player.accountId, tattooId) || !ServerTattooShops.ExistTattooShop(shopId)) return;
				int price = ServerTattoos.GetTattooPrice(tattooId); 
				if (player.cash < price)
				{
					player.SendCustomNotification(4, 5000, $"Fehler: Du hast nicht genügend Geld dabei ({price}$).");
					return;
				}
				player.SetPlayerCash(player.cash - price);
				ServerTattooShops.SetTattooShopBankMoney(shopId, ServerTattooShops.GetTattooShopBank(shopId) + price);
				AccountsTattoos.CreateNewEntry(player.accountId, tattooId);
				player.SendCustomNotification(2, 1500, $"Du hast das Tattoo '{ServerTattoos.GetTattooName(tattooId)}' für {price}$ gekauft.");
				player.updateTattoos();
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:TattooShop:deleteTattoo")]
		public async Task ClientEvent_deleteTattoo(MuchPlayer player, int tattooId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 ||tattooId <= 0 || !AccountsTattoos.ExistAccountTattoo(player.accountId, tattooId)) return;
				AccountsTattoos.RemoveAccountTattoo(player.accountId, tattooId);
				player.SendCustomNotification(2, 1500, $"Du hast das Tattoo '{ServerTattoos.GetTattooName(tattooId)}' erfolgreich entfernen lassen.");
				player.updateTattoos();
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
        #endregion

        #region Kleiderladen
		internal static async void openClothesShop(MuchPlayer player, int shopId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || player.IsCefOpen() || shopId <= 0) return;
				var items = ServerClothesShops.ServerClothesShopItems_.ToList().Where(x => x.shopId == shopId && (ServerClothes.GetClothesGender(x.clothesName) == player.gender || ServerClothes.GetClothesGender(x.clothesName) == 2)).Select(x => new
				{
					x.clothesName,
					x.price,
					type = ServerClothes.GetClothesType(x.clothesName),
					draw = ServerClothes.GetClothesDraw(x.clothesName),
					tex = ServerClothes.GetClothesTexture(x.clothesName),
				}).ToList();

				var itemCount = (int)items.Count;
				var iterations = Math.Floor((decimal)(itemCount / 30));
				var rest = itemCount % 30;
				for (var i = 0; i < iterations; i++)
				{
					var skip = i * 30;
					HelperMethods.TriggerClientEvent(player, "Client:ClothesShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(items.Skip(skip).Take(30).ToList()));
				}
				if (rest != 0) HelperMethods.TriggerClientEvent(player, "Client:ClothesShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(items.Skip((int)iterations * 30).ToList()));

				var torsoItems = ServerClothes.ServerClothes_.ToList().Where(x => x.faction == 0 && x.gender == player.gender && (x.type == "Torso" || x.type == "Undershirt")).Select(x => new
				{
					x.clothesName,
					price = 0,
					x.type,
					x.draw,
					tex = x.texture,
				}).ToList();

				var torsoCount = (int)torsoItems.Count;
				var torsoIterations = Math.Floor((decimal)(torsoCount / 30));
				var torsoRest = torsoCount % 30;
				for (var i = 0; i < torsoIterations; i++)
				{
					var torsoSkip = i * 30;
					HelperMethods.TriggerClientEvent(player, "Client:ClothesShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(torsoItems.Skip(torsoSkip).Take(30).ToList()));
				}
				if (rest != 0) HelperMethods.TriggerClientEvent(player, "Client:ClothesShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(torsoItems.Skip((int)torsoIterations * 30).ToList()));
				HelperMethods.TriggerClientEvent(player, "Client:ClothesShop:openShop", shopId);
				HelperEvents.ClientEvent_setCefStatus(player, true);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:ClothesShop:buyItem")]
		public async Task buyClothes(MuchPlayer player, int shopId, string clothesName)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(clothesName) || !ServerClothes.ExistClothes(clothesName)) return;
				if(AccountsClothes.ExistCharacterClothes(player.accountId, clothesName)) { player.SendCustomNotification(3, 1500, "Dieses Kleidungsteil besitzt du bereits."); return; }
				if(ServerClothes.GetClothesType(clothesName) == "Torso") { AccountsSkin.SwitchCharacterClothes(player, "Torso", clothesName); return; }

				int price = ServerClothesShops.GetPrice(shopId, clothesName);
				if(player.cash < price) { player.SendCustomNotification(3, 1500, $"Du hast nicht genügend Geld ({price}$)."); return; }
				player.SetPlayerCash(player.cash - price);
				AccountsClothes.AddCharacterClothes(player.accountId, clothesName);
				AccountsSkin.SwitchCharacterClothes(player, ServerClothes.GetClothesType(clothesName), clothesName);
				player.SendCustomNotification(2, 1500, $"Kleidung erfolgreich gekauft: {clothesName} ({price}$).");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
        #endregion
    }
}
