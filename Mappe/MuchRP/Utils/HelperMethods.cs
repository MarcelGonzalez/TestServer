using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MuchRP.Utils
{
    public class HelperMethods : IScript
    {
        public static async Task sendDiscordLog(string title, string desc, string color)
        {
            Alt.Emit("DiscordBot:DiscordLog", $"{title}", $"{desc}", $"{color}");
        }

        internal static void TriggerClientEvent(IPlayer player, string eventName, params object[] args)
        {
            try
            {
                if (player == null) return;
                if (Thread.CurrentThread.ManagedThreadId != Main.mainThreadId) player.EmitLocked(eventName, args);
                else player.Emit(eventName, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static MuchColshape CreateColShapeSphere(Position position, int dimension, float range)
        {
            if(Main.mainThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                return AltAsync.Do(() =>
                {
                    MuchColshape colshape = (MuchColshape)Alt.CreateColShapeSphere(position, range);
                    colshape.Dimension = dimension;
                    //colshape.IsPlayersOnly = isPlayerOnly;
                    return colshape;
                }).Result;
            }
            else
            {
                MuchColshape colshape = (MuchColshape)Alt.CreateColShapeSphere(position, range);
                colshape.Dimension = dimension;
                //colshape.IsPlayersOnly = isPlayerOnly;
                return colshape;
            }
        }

        internal static MuchVehicle CreateVehicle(uint model, Position position, Rotation rotation, int dimension, string numberplate = "", byte primaryColor = 0, byte secondaryColor = 0)
        {
            if(Main.mainThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                return AltAsync.Do(() =>
                {
                    MuchVehicle vehicle = (MuchVehicle)Alt.CreateVehicle(model, position, rotation);
                    vehicle.Dimension = dimension;
                    vehicle.NumberplateText = numberplate;
                    vehicle.PrimaryColor = primaryColor;
                    vehicle.SecondaryColor = secondaryColor;
                    return vehicle;
                }).Result;
            }
            else
            {
                MuchVehicle vehicle = (MuchVehicle)Alt.CreateVehicle(model, position, rotation);
                vehicle.Dimension = dimension;
                vehicle.NumberplateText = numberplate;
                vehicle.PrimaryColor = primaryColor;
                vehicle.SecondaryColor = secondaryColor;
                return vehicle;
            }
        }
    }
}
