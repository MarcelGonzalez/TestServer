using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Resources.Chat.Api;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MuchRP.Utils;

namespace MuchRP.Handler
{
    class ATMHandler : IScript
    {
        internal static async void openATM(MuchPlayer player)
        {
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0) return;
				//ToDo: Kontoverlauf
				await HelperEvents.ClientEvent_setCefStatus(player, true);
				HelperMethods.TriggerClientEvent(player, "Client:ATM:openATM", player.bank);
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
        }

		[AsyncClientEvent("Server:ATM:withdrawMoney")]
		public async Task ClientEvent_withdrawMoney(MuchPlayer player, int money)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || money <= 0) return;
				if(money > player.bank)
				{
					player.SendCustomNotification(4, 2000, $"Fehler: Du hast den ausgewählten Betrag nicht auf dem Konto ({money}$).");
					return;
				}

				player.SetPlayerBankCash(player.bank - money);
				player.SetPlayerCash(player.cash + money);
				player.SendCustomNotification(2, 2000, $"Du hast {money}$ ausgezahlt. Neuer Kontostand: {player.bank}$");
				HelperMethods.sendDiscordLog("ATM", $"**{player.accountName}** hat **{money}$** ausgezahlt.\nNeuer Kontostand: {player.bank}$.", "blue");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}

		[AsyncClientEvent("Server:ATM:depositMoney")]
		public async Task ClientEvent_depositMoney(MuchPlayer player, int money)
		{
			try
			{
				if (player == null || !player.Exists || player.accountId <= 0 || money <= 0) return;
				if (money > player.cash)
				{
					player.SendCustomNotification(4, 2000, $"Fehler: Du hast den ausgewählten Betrag nicht dabei ({money}$).");
					return;
				}

				player.SetPlayerBankCash(player.bank + money);
				player.SetPlayerCash(player.cash - money);
				player.SendCustomNotification(2, 2000, $"Du hast {money}$ eingezahlt. Neuer Kontostand: {player.bank}$");
				HelperMethods.sendDiscordLog("ATM", $"**{player.accountName}** hat **{money}$** eingezahlt.\nNeuer Kontostand: {player.bank}$.", "blue");
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}");
			}
		}
	}
}
