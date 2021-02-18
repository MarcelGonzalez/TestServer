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
    class GarageHandler : IScript
    {
        internal static async void openGarage(MuchPlayer player, ServerGarages.Server_Garages garageData)
        {
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || garageData == null) return;
				if(garageData.faction > 0)
				{
					//Faction Garage
					if (!ServerFactions.IsAccountInAnyFaction(player.accountId) || ServerFactions.GetAccountFaction(player.accountId) != garageData.faction)
					{
						player.SendCustomNotification(4, 1500, "Du hast hier keinen Zugriff drauf (Fraktionsgarage).");
						return;
					}

					string garageVehicles = ServerGarages.GetGarageParkedVehicles(player.accountId, garageData.id, garageData.faction);
					string outsideVehicles = ServerGarages.GetGarageOutsideVehicles(player.accountId, garageData.id, garageData.faction, garageData.vehClass, new AltV.Net.Data.Position(garageData.pedX, garageData.pedY, garageData.pedZ));
					await HelperEvents.ClientEvent_setCefStatus(player, true);
					HelperMethods.TriggerClientEvent(player, "Client:Garage:openGarage", garageData.id, garageVehicles, outsideVehicles);
				}
				else if(garageData.faction == 0)
				{
					//Privat Garage
					string garageVehicles = ServerGarages.GetGarageParkedVehicles(player.accountId, garageData.id, 0);
					string outsideVehicles = ServerGarages.GetGarageOutsideVehicles(player.accountId, garageData.id, 0, garageData.vehClass, new AltV.Net.Data.Position(garageData.pedX, garageData.pedY, garageData.pedZ));
					await HelperEvents.ClientEvent_setCefStatus(player, true);
					HelperMethods.TriggerClientEvent(player, "Client:Garage:openGarage", garageData.id, garageVehicles, outsideVehicles);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
        }		

		[AsyncClientEvent("Server:Garage:storeVehicle")]
		public async Task ClientEvent_storeVehicle(MuchPlayer player, int garageId, int vehicleId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || garageId <= 0 || vehicleId <= 0) return;
				var garageData = ServerGarages.ServerGarages_.ToList().FirstOrDefault(x => x.id == garageId);
				MuchVehicle veh = (MuchVehicle)Alt.Server.GetVehicles().FirstOrDefault(x => x != null && x.Exists && ((MuchVehicle)x).vehicleId == vehicleId);
				if (garageData == null || veh == null || !veh.Exists || !veh.Position.IsInRange(new AltV.Net.Data.Position(garageData.pedX, garageData.pedY, garageData.pedZ), 15f)) return;

				ServerVehicles.UpdateVehicleLastPosition(veh);
				ServerVehicles.SetVehicleInGarage(veh.vehicleId, true, garageId);
				veh.Remove();
				HelperMethods.sendDiscordLog("Garage", $"**{player.accountName}** hat das Fahrzeug **{ServerVehicleInfo.GetDisplayName(veh.Model)}** eingeparkt.\nGarage-ID: {garageId} || Vehicle-ID: {vehicleId}", "blue");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:Garage:takeVehicle")]
		public async Task ClientEvent_takeVehicle(MuchPlayer player, int garageId, int vehicleId)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || garageId <= 0 || vehicleId <= 0) return;
				var garageData = ServerGarages.ServerGarages_.ToList().FirstOrDefault(x => x.id == garageId);
				var dbVehicle = ServerVehicles.ServerVehicles_.FirstOrDefault(x => x.id == vehicleId);
				if (garageData == null || dbVehicle == null || !dbVehicle.isInGarage || dbVehicle.garageId != garageId) return;
				MuchVehicle veh = (MuchVehicle)Alt.Server.GetVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && ((MuchVehicle)x).vehicleId != 0 && x.Position.IsInRange(new AltV.Net.Data.Position(garageData.outX, garageData.outY, garageData.outZ), 2f));
				if(veh != null)
				{
					player.SendCustomNotification(4, 1500, "Der Ausparkpunkt ist belegt.");
					return;
				}

				ServerVehicles.SetVehicleInGarage(vehicleId, false, dbVehicle.garageId);
				MuchVehicle vehicle = HelperMethods.CreateVehicle(dbVehicle.hash, new AltV.Net.Data.Position(garageData.outX, garageData.outY, garageData.outZ), new AltV.Net.Data.Rotation(0, 0, garageData.outRotZ), 0, dbVehicle.plate, 0, 0);
				vehicle.LockState = AltV.Net.Enums.VehicleLockState.Locked;
				vehicle.ManualEngineControl = true;
				vehicle.EngineOn = false;
				vehicle.vehicleId = dbVehicle.id;
				vehicle.factionId = dbVehicle.faction;
				ServerVehiclesMod.SetVehicleModifications(vehicle);
				ServerVehicles.UpdateVehicleLastPosition(vehicle);
				HelperMethods.sendDiscordLog("Garage", $"**{player.accountName}** hat das Fahrzeug **{ServerVehicleInfo.GetDisplayName(vehicle.Model)}** ausgeparkt.\nGarage-ID: {garageId} || Vehicle-ID: {vehicleId}", "blue");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
    }
}
