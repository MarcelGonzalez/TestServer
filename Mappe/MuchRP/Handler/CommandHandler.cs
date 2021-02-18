using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Resources.Chat.Api;
using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Handler
{
    class CommandHandler : IScript
    {
        [Command("pos")]
        public static void CMD_Pos(MuchPlayer player)
        {
            if (!player.IsInVehicle)
            {
                player.SendChatMessage($"{player.Position.ToString()}");
                Alt.Log($"{player.Position.ToString()}");
            }
            else
            {
                player.Vehicle.EngineOn = true;
                player.SendChatMessage($"{player.Vehicle.Position.ToString()}");
                player.SendChatMessage($"{player.Vehicle.Rotation.ToString()}");
                Alt.Log($"{player.Vehicle.Position.ToString()}");
                Alt.Log($"{player.Vehicle.Rotation.ToString()}");
            }
        }

        [Command("setpos")]
        public static void CMD_SetPos(MuchPlayer player, float X, float Y, float Z)
        {
            player.Position = new Position(X, Y, Z);
        }

        [Command("veh")]
        public static void CMD_Veh(MuchPlayer player, string model)
        {
            MuchVehicle veh = (MuchVehicle)Alt.CreateVehicle(Alt.Hash(model), player.Position, player.Rotation);
            veh.ManualEngineControl = true;
            veh.EngineOn = true;
        }

        [Command("giveitem")]
        public static void CMD_GiveItem(MuchPlayer player, string itemName, int itemAmount)
        {
            try
            {
                Models.AccountsInventory.AddCharacterItem(player, itemName, itemAmount);
                player.SendChatMessage($"{itemName} ({itemAmount}x) hinzugefügt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
