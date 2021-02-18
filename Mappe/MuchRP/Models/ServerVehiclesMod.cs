using AltV.Net;
using AltV.Net.Data;
using MuchRP.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerVehiclesMod : IScript
    {
        public partial class Server_Vehicles_Mod
        {
            public int id { get; set; }
            public int vehId { get; set; }
            public int colorPrimary { get; set; }
            public int colorSecondary { get; set; }
            public int colorPearl { get; set; }
            public int headlightColor { get; set; }
            public int spoiler { get; set; }
            public int front_bumper { get; set; }
            public int rear_bumper { get; set; }
            public int side_skirt { get; set; }
            public int exhaust { get; set; }
            public int frame { get; set; }
            public int grille { get; set; }
            public int hood { get; set; }
            public int fender { get; set; }
            public int right_fender { get; set; }
            public int roof { get; set; }
            public int engine { get; set; }
            public int brakes { get; set; }
            public int transmission { get; set; }
            public int horns { get; set; }
            public int suspension { get; set; }
            public int armor { get; set; }
            public int turbo { get; set; }
            public int xenon { get; set; }
            public int wheel_type { get; set; }
            public int wheels { get; set; }
            public int wheelcolor { get; set; }
            public int plate_holder { get; set; }
            public int trim_design { get; set; }
            public int ornaments { get; set; }
            public int dial_design { get; set; }
            public int steering_wheel { get; set; }
            public int shift_lever { get; set; }
            public int plaques { get; set; }
            public int hydraulics { get; set; }
            public int airfilter { get; set; }
            public int window_tint { get; set; }
            public int livery { get; set; }
            public int plate { get; set; }
            public int neon { get; set; }
            public int neon_r { get; set; }
            public int neon_g { get; set; }
            public int neon_b { get; set; }
            public int smoke_r { get; set; }
            public int smoke_g { get; set; }
            public int smoke_b { get; set; }
        }

        public static List<Server_Vehicles_Mod> ServerVehiclesMod_ = new List<Server_Vehicles_Mod>();

        public static void AddVehicleModToL(params int[] args)
        {
            try
            {
                var vehMods = new Server_Vehicles_Mod
                {
                    id = args[0],
                    vehId = args[1],
                    colorPrimary = (byte)args[2],
                    colorSecondary = (byte)args[3],
                    spoiler = (byte)args[4],
                    front_bumper = (byte)args[5],
                    rear_bumper = (byte)args[6],
                    side_skirt = (byte)args[7],
                    exhaust = (byte)args[8],
                    frame = (byte)args[9],
                    grille = (byte)args[10],
                    hood = (byte)args[11],
                    fender = (byte)args[12],
                    right_fender = (byte)args[13],
                    roof = (byte)args[14],
                    engine = (byte)args[15],
                    brakes = (byte)args[16],
                    transmission = (byte)args[17],
                    horns = (byte)args[18],
                    suspension = (byte)args[19],
                    armor = (byte)args[20],
                    turbo = (byte)args[21],
                    xenon = (byte)args[22],
                    wheel_type = (byte)args[23],
                    wheels = (byte)args[24],
                    wheelcolor = (byte)args[25],
                    plate_holder = (byte)args[26],
                    trim_design = (byte)args[27],
                    ornaments = (byte)args[28],
                    dial_design = (byte)args[29],
                    steering_wheel = (byte)args[30],
                    shift_lever = (byte)args[31],
                    plaques = (byte)args[32],
                    hydraulics = (byte)args[33],
                    airfilter = (byte)args[34],
                    window_tint = (byte)args[35],
                    livery = (byte)args[36],
                    plate = (byte)args[37],
                    neon = (byte)args[38],
                    neon_r = (byte)args[39],
                    neon_g = (byte)args[40],
                    neon_b = (byte)args[41],
                    smoke_r = (byte)args[42],
                    smoke_g = (byte)args[43],
                    smoke_b = (byte)args[44],
                    colorPearl = (byte)args[45],
                    headlightColor = (byte)args[46]
                };

                ServerVehiclesMod_.Add(vehMods);
                if (Database.VehicleDatabase.ExistVehicleModEntry(vehMods.vehId)) return;
                Database.VehicleDatabase.AddVehicleModEntry(vehMods.id, vehMods.vehId, vehMods.colorPrimary, vehMods.colorSecondary, vehMods.spoiler, vehMods.front_bumper, vehMods.rear_bumper, vehMods.side_skirt, vehMods.exhaust, vehMods.frame, vehMods.grille, vehMods.hood, vehMods.fender, vehMods.right_fender, vehMods.roof, vehMods.engine, vehMods.brakes, vehMods.transmission, vehMods.horns, vehMods.suspension, vehMods.armor, vehMods.turbo, vehMods.xenon, vehMods.wheel_type, vehMods.wheels, vehMods.wheelcolor, vehMods.plate_holder, vehMods.trim_design, vehMods.ornaments, vehMods.dial_design, vehMods.steering_wheel, vehMods.shift_lever, vehMods.plaques, vehMods.hydraulics, vehMods.airfilter, vehMods.window_tint, vehMods.livery, vehMods.plate, vehMods.neon, vehMods.neon_r, vehMods.neon_g, vehMods.neon_g, vehMods.smoke_r, vehMods.smoke_g, vehMods.smoke_b, vehMods.colorPearl, vehMods.headlightColor);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetVehicleModifications(MuchVehicle veh)
        {
            try
            {
                if (veh == null || !veh.Exists || veh.vehicleId <= 0) return;
                veh.ModKit = 1;
                for (var i = 0; i <= 16; i++)
                    veh.SetMod((byte)i, GetCurrentVehMod(veh, i));

                veh.SetMod(18, GetCurrentVehMod(veh, 18));
                veh.SetMod(22, GetCurrentVehMod(veh, 22));
                veh.SetWheels(0, GetCurrentVehMod(veh, 23));
                veh.SetMod(25, GetCurrentVehMod(veh, 25));
                veh.SetMod(27, GetCurrentVehMod(veh, 27));
                veh.SetMod(28, GetCurrentVehMod(veh, 28));
                veh.SetMod(30, GetCurrentVehMod(veh, 30));
                veh.SetMod(33, GetCurrentVehMod(veh, 33));
                veh.SetMod(34, GetCurrentVehMod(veh, 34));
                veh.SetMod(35, GetCurrentVehMod(veh, 35));
                veh.SetMod(38, GetCurrentVehMod(veh, 38));
                veh.SetMod(40, GetCurrentVehMod(veh, 40));
                veh.SetMod(48, GetCurrentVehMod(veh, 48));
                veh.SetMod(62, GetCurrentVehMod(veh, 62));
                veh.PrimaryColor = GetCurrentVehMod(veh, 100);
                veh.SecondaryColor = GetCurrentVehMod(veh, 200);
                veh.PearlColor = GetCurrentVehMod(veh, 250);
                veh.HeadlightColor = GetCurrentVehMod(veh, 280);
                veh.WheelColor = GetCurrentVehMod(veh, 132); 
                veh.CustomTires = true;
                veh.TireSmokeColor = new Rgba(GetCurrentVehMod(veh, 400), GetCurrentVehMod(veh, 401), GetCurrentVehMod(veh, 402), 255);
                veh.WindowTint = GetCurrentVehMod(veh, 46);

                if (GetCurrentVehMod(veh, 299) == 1)
                {
                    veh.SetNeonActive(true, true, true, true); 
                    veh.NeonColor = new Rgba(GetCurrentVehMod(veh, 300), GetCurrentVehMod(veh, 301), GetCurrentVehMod(veh, 302), 1);
                }
                else if (GetCurrentVehMod(veh, 299) == 0) 
                { 
                    veh.SetNeonActive(false, false, false, false); 
                    veh.NeonColor = new Rgba(GetCurrentVehMod(veh, 300), GetCurrentVehMod(veh, 301), GetCurrentVehMod(veh, 302), 1); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static byte GetCurrentVehMod(MuchVehicle veh, int modTyp)
        {
            try
            {
                if (veh == null || !veh.Exists || veh.vehicleId <= 0) return 0;
                var vehicleMod = ServerVehiclesMod_.ToList().FirstOrDefault(x => x.vehId == veh.vehicleId);
                if (vehicleMod == null) return 0;
                switch(modTyp)
                {
                    case 0: return (byte)vehicleMod.spoiler;
                    case 1: return (byte)vehicleMod.front_bumper;
                    case 2: return (byte)vehicleMod.rear_bumper;
                    case 3: return (byte)vehicleMod.side_skirt;
                    case 4: return (byte)vehicleMod.exhaust;
                    case 5: return (byte)vehicleMod.frame;
                    case 6: return (byte)vehicleMod.grille;
                    case 7: return (byte)vehicleMod.hood;
                    case 8: return (byte)vehicleMod.fender;
                    case 9: return (byte)vehicleMod.right_fender;
                    case 10: return (byte)vehicleMod.roof;
                    case 11: return (byte)vehicleMod.engine;
                    case 12: return (byte)vehicleMod.brakes;
                    case 13: return (byte)vehicleMod.transmission;
                    case 14: return (byte)vehicleMod.horns;
                    case 15: return (byte)vehicleMod.suspension;
                    case 16: return (byte)vehicleMod.armor;
                    case 18: return (byte)vehicleMod.turbo;
                    case 22: return (byte)vehicleMod.xenon;
                    case 131: return (byte)vehicleMod.wheel_type;
                    case 23: return (byte)vehicleMod.wheels;
                    case 132: return (byte)vehicleMod.wheelcolor;
                    case 25: return (byte)vehicleMod.plate_holder;
                    case 27: return (byte)vehicleMod.trim_design;
                    case 28: return (byte)vehicleMod.ornaments;
                    case 30: return (byte)vehicleMod.dial_design;
                    case 33: return (byte)vehicleMod.steering_wheel;
                    case 34: return (byte)vehicleMod.shift_lever;
                    case 35: return (byte)vehicleMod.plaques;
                    case 38: return (byte)vehicleMod.hydraulics;
                    case 40: return (byte)vehicleMod.airfilter;
                    case 46: return (byte)vehicleMod.window_tint;
                    case 48: return (byte)vehicleMod.livery;
                    case 62: return (byte)vehicleMod.plate;
                    case 100: return (byte)vehicleMod.colorPrimary;
                    case 200: return (byte)vehicleMod.colorSecondary;
                    case 250: return (byte)vehicleMod.colorPearl;
                    case 280: return (byte)vehicleMod.headlightColor;
                    case 299: return (byte)vehicleMod.neon;
                    case 300: return (byte)vehicleMod.neon_r;
                    case 301: return (byte)vehicleMod.neon_g;
                    case 302: return (byte)vehicleMod.neon_b;
                    case 400: return (byte)vehicleMod.smoke_r;
                    case 401: return (byte)vehicleMod.smoke_g;
                    case 402: return (byte)vehicleMod.smoke_b;
                    default: return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }
    }
}
