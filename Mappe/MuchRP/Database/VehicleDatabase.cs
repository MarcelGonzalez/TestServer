using AltV.Net.Data;
using MuchRP.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Database
{
    class VehicleDatabase
    {
        public static void AddVehicleModEntry(params int[] args)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO server_vehicles_modifications (vehId, colorPrimary, colorSecondary, colorPearl, headlightColor, spoiler, front_bumper, rear_bumper, side_skirt, exhaust, frame, grille, hood, fender, right_fender, roof, engine, brakes, transmission, horns, suspension, armor, turbo, xenon, wheel_type, wheels, wheelcolor, plate_holder, trim_design, ornaments, dial_design, steering_wheel, shift_lever, plaques, hydraulics, airfilter, window_tint, livery, plate, neon, neon_r, neon_g, neon_b, smoke_r, smoke_g, smoke_b) VALUES (@vehId, @colorPrimary, @colorSecondary, @colorPearl, @headlightColor, @spoiler, @front_bumper, @rear_bumper, @side_skirt, @exhaust, @frame, @grille, @hood, @fender, @right_fender, @roof, @engine, @brakes, @transmission, @horns, @suspension, @armor, @turbo, @xenon, @wheel_type, @wheels, @wheelcolor, @plate_holder, @trim_design, @ornaments, @dial_design, @steering_wheel, @shift_lever, @plaques, @hydraulics, @airfilter, @window_tint, @livery, @plate, @neon, @neon_r, @neon_g, @neon_b, @smoke_r, @smoke_g, @smoke_b)";

                    command.Parameters.AddWithValue("@vehId", args[1]);
                    command.Parameters.AddWithValue("@colorPrimary", args[2]);
                    command.Parameters.AddWithValue("@colorSecondary", args[3]);
                    command.Parameters.AddWithValue("@spoiler", args[4]);
                    command.Parameters.AddWithValue("@front_bumper", args[5]);
                    command.Parameters.AddWithValue("@rear_bumper", args[6]);
                    command.Parameters.AddWithValue("@side_skirt", args[7]);
                    command.Parameters.AddWithValue("@exhaust", args[8]);
                    command.Parameters.AddWithValue("@frame", args[9]);
                    command.Parameters.AddWithValue("@grille", args[10]);
                    command.Parameters.AddWithValue("@hood", args[11]);
                    command.Parameters.AddWithValue("@fender", args[12]);
                    command.Parameters.AddWithValue("@right_fender", args[13]);
                    command.Parameters.AddWithValue("@roof", args[14]);
                    command.Parameters.AddWithValue("@engine", args[15]);
                    command.Parameters.AddWithValue("@brakes", args[16]);
                    command.Parameters.AddWithValue("@transmission", args[17]);
                    command.Parameters.AddWithValue("@horns", args[18]);
                    command.Parameters.AddWithValue("@suspension", args[19]);
                    command.Parameters.AddWithValue("@armor", args[20]);
                    command.Parameters.AddWithValue("@turbo", args[21]);
                    command.Parameters.AddWithValue("@xenon", args[22]);
                    command.Parameters.AddWithValue("@wheel_type", args[23]);
                    command.Parameters.AddWithValue("@wheels", args[24]);
                    command.Parameters.AddWithValue("@wheelcolor", args[25]);
                    command.Parameters.AddWithValue("@plate_holder", args[26]);
                    command.Parameters.AddWithValue("@trim_design", args[27]);
                    command.Parameters.AddWithValue("@ornaments", args[28]);
                    command.Parameters.AddWithValue("@dial_design", args[29]);
                    command.Parameters.AddWithValue("@steering_wheel", args[30]);
                    command.Parameters.AddWithValue("@shift_lever", args[31]);
                    command.Parameters.AddWithValue("@plaques", args[32]);
                    command.Parameters.AddWithValue("@hydraulics", args[33]);
                    command.Parameters.AddWithValue("@airfilter", args[34]);
                    command.Parameters.AddWithValue("@window_tint", args[35]);
                    command.Parameters.AddWithValue("@livery", args[36]);
                    command.Parameters.AddWithValue("@plate", args[37]);
                    command.Parameters.AddWithValue("@neon", args[38]);
                    command.Parameters.AddWithValue("@neon_r", args[39]);
                    command.Parameters.AddWithValue("@neon_g", args[40]);
                    command.Parameters.AddWithValue("@neon_b", args[41]);
                    command.Parameters.AddWithValue("@smoke_r", args[42]);
                    command.Parameters.AddWithValue("@smoke_g", args[43]);
                    command.Parameters.AddWithValue("@smoke_b", args[44]);
                    command.Parameters.AddWithValue("@colorPearl", args[45]);
                    command.Parameters.AddWithValue("@headlightColor", args[46]);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e}");
                }
            }
        }

        internal static void AddVehicleToDB(int id, int ownerId, int secondOwnerId, uint hash, int faction, float fuel, float km, bool isInGarage, int garageId, Position pos, Rotation rot, string plate, DateTime lastUsage)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO server_vehicles (id, ownerId, secondOwnerId, hash, faction, fuel, km, isInGarage, garageId, posX, posY, posZ, rotX, rotY, rotZ, plate, lastUsage) VALUES (@id, @ownerId, @secondOwnerId, @hash, @faction, @fuel, @km, @isInGarage, @garageId, @posX, @posY, @posZ, @rotX, @rotY, @rotZ, @plate, @lastUsage)";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ownerId", ownerId);
                    command.Parameters.AddWithValue("@secondOwnerId", secondOwnerId);
                    command.Parameters.AddWithValue("@hash", hash);
                    command.Parameters.AddWithValue("@faction", faction);
                    command.Parameters.AddWithValue("@fuel", fuel);
                    command.Parameters.AddWithValue("@km", km);
                    command.Parameters.AddWithValue("@isInGarage", isInGarage);
                    command.Parameters.AddWithValue("@garageId", garageId);
                    command.Parameters.AddWithValue("@posX", pos.X);
                    command.Parameters.AddWithValue("@posY", pos.Y);
                    command.Parameters.AddWithValue("@posZ", pos.Z);
                    command.Parameters.AddWithValue("@rotX", rot.Pitch);
                    command.Parameters.AddWithValue("@rotY", rot.Roll);
                    command.Parameters.AddWithValue("@rotZ", rot.Yaw);
                    command.Parameters.AddWithValue("@plate", plate);
                    command.Parameters.AddWithValue("@lastUsage", lastUsage);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool ExistVehicleModEntry(int vehId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicles_modifications WHERE vehId=@vehId LIMIT 1";
                    command.Parameters.AddWithValue("@vehId", vehId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connection.Close();
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        internal static void UpdateVehicleLastPos(int vehicleId, Position position, Rotation rotation)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_vehicles SET posX=@posX, posY=@posY, posZ=@posZ, rotX=@rotX, rotY=@rotY, rotZ=@rotZ WHERE id=@id";
                    command.Parameters.AddWithValue("@id", vehicleId);
                    command.Parameters.AddWithValue("@posX", position.X);
                    command.Parameters.AddWithValue("@posY", position.Y);
                    command.Parameters.AddWithValue("@posZ", position.Z);
                    command.Parameters.AddWithValue("@rotX", rotation.Pitch);
                    command.Parameters.AddWithValue("@rotY", rotation.Roll);
                    command.Parameters.AddWithValue("@rotZ", rotation.Yaw);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void SetVehicleInGarageDB(int vehId, bool isInGarage, int garageId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_vehicles SET isInGarage=@isInGarage, garageId=@garageId WHERE id=@id";
                    command.Parameters.AddWithValue("@id", vehId);
                    command.Parameters.AddWithValue("@isInGarage", isInGarage);
                    command.Parameters.AddWithValue("@garageId", garageId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
