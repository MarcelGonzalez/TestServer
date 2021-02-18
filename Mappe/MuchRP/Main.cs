using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using MuchRP.Factories;
using MuchRP.Handler;
using MuchRP.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MuchRP
{
    internal class Main : AsyncResource
    {
        public static int mainThreadId = Thread.CurrentThread.ManagedThreadId;

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new AccountsFactory();
        }

        public override IBaseObjectFactory<IColShape> GetColShapeFactory()
        {
            return new ColshapeFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new VehicleFactory();
        }

        public override void OnStart()
        {
            Utils.Configuration.FetchConfiguration();
            Database.UtilsDatabase.LoadAllBlips();
            Database.UtilsDatabase.LoadAllShops();
            Database.UtilsDatabase.LoadAllServerItems();
            Database.UtilsDatabase.LoadAllShopItems();
            Database.UtilsDatabase.LoadAllATMs();
            Database.UtilsDatabase.LoadAllVehicleInfos();
            Database.UtilsDatabase.LoadAllVehicleModifications();
            Database.UtilsDatabase.LoadAllVehicles();
            Database.UtilsDatabase.LoadAllGarages();
            Database.UtilsDatabase.LoadAllVehicleShops();
            Database.UtilsDatabase.LoadAllVehicleShopItems();
            Database.UtilsDatabase.LoadAllTattoos();
            Database.UtilsDatabase.LoadAllTatooShops();
            Database.UtilsDatabase.LoadAllServerClothes();
            Database.UtilsDatabase.LoadAllServerClothesStorages();
            Database.PlayerDatabase.LoadAccountTattoos();
            Database.PlayerDatabase.LoadAccountClothes();
            Database.UtilsDatabase.LoadAllServerClothesShops();
            Database.PlayerDatabase.LoadAccountSkins();
            Database.UtilsDatabase.LoadAllFactions();
            Console.WriteLine("Server gestartet.");
            HelperMethods.sendDiscordLog("Server Status", "Der Server wurde erfolgreich gestartet.", "green");
            Alt.Log($"{Thread.CurrentThread.ManagedThreadId} - {mainThreadId}");

            //Timer
            System.Timers.Timer playerTimer = new System.Timers.Timer();
            playerTimer.Interval = 60000;
            playerTimer.Elapsed += TimerHandler.PlayerTimer;
            playerTimer.Enabled = true;

            System.Threading.Timer laborTimer = new System.Threading.Timer(TimerHandler.LaborTimer, null, 0, 60000);
        }

        public override void OnStop()
        {
            Console.WriteLine("Stopped");
        }

        [AsyncScriptEvent(ScriptEventType.PlayerConnect)]
        public async Task Connecthandler(MuchPlayer player, string reason)
        {
            if (player == null || !player.Exists) return;
            player.Model = 0x3D843282;
            player.Dimension = 10000;
            player.Position = new Position(3120, 5349, 10);
            player.Spawn(new Position(3120, 5349, 10), 0);
        }

        [AsyncScriptEvent(ScriptEventType.PlayerDead)]
        public async Task DeadEvent(MuchPlayer player, IEntity killer, uint reason)
        {
            if (player == null || !player.Exists) return;
            await player.SpawnAsync(new Position(player.Position.X, player.Position.Y, player.Position.Z + 1));
        }
    }
}
