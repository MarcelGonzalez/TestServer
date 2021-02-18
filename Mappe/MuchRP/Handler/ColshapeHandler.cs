using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using MuchRP.Factories;
using MuchRP.Models;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Handler
{
    class ColshapeHandler : IScript
    {
        [ScriptEvent(ScriptEventType.ColShape)]
        public async Task ScriptEvent_ColShape(MuchColshape colShape, IEntity entity, bool state)
        {
            try
            {
                if (colShape == null || !colShape.Exists || entity == null || !entity.Exists) return;

                // Vehicle Shop Notifications
                if(state && colShape.isCarDealerShape && colShape.carDealerHash != 0 && entity is MuchPlayer player && player != null && player.Exists && player.accountId != 0) player.SendCustomNotification(1, 2500, $"Name: {ServerVehicleInfo.GetDisplayName(colShape.carDealerHash)}<br>Preis: {ServerVehicleShops.GetVehiclePrice(colShape.carDealerShopId, colShape.carDealerHash)}$");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
