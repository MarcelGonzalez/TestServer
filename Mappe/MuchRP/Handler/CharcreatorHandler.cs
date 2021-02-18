using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using MuchRP.Database;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Handler
{
    class CharcreatorHandler : IScript
    {
        [AsyncClientEvent("Server:Charcreator:prepare")]
        public async Task ClientEvent_PrepareCharcreator(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                player.Position = new Position((float)402.778, (float)-996.9758, (float)-96);
                player.Rotation = new Rotation(0, 0, (float)3.1168559);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Charcreator:CreateCharacter")]
        public async Task ClientEvent_CreateCharacter(MuchPlayer player, string birthdate, int gender, bool isCrimeFlagged, string facefeatures, string headblend, string headoverlay)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                player.gender = gender;
                player.isCrimeFlagged = isCrimeFlagged;
                PlayerDatabase.UpdateAccountGender(player.accountName, gender);
                PlayerDatabase.UpdateAccountCrimeFlag(player.accountName, isCrimeFlagged);
                int id = 0;
                if (Models.AccountsSkin.AccountsSkin_.Any())
                    id = Models.AccountsSkin.AccountsSkin_.Last().id + 1;
                Models.AccountsSkin.CreateNewEntry(new Models.AccountsSkin.Accounts_Skin { accId = player.accountId, facefeatures = facefeatures, headblendsdata = headblend, headoverlays = headoverlay, clothesArmor = "None", clothesBag = "None", clothesBracelet = "None", clothesDecal = "None", clothesEarring = "None", clothesFeet = "None", clothesGlass = "None", clothesHat = "None", clothesLeg = "None", clothesMask = "None", clothesNecklace = "None", clothesTop = "None", clothesTorso = "None", clothesUndershirt = "None", clothesWatch = "None", id = id });
                HelperMethods.TriggerClientEvent(player, "Client:Login:loginSuccess", true, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
