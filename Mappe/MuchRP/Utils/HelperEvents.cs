using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Utils
{
    class HelperEvents : IScript
    {
        [AsyncClientEvent("Server:CEF:setCefStatus")]
        public async static Task ClientEvent_setCefStatus(MuchPlayer player, bool state)
        {
            try
            {
                if (player == null || !player.Exists) return;
                await AltAsync.Do(() =>
                {
                    player.SetSyncedMetaData("IsCefOpen", state);
                });
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Player:setPos")]
        public async Task ClientEvent_SetPos(MuchPlayer player, float X, float Y, float Z)
        {
            try
            {
                if (player == null || !player.Exists) return;
                player.Position = new Position(X, Y, Z);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
