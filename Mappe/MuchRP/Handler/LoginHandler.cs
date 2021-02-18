using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using MuchRP.Database;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Handler
{
    class LoginHandler : IScript
    {
        [AsyncClientEvent("Server:Login:ValidateLoginCredentials")]
        public async Task ClientEvent_ValidateLoginCredentials(MuchPlayer player, string username, string password)
        {
            try
            {
                if (player == null || !player.Exists || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return;
                if(PlayerDatabase.ExistAccountName(username))
                {
                    //Name existiert
                    if(PlayerDatabase.GetAccountPassword(username) != password)
                    {
                        HelperMethods.TriggerClientEvent(player, "Client:Login:showError", $"Das eingegebene Passwort ist falsch.");
                        HelperMethods.sendDiscordLog("Falsche Benutzerdaten", $"Der Spieler **{username}** hat versucht sich mit einem falschen Passwort anzumelden. \nIP: {player.Ip}", "red");
                        return;
                    }

                    if(PlayerDatabase.GetAccountSocialClub(username) != player.SocialClubId)
                    {
                        HelperMethods.TriggerClientEvent(player, "Client:Login:showError", $"Dieser Account gehört nicht dir.");
                        HelperMethods.sendDiscordLog("Falsche Social-Club ID", $"Der Spieler **{username}** hat versucht sich mit einer falschen Social-Club ID anzumelden. \nIP: {player.Ip} | Versuchte Social-ID: {player.SocialClubId}", "red");
                        return;
                    }

                    if(!PlayerDatabase.HasAccountWhitelist(username))
                    {
                        HelperMethods.TriggerClientEvent(player, "Client:Login:showError", $"Dieser Account wurde noch nicht freigeschaltet.");
                        HelperMethods.sendDiscordLog("Fehlende Freischaltung", $"Der Spieler **{username}** hat versucht sich mit einem nicht freigeschalteten Account anzumelden. \nIP: {player.Ip} | Versuchte Social-ID: {player.SocialClubId}", "red");
                        return;
                    }

                    PlayerDatabase.LoadAccount(player, username);
                    player.Dimension = player.accountId;
                    HelperMethods.TriggerClientEvent(player, "Client:Login:loginSuccess", PlayerDatabase.ExistAccountSkin(player.accountId), 0);
                    HelperMethods.sendDiscordLog("Login erfolgreich", $"Der Spieler **{username}** hat sich erfolgreich angemeldet. \nIP: {player.Ip}", "green");

                } 
                else
                {
                    //Name existiert nicht
                    if(PlayerDatabase.ExistSocialIdInDB(player.SocialClubId))
                    {
                        HelperMethods.TriggerClientEvent(player, "Client:Login:showError", $"Du besitzt bereits einen Benutzeraccount.");
                        return;
                    }
                    PlayerDatabase.RegisterAccount(username, password, player.SocialClubId);
                    HelperMethods.TriggerClientEvent(player, "Client:Login:showError", $"Benutzeraccount erstellt - logge dich ein.");
                    HelperMethods.sendDiscordLog("Registrierung erfolgreich", $"Der Spieler **{username}** hat sich erfolgreich registriert. \nIP: {player.Ip} | Social-ID: {player.SocialClubId}", "green");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Charselector:loadCharacter")]
        public async Task ClientEvent_loadCharacter(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                if(PlayerDatabase.IsAccountFirstLogin(player.accountName))
                {
                    //Erster Login
                    if (player.isCrimeFlagged) PlayerDatabase.CreateAccountLastPosition(player.accountId, Constants.Positions.spawnPosition_PaletoBay, 0);
                    else PlayerDatabase.CreateAccountLastPosition(player.accountId, Constants.Positions.spawnPosition_Airport, 0);
                    PlayerDatabase.UpdateAccountFirstLogin(player.accountName, false);
                } 

                if(player.gender == 0) player.Model = 1885233650;
                else if(player.gender == 1) player.Model = 2627665880;
                HelperMethods.TriggerClientEvent(player, "Client:Charselector:setCorrectSkin", Models.AccountsSkin.GetFacefeatures(player.accountId), Models.AccountsSkin.GetHeadblend(player.accountId), Models.AccountsSkin.GetHeadoverlay(player.accountId));
                Models.AccountsSkin.requestCurrentSkin(player);
                HelperMethods.TriggerClientEvent(player, "Client:Charselector:spawnCharacterFinal");

                Position pos = PlayerDatabase.GetAccountLastPosition(player.accountId);
                player.Spawn(pos, 0);
                player.Position = pos;
                player.Dimension = PlayerDatabase.GetAccountLastDimension(player.accountId);
                HelperEvents.ClientEvent_setCefStatus(player, false);
                HelperMethods.TriggerClientEvent(player, "Client:Utilities:LoadBlips", System.Text.Json.JsonSerializer.Serialize(Models.ServerBlips.ServerBlips_));
                HelperMethods.TriggerClientEvent(player, "Client:Utilities:LoadPeds", System.Text.Json.JsonSerializer.Serialize(Models.ServerPeds.ServerPeds_));
                HelperMethods.TriggerClientEvent(player, "Client:Utilities:LoadMarkers", System.Text.Json.JsonSerializer.Serialize(Models.ServerMarker.ServerMarker_));
                player.updateTattoos();
                HelperMethods.TriggerClientEvent(player, "Client:HUD:createBrowser", player.cash);
                player.SendChatMessage($"Willkommen {player.accountName} - deine ID lautet: {player.accountId}.");
                await Task.Delay(5000);
                Models.ServerTattoos.GetAllTattoos(player);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
