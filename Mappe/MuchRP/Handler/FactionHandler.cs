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
    class FactionHandler : IScript
    {
        #region Faction Manager 
        internal static async void openFactionManager(MuchPlayer player, int factionId)
        {

            try
            {
                if (player == null || !player.Exists || player.IsCefOpen() || player.accountId <= 0 || factionId <= 0 || !ServerFactions.IsAccountInAnyFaction(player.accountId) || ServerFactions.GetAccountFaction(player.accountId) != factionId || !ServerFactions.IsRankALeaderShipment(factionId, ServerFactions.GetAccountRank(player.accountId))) return;
                HelperMethods.TriggerClientEvent(player, "Client:FactionManage:openFactionManager", factionId, ServerFactions.GetFactionMemberJSON(factionId));
                await HelperEvents.ClientEvent_setCefStatus(player, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [AsyncClientEvent("Server:FactionManage:rankUp")]
        public async Task rankUpMember(MuchPlayer player, int factionId, int targetAccountId)
        {

            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || player.accountId == targetAccountId || factionId <= 0 || targetAccountId <= 0 || !ServerFactions.IsAccountInAnyFaction(player.accountId) || ServerFactions.GetAccountFaction(player.accountId) != factionId || !ServerFactions.IsRankALeaderShipment(factionId, ServerFactions.GetAccountRank(player.accountId)) || !ServerFactions.IsAccountInAnyFaction(targetAccountId) || ServerFactions.GetAccountFaction(targetAccountId) != factionId) return;
                if(ServerFactions.GetAccountRank(targetAccountId) >= ServerFactions.GetAccountRank(player.accountId))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler hat einen höheren oder den gleichen Rang in deiner Fraktion als du.");
                    return;
                }

                if(ServerFactions.GetAccountRank(targetAccountId) == ServerFactions.GetFactionRankCount(factionId))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler hat bereits den höchsten Rang.");
                    return;
                }

                ServerFactions.SetAccountRank(factionId, targetAccountId, ServerFactions.GetAccountRank(targetAccountId) + 1);
                player.SendCustomNotification(2, 1500, $"Du hast den ausgewählten Spieler auf den Rang '{ServerFactions.GetFactionRankName(factionId, ServerFactions.GetAccountRank(targetAccountId))}' befördert.");
                HelperMethods.sendDiscordLog("Fraktionsverwaltung", $"**{player.accountName}** hat **{Database.PlayerDatabase.GetAccountName(targetAccountId)}** auf Rang {ServerFactions.GetAccountRank(targetAccountId)} ({ServerFactions.GetFactionRankName(factionId, ServerFactions.GetAccountRank(targetAccountId))} befördert.\nFraktion: {ServerFactions.GetFactionName(factionId)}", "blue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [AsyncClientEvent("Server:FactionManage:rankDown")]
        public async Task rankDownMember(MuchPlayer player, int factionId, int targetAccountId)
        {

            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || player.accountId == targetAccountId || factionId <= 0 || targetAccountId <= 0 || !ServerFactions.IsAccountInAnyFaction(player.accountId) || ServerFactions.GetAccountFaction(player.accountId) != factionId || !ServerFactions.IsRankALeaderShipment(factionId, ServerFactions.GetAccountRank(player.accountId)) || !ServerFactions.IsAccountInAnyFaction(targetAccountId) || ServerFactions.GetAccountFaction(targetAccountId) != factionId) return;
                if(ServerFactions.GetAccountRank(targetAccountId) >= ServerFactions.GetAccountRank(player.accountId))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler hat einen höheren oder den gleichen Rang in deiner Fraktion als du.");
                    return;
                }

                if(ServerFactions.GetAccountRank(targetAccountId) <= 1)
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler hat bereits den niedrigst möglichsten Rang.");
                    return;
                }

                ServerFactions.SetAccountRank(factionId, targetAccountId, ServerFactions.GetAccountRank(targetAccountId) - 1);
                player.SendCustomNotification(2, 1500, $"Du hast den ausgewählten Spieler auf den Rang '{ServerFactions.GetFactionRankName(factionId, ServerFactions.GetAccountRank(targetAccountId))}' degradiert.");
                HelperMethods.sendDiscordLog("Fraktionsverwaltung", $"**{player.accountName}** hat **{Database.PlayerDatabase.GetAccountName(targetAccountId)}** auf Rang {ServerFactions.GetAccountRank(targetAccountId)} ({ServerFactions.GetFactionRankName(factionId, ServerFactions.GetAccountRank(targetAccountId))} degradiert.\nFraktion: {ServerFactions.GetFactionName(factionId)}", "blue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [AsyncClientEvent("Server:FactionManage:removeMember")]
        public async Task removeMember(MuchPlayer player, int factionId, int targetAccountId)
        {

            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || player.accountId == targetAccountId || factionId <= 0 || targetAccountId <= 0 || !ServerFactions.IsAccountInAnyFaction(player.accountId) || ServerFactions.GetAccountFaction(player.accountId) != factionId || !ServerFactions.IsRankALeaderShipment(factionId, ServerFactions.GetAccountRank(player.accountId)) || !ServerFactions.IsAccountInAnyFaction(targetAccountId) || ServerFactions.GetAccountFaction(targetAccountId) != factionId) return;
                if (ServerFactions.GetAccountRank(targetAccountId) >= ServerFactions.GetAccountRank(player.accountId))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler hat einen höheren oder den gleichen Rang in deiner Fraktion als du.");
                    return;
                }
                ServerFactions.RemoveFactionMember(factionId, targetAccountId);
                player.SendCustomNotification(2, 1500, $"Du hast den ausgewählten Spieler aus der Fraktion '{ServerFactions.GetFactionName(factionId)}' entlassen.");
                HelperMethods.sendDiscordLog("Fraktionsverwaltung", $"**{player.accountName}** hat **{Database.PlayerDatabase.GetAccountName(targetAccountId)}** aus der Fraktion geworfen.\nFraktion: {ServerFactions.GetFactionName(factionId)}", "blue");

                MuchPlayer targetPlayer = (MuchPlayer)Alt.Server.GetPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && ((MuchPlayer)x).accountId == targetAccountId);
                if (targetPlayer != null && targetPlayer.Exists) targetPlayer.SendCustomNotification(2, 1500, $"Du wurdest aus der Fraktion '{ServerFactions.GetFactionName(factionId)}' entlassen.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [AsyncClientEvent("Server:FactionManage:inviteMember")]
        public async Task inviteMember(MuchPlayer player, string targetName)
        {

            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || !ServerFactions.IsAccountInAnyFaction(player.accountId) || !ServerFactions.IsRankALeaderShipment(ServerFactions.GetAccountFaction(player.accountId), ServerFactions.GetAccountRank(player.accountId))) return;
                MuchPlayer targetPlayer = (MuchPlayer)Alt.Server.GetPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && ((MuchPlayer)x).accountName == targetName);
                if(targetPlayer == null || !targetPlayer.Exists || !targetPlayer.Position.IsInRange(player.Position, 5f))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler ist nicht online oder nicht in deiner Nähe.");
                    return;
                }

                if(ServerFactions.IsAccountInAnyFaction(targetPlayer.accountId))
                {
                    player.SendCustomNotification(4, 1500, "Fehler: Der Spieler ist bereits in einer anderen Fraktion.");
                    return;
                }
                
                ServerFactions.AddFactionMember(ServerFactions.GetAccountFaction(player.accountId), targetPlayer.accountId, 1);
                player.SendCustomNotification(2, 1500, $"Du hast den Spieler '{targetName}' in deine Fraktion '{ServerFactions.GetFactionName(ServerFactions.GetAccountFaction(player.accountId))}' eingeladen.");
                targetPlayer.SendCustomNotification(1, 1500, $"Der Spieler '{player.accountName}' hat dich in seine Fraktion '{ServerFactions.GetFactionName(ServerFactions.GetAccountFaction(player.accountId))}' eingeladen.");
                HelperMethods.sendDiscordLog("Fraktionsverwaltung", $"**{player.accountName}** hat **{targetName}** in die Fraktion aufgenommen.\nFraktion: {ServerFactions.GetFactionName(ServerFactions.GetAccountFaction(player.accountId))}", "blue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion
    }
}
