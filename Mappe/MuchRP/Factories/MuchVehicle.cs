using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Factories
{
    public class MuchVehicle : Vehicle
    {
        public int vehicleId { get; set; } = 0;
        public bool isCardealerVehicle { get; set; } = false;
        public int factionId { get; set; } = 0;

        public MuchVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
        }

        public MuchVehicle(uint model, Position position, Rotation rotation) : base(model, position, rotation)
        {
        }
    }
}
