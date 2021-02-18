using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Factories
{
    public class MuchColshape : Checkpoint
    {
        public int colshapeId { get; set; } = 0;
        public float Radius { get; set; }
        public bool isCarDealerShape { get; set; } = false;
        public int carDealerShopId { get; set; } = 0;
        public uint carDealerHash { get; set; } = 0;

        public MuchColshape(IntPtr nativePointer) : base(nativePointer)
        {

        }

        public bool IsInRange(MuchPlayer player)
        {
            lock (player)
            {
                if (!player.Exists) return false;
                return player.Position.Distance(Position) <= Radius;
            }
        }
    }
}
