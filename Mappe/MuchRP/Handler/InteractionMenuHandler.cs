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
    class InteractionMenuHandler : IScript
    {
        #region Player Interactions
        [AsyncClientEvent("Server:Interaction:GetPlayerInfo")]
        public async Task ClientEvent_GetPlayerInfo(MuchPlayer player, MuchPlayer targetPlayer)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || targetPlayer == null || !targetPlayer.Exists || targetPlayer.accountId <= 0) return;
                string interactHTML = "";
                interactHTML += "<li><p id='InteractionMenu-Title'>Schließen</p></li><li class='interactitem' data-action='close' data-actionstring='Schließen'><img src='../utils/img/cancel.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-showSupportId' data-action='showSupportId' data-actionstring='Spieler-ID anzeigen'><img src='../utils/img/showSupportId.png'></li>";
                HelperMethods.TriggerClientEvent(player, "Client:Interaction:SetInfo", "player", interactHTML);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Interaction:showSupportId")]
        public async Task ClientEvent_showSupportId(MuchPlayer player, MuchPlayer targetPlayer)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || targetPlayer == null || !targetPlayer.Exists || targetPlayer.accountId <= 0 || player == targetPlayer) return;
                player.SendCustomNotification(1, 2500, $"Die Spieler-ID der Person lautet: {targetPlayer.accountId}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region Vehicle Interactions
        [AsyncClientEvent("Server:Interaction:GetVehicleInfo")]
        public async Task ClientEvent_GetVehicleInfo(MuchPlayer player, string type, MuchVehicle targetVehicle)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || targetVehicle == null || !targetVehicle.Exists || targetVehicle.vehicleId <= 0) return;
                string interactHTML = "";
                interactHTML += "<li><p id='InteractionMenu-Title'>Schließen</p></li><li class='interactitem' data-action='close' data-actionstring='Schließen'><img src='../utils/img/close.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-lockVehicle' data-action='lockVehicle' data-actionstring='Fahrzeug auf- / abschließen'><img src='../utils/img/lockVehicle.png'></li>";

                if(player.IsInVehicle) interactHTML += "<li class='interactitem' id='InteractionMenu-engineVehicle' data-action='engineVehicle' data-actionstring='Motor betätigen'><img src='../utils/img/engineVehicle.png'></li>";
                HelperMethods.TriggerClientEvent(player, "Client:Interaction:SetInfo", type, interactHTML);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Interaction:lockVehicle")]
        public async Task ClientEvent_lockVehicle(MuchPlayer player, MuchVehicle targetVehicle)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || targetVehicle == null || !targetVehicle.Exists || targetVehicle.vehicleId <= 0 || !player.Position.IsInRange(targetVehicle.Position, 8f)) return;
                if ((ServerVehicles.GetVehicleOwner(targetVehicle.vehicleId) != player.accountId && ServerVehicles.GetVehicleSecondaryOwner(targetVehicle.vehicleId) != player.accountId) && ServerFactions.GetAccountFaction(player.accountId) != targetVehicle.factionId) return;
                if (ServerVehicles.IsVehicleLocked(targetVehicle.vehicleId))
                {
                    await AltAsync.Do(() => { targetVehicle.LockState = AltV.Net.Enums.VehicleLockState.Unlocked; });
                    player.SendCustomNotification(2, 1500, "Fahrzeug aufgeschlossen.");
                }
                else
                {
                    await AltAsync.Do(() => { targetVehicle.LockState = AltV.Net.Enums.VehicleLockState.Locked; });
                    player.SendCustomNotification(2, 1500, "Fahrzeug abgeschlossen.");
                }
                ServerVehicles.SetVehicleLocked(targetVehicle.vehicleId, !ServerVehicles.IsVehicleLocked(targetVehicle.vehicleId));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Interaction:engineVehicle")]
        public async Task ClientEvent_engineVehicle(MuchPlayer player, MuchVehicle targetVehicle)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || targetVehicle == null || !targetVehicle.Exists || targetVehicle.vehicleId <= 0 || player.Vehicle != targetVehicle) return;
                if ((ServerVehicles.GetVehicleOwner(targetVehicle.vehicleId) != player.accountId && ServerVehicles.GetVehicleSecondaryOwner(targetVehicle.vehicleId) != player.accountId) && ServerFactions.GetAccountFaction(player.accountId) != targetVehicle.factionId) return;
                if (targetVehicle.EngineOn)
                    player.SendCustomNotification(2, 1500, "Motor gestoppt.");
                else player.SendCustomNotification(2, 1500, "Motor gestartet.");


                AltAsync.Do(() => { targetVehicle.EngineOn = !targetVehicle.EngineOn; });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion
    }
}
