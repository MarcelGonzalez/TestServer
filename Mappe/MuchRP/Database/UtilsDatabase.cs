using MuchRP.Models;
using MuchRP.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Database
{
    class UtilsDatabase
    {
        public static void LoadAllBlips()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_blips";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                            ServerBlips.AddBlipToList(reader.GetString("name"), reader.GetInt32("color"), reader.GetFloat("scale"), true, reader.GetInt32("sprite"), reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerBlips.ServerBlips_.Count} Server-Blips geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllServerClothesStorages()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_clothes_storages";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerClothesStorages.AddEntryToList(reader.GetInt32("id"), reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"), reader.GetInt32("faction"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerClothesStorages.ServerClothesStorages_.Count} Server-Clothes-Storages geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllServerClothesShops()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_clothesshops";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServerBlips.AddBlipToList("Kleiderladen", 0, 0.7f, true, 73, reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"));
                            ServerClothesShops.AddShopEntryToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"), reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"), reader.GetString("pedModel"));
                        }
                    }

                    command.CommandText = "SELECT * FROM server_clothesshops_items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerClothesShops.AddItemEntryToList(reader.GetInt32("id"), reader.GetInt32("shopId"), reader.GetString("clothesName"), reader.GetInt32("price"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerClothesShops.ServerClothesShops_.Count} Server-Clothes-Shops geladen.");
                Console.WriteLine($"[SERVER] {ServerClothesShops.ServerClothesShopItems_.Count} Server-Clothes-Shop-Items geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllFactions()
        {

            try
            {
                using(var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_factions";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerFactions.AddFactionEntryToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetString("shortcut"), reader.GetInt32("money"), reader.GetInt32("phoneNumber"), reader.GetInt32("type"), new AltV.Net.Data.Position(reader.GetFloat("manageX"), reader.GetFloat("manageY"), reader.GetFloat("manageZ")), new AltV.Net.Data.Position(reader.GetFloat("laborEntryX"), reader.GetFloat("laborEntryY"), reader.GetFloat("laborEntryZ")));
                    }

                    command.CommandText = "SELECT * FROM server_faction_ranks";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerFactions.AddFactionRankEntryToList(reader.GetInt32("id"), reader.GetInt32("factionId"), reader.GetInt32("rankId"), reader.GetString("name"), reader.GetInt32("payday"));
                    }

                    command.CommandText = "SELECT * FROM server_faction_members";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerFactions.AddFactionMemberEntryToList(reader.GetInt32("id"), reader.GetInt32("factionId"), reader.GetInt32("accountId"), reader.GetInt32("rankId"));
                    }

                    command.CommandText = "SELECT * FROM server_faction_labor_items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerFactions.AddFactionLaborItemToList(reader.GetInt32("id"), reader.GetInt32("factionId"), reader.GetInt32("accountId"), reader.GetString("itemName"), reader.GetInt32("itemAmount"));
                    }

                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerFactions.ServerFactions_.Count} Server-Factions geladen.");
                Console.WriteLine($"[SERVER] {ServerFactions.ServerFactionRanks_.Count} Server-Faction-Ranks geladen.");
                Console.WriteLine($"[SERVER] {ServerFactions.ServerFactionMembers_.Count} Server-Faction-Members geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void LoadAllServerClothes()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_clothes";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerClothes.AddEntryToList(reader.GetInt32("id"), reader.GetString("clothesName"), reader.GetString("type"), reader.GetInt32("draw"), reader.GetInt32("texture"), reader.GetInt32("gender"), reader.GetInt32("faction"));
                    }
                    connection.Close();                    
                }
                Console.WriteLine($"[SERVER] {ServerClothes.ServerClothes_.Count} Server-Clothes geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllTatooShops()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_tattooshops";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                            ServerTattooShops.AddShopToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("owner"), reader.GetInt32("bank"), reader.GetInt32("price"), reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetString("pedModel"), reader.GetFloat("pedRot"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerTattooShops.ServerTattooShops_.Count} Server-Tattoo-Shops geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllTattoos()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_tattoos";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                            ServerTattoos.AddTattooToList(reader.GetInt32("id"), reader.GetString("collection"), reader.GetString("nameHash"), reader.GetString("name"), reader.GetString("part"), reader.GetInt32("price"), reader.GetInt32("gender"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerTattoos.ServerTattoos_.Count} Server-Tattoos geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllVehicleShops()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicleshops";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            ServerBlips.AddBlipToList($"Fahrzeughandel: {reader.GetString("name")}", 9, 0.7f, true, 225, reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"));
                            ServerPeds.AddPedToList("ig_car3guy1", reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"));
                            ServerVehicleShops.AddVehicleShopToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"), reader.GetFloat("outX"), reader.GetFloat("outY"), reader.GetFloat("outZ"), reader.GetFloat("outRotX"), reader.GetFloat("outRotY"), reader.GetFloat("outRotZ"));
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerVehicleShops.ServerVehicleShops_.Count} Server-Vehicle-Shops geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllVehicleShopItems()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicleshops_items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                            ServerVehicleShops.AddVehicleShopItemToList(reader.GetInt32("id"), reader.GetInt32("shopId"), reader.GetUInt32("hash"), reader.GetInt32("price"), reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"), reader.GetFloat("rotX"), reader.GetFloat("rotY"), reader.GetFloat("rotZ"), reader.GetBoolean("previewSpawn"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerVehicleShops.ServerVehicleShopItems_.Count} Server-Vehicle-Shop-Items geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllGarages()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_garages";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader.GetInt32("faction") <= 0) ServerBlips.AddBlipToList($"Garage: {reader.GetString("name")}", 29, 0.7f, true, 50, reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"));
                            
                            ServerGarages.AddGarageToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("faction"), reader.GetInt32("vehClass"), reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"), reader.GetFloat("outX"), reader.GetFloat("outY"), reader.GetFloat("outZ"), reader.GetFloat("outRotZ"));
                            ServerMarker.AddMarkerToList(36, reader.GetFloat("outX"), reader.GetFloat("outY"), reader.GetFloat("outZ"), 1f, 255, 80, 80, 150, true);
                            ServerPeds.AddPedToList("s_m_y_airworker", reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"));
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerGarages.ServerGarages_.Count} Server-Garages geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllVehicles()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicles";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerVehicles.AddVehicleToList(reader.GetInt32("id"), reader.GetInt32("ownerId"), reader.GetInt32("secondOwnerId"), reader.GetUInt32("hash"), reader.GetInt32("faction"), reader.GetFloat("fuel"), reader.GetFloat("km"), reader.GetBoolean("isInGarage"), reader.GetInt32("garageId"), new AltV.Net.Data.Position(reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ")), new AltV.Net.Data.Rotation(reader.GetFloat("rotX"), reader.GetFloat("rotY"), reader.GetFloat("rotZ")), reader.GetString("plate"), reader.GetDateTime("lastUsage"));
                    }
                    connection.Close();
                }


                Console.WriteLine($"[SERVER] {ServerVehicles.ServerVehicles_.Count} Server-Vehicles geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllVehicleModifications()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicles_modifications";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            ServerVehiclesMod.AddVehicleModToL(reader.GetInt32("id"), reader.GetInt32("vehId"), reader.GetInt32("colorPrimary"), reader.GetInt32("colorSecondary"), reader.GetInt32("spoiler"), reader.GetInt32("front_bumper"), reader.GetInt32("rear_bumper"), reader.GetInt32("side_skirt"), reader.GetInt32("exhaust"), reader.GetInt32("frame"), reader.GetInt32("grille"), reader.GetInt32("hood"), reader.GetInt32("fender"), reader.GetInt32("right_fender"), reader.GetInt32("roof"), reader.GetInt32("engine"), reader.GetInt32("brakes"), reader.GetInt32("transmission"), reader.GetInt32("horns"), reader.GetInt32("suspension"), reader.GetInt32("armor"), reader.GetInt32("turbo"), reader.GetInt32("xenon"), reader.GetInt32("wheel_type"), reader.GetInt32("wheels"), reader.GetInt32("wheelcolor"), reader.GetInt32("plate_holder"), reader.GetInt32("trim_design"), reader.GetInt32("ornaments"), reader.GetInt32("dial_design"), reader.GetInt32("steering_wheel"), reader.GetInt32("shift_lever"), reader.GetInt32("plaques"), reader.GetInt32("hydraulics"), reader.GetInt32("airfilter"), reader.GetInt32("window_tint"), reader.GetInt32("livery"), reader.GetInt32("plate"), reader.GetInt32("neon"), reader.GetInt32("neon_r"), reader.GetInt32("neon_g"), reader.GetInt32("neon_b"), reader.GetInt32("smoke_r"), reader.GetInt32("smoke_g"), reader.GetInt32("smoke_b"), reader.GetInt32("colorPearl"), reader.GetInt32("headlightColor"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerVehiclesMod.ServerVehiclesMod_.Count} Server-Vehicle-Modifications geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllVehicleInfos()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_vehicle_infos";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                            ServerVehicleInfo.AddVehicleInfoToList(reader.GetUInt32("hash"), reader.GetString("displayName"), reader.GetFloat("trunk"), reader.GetFloat("maxfuel"), reader.GetInt32("tax"), reader.GetInt32("vehClass"));
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerVehicleInfo.ServerVehicleInfo_.Count} Server-Vehicle-Infos geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllATMs()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_atm";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServerATM.ServerATM_.Add(new ServerATM.Server_ATM
                            {
                                id = reader.GetInt32("id"),
                                posX = reader.GetFloat("posX"),
                                posY = reader.GetFloat("posY"),
                                posZ = reader.GetFloat("posZ")
                            });

                            ServerBlips.AddBlipToList("Bankautomat", 2, 0.7f, true, 277, reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"));
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerATM.ServerATM_.Count} Server-ATMs geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllShops()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_shops";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ServerShops.Server_Shops
                            {
                                id = reader.GetInt32("id"),
                                name = reader.GetString("name"),
                                owner = reader.GetInt32("owner"),
                                bank = reader.GetInt32("bank"),
                                price = reader.GetInt32("price"),
                                pedX = reader.GetFloat("pedX"),
                                pedY = reader.GetFloat("pedY"),
                                pedZ = reader.GetFloat("pedZ"),
                                posX = reader.GetFloat("posX"),
                                posY = reader.GetFloat("posY"),
                                posZ = reader.GetFloat("posZ"),
                                manageX = reader.GetFloat("manageX"),
                                manageY = reader.GetFloat("manageY"),
                                manageZ = reader.GetFloat("manageZ"),
                                pedModel = reader.GetString("pedModel"),
                                blipColor = reader.GetInt32("blipColor"),
                                blipSprite = reader.GetInt32("blipSprite"),
                                pedRot = reader.GetFloat("pedRot"),
                                type = reader.GetInt32("type"),
                                faction = reader.GetInt32("faction")
                            };
                            ServerShops.ServerShops_.Add(item);

                            if (item.type == 0 && item.faction <= 0)
                            {
                                ServerMarker.AddMarkerToList(1, reader.GetFloat("manageX"), reader.GetFloat("manageY"), reader.GetFloat("manageZ"), 1f, 255, 80, 80, 150, false);
                                ServerBlips.AddBlipToList(reader.GetString("name"), reader.GetInt32("blipColor"), 0.8f, true, reader.GetInt32("blipSprite"), reader.GetFloat("posX"), reader.GetFloat("posY"), reader.GetFloat("posZ"));
                            }                            
                            ServerPeds.AddPedToList(reader.GetString("pedModel"), reader.GetFloat("pedX"), reader.GetFloat("pedY"), reader.GetFloat("pedZ"), reader.GetFloat("pedRot"));
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerShops.ServerShops_.Count} Server-Shops geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAllShopItems()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_shops_items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServerShops.ServerShopsItems_.Add(new ServerShops.Server_Shops_Items
                            {
                               id = reader.GetInt32("id"),
                               shopId = reader.GetInt32("shopId"),
                               itemName = reader.GetString("itemName"),
                               itemPrice = reader.GetInt32("itemPrice"),
                               itemAmount = reader.GetInt32("itemAmount")
                            });
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerShops.ServerShopsItems_.Count} Server-Shop-Items geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddClothes(string type, int gender, string clothesName, int draw, int texture, int price)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO server_clothes (clothesName, type, draw, texture, gender, faction) VALUES (@clothesName, @type, @draw, @texture, @gender, @faction)";
                    command.Parameters.AddWithValue("@clothesName", clothesName);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@draw", draw);
                    command.Parameters.AddWithValue("@texture", texture);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@faction", 0);
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO server_clothesshops_items (shopId, clothesName, price) VALUES (@shopId, @clothesNamee, @price)";
                    command.Parameters.AddWithValue("@shopId", 1);
                    command.Parameters.AddWithValue("@clothesNamee", clothesName);
                    command.Parameters.AddWithValue("@price", price);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddLaborItem(int factionId, int accountId, string itemName, int itemAmount)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO server_faction_labor_items (factionId, accountId, itemName, itemAmount) VALUES (@factionId, @accountId, @itemName, @itemAmount)";
                    command.Parameters.AddWithValue("@factionId", factionId);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@itemAmount", itemAmount);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void UpdateLaborItemAmount(int accountId, string itemName, int itemAmount)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_faction_labor_items SET itemAmount=@itemAmount WHERE accountId=@accountId AND itemName=@itemName";
                    command.Parameters.AddWithValue("@itemAmount", itemAmount);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void DeleteLaborItem(int accountId, string itemName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM server_faction_labor_items WHERE accountId=@accountId AND itemName=@itemName";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void LoadAllServerItems()
        {
            try
            {
                using (var connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM server_items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServerItems.ServerItems_.Add(new ServerItems.Server_Items
                            {
                                id = reader.GetInt32("id"),
                                itemName = reader.GetString("itemName"),
                                itemWeight = reader.GetFloat("itemWeight"),
                                hasItemAnimation = reader.GetBoolean("hasItemAnimation"),
                                itemAnimationName = reader.GetString("itemAnimationName"),
                                isItemDroppable = reader.GetBoolean("isItemDroppable"),
                                isItemUseable = reader.GetBoolean("isItemUseable"),
                                itemPic = reader.GetString("itemPic")
                            });
                        }
                    }
                    connection.Close();
                }
                Console.WriteLine($"[SERVER] {ServerItems.ServerItems_.Count} Server-Items geladen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
