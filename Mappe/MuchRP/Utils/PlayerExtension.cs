using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Utils
{
    public static partial class PlayerExtension
    {
        public static bool IsInRange(this Position currentPosition, Position otherPosition, float distance)
            => currentPosition.Distance(otherPosition) <= distance;

        public static bool IsCefOpen(this MuchPlayer player)
        {
            if (player == null || !player.Exists) return false;
            player.GetSyncedMetaData("IsCefOpen", out bool isCefOpened);
            return isCefOpened;
        }

        public static void updateTattoos(this MuchPlayer player)
        {
            if (player == null || !player.Exists || player.accountId <= 0) return;
            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setTattoos", Models.AccountsTattoos.GetAccountTattoos(player.accountId));
        }

        public static void SendCustomNotification(this MuchPlayer player, int type, int duration, string msg, int delay = 0)
        {
            if (player == null || !player.Exists) return;
            HelperMethods.TriggerClientEvent(player, "Client:HUD:sendNotification", type, duration, msg, delay);
        }

        public static void SetPlayerCash(this MuchPlayer player, int cash)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                player.cash = cash;
                Database.PlayerDatabase.UpdatePlayerCash(player.accountId, cash);
                HelperMethods.TriggerClientEvent(player, "Client:HUD:updateMoney", cash);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetPlayerBankCash(this MuchPlayer player, int cash)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                player.bank = cash;
                Database.PlayerDatabase.UpdatePlayerBankCash(player.accountId, cash);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
