using AltV.Net;
using AltV.Net.Async;
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
    class ClothesHandler : IScript
    {
        internal static void openClothesStorage(MuchPlayer player)
        {
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || player.IsCefOpen()) return;
				var accItems = AccountsClothes.AccountsClothes_.ToList().Where(x => x.accId == player.accountId).Select(x => new
				{
					x.clothesName,
					type = ServerClothes.GetClothesType(x.clothesName),
					draw = ServerClothes.GetClothesDraw(x.clothesName),
					tex = ServerClothes.GetClothesTexture(x.clothesName),
				}).ToList();
				var itemCount = (int)accItems.Count;
				var iterations = Math.Floor((decimal)(itemCount / 30));
				var rest = itemCount % 30;
				for (var i = 0; i < iterations; i++)
				{
					var skip = i * 30;
					HelperMethods.TriggerClientEvent(player, "Client:ClothesStorage:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(accItems.Skip(skip).Take(30).ToList()));
				}
				if (rest != 0) HelperMethods.TriggerClientEvent(player, "Client:ClothesStorage:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(accItems.Skip((int)iterations * 30).ToList()));

				var torsoItems = ServerClothes.ServerClothes_.ToList().Where(x => x.faction == 0 && x.gender == player.gender && x.type == "Torso").Select(x => new
				{
					x.clothesName,
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
					HelperMethods.TriggerClientEvent(player, "Client:ClothesStorage:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(torsoItems.Skip(torsoSkip).Take(30).ToList()));
				}
				if (rest != 0)
					HelperMethods.TriggerClientEvent(player, "Client:ClothesStorage:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(torsoItems.Skip((int)torsoIterations * 30).ToList()));
				HelperMethods.TriggerClientEvent(player, "Client:ClothesStorage:openStorage", player.gender);
				HelperEvents.ClientEvent_setCefStatus(player, true);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
        }

		[AsyncClientEvent("Server:ClothesStorage:SwitchClothes")]
		public async Task SwitchClothes(MuchPlayer player, string type, string clothesName)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(clothesName) || (clothesName != "None" && !ServerClothes.ExistClothes(clothesName))) return;
				AccountsSkin.SwitchCharacterClothes(player, type, clothesName);
				if (clothesName == "None") player.SendCustomNotification(2, 1500, "Du hast das Kleidungsteil erfolgreich ausgezogen.");
				else player.SendCustomNotification(2, 1500, "Du hast das Kleidungsteil erfolgreich angezogen.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

	}
}
