using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using MuchRP.Factories;
using MuchRP.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace MuchRP.Database
{
    class PlayerDatabase : IScript
    {
        public static void RegisterAccount(string name, string password, ulong socialId)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
            {
                try {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO accounts (username, password, socialId, hasWhitelist, adminlevel, gender, isFirstLogin, isCrimeFlagged, cash, bank) VALUES (@username, @password, @socialId, @hasWhitelist, @adminlevel, @gender, @isFirstLogin, @isCrimeFlagged, @cash, @bank)";

                    command.Parameters.AddWithValue("@username", name);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@socialId", socialId);
                    command.Parameters.AddWithValue("@hasWhitelist", false);
                    command.Parameters.AddWithValue("@adminlevel", 0);
                    command.Parameters.AddWithValue("@gender", 0);
                    command.Parameters.AddWithValue("@isFirstLogin", true);
                    command.Parameters.AddWithValue("@isCrimeFlagged", false);
                    command.Parameters.AddWithValue("@cash", 5000);
                    command.Parameters.AddWithValue("@bank", 10000);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"[SERVER] Neuer Account angelegt: {name}.");
                }
                catch(Exception e) {
                    Console.WriteLine($"[EXCEPTION] RegisterPlayer: {e.Message}");
                    Console.WriteLine($"[EXCEPTION] RegisterPlayer: {e.StackTrace}");
                }
            }
        }

        public static void DeleteAccountTattoo(int accId, int tattooId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM accounts_tattoos WHERE accId=@accId AND tattooId=@tattooId";
                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@tattooId", tattooId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void CreateAccountClothesEntry(int accId, string clothesName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO accounts_clothes (accountId, clothesName) VALUES (@accountId, @clothesName)";
                    command.Parameters.AddWithValue("@accountId", accId);
                    command.Parameters.AddWithValue("@clothesName", clothesName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void CreateAccountTattooEntry(int accId, int tattooId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO accounts_tattoos (accId, tattooId) VALUES (@accId, @tattooId)";

                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@tattooId", tattooId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void CreateAccountSkin(Models.AccountsSkin.Accounts_Skin skin)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO accounts_skin (accId, facefeatures, headblendsdata, headoverlays, clothesTop, clothesTorso, clothesLeg, clothesFeet, clothesHat, clothesGlass, clothesEarring, clothesNecklace, clothesMask, clothesArmor, clothesUndershirt, clothesBracelet, clothesWatch, clothesBag, clothesDecal) VALUES (@accId, @facefeatures, @headblendsdata, @headoverlays, @clothesTop, @clothesTorso, @clothesLeg, @clothesFeet, @clothesHat, @clothesGlass, @clothesEarring, @clothesNecklace, @clothesMask, @clothesArmor, @clothesUndershirt, @clothesBracelet, @clothesWatch, @clothesBag, @clothesDecal)";

                    command.Parameters.AddWithValue("@accId", skin.accId);
                    command.Parameters.AddWithValue("@facefeatures", skin.facefeatures);
                    command.Parameters.AddWithValue("@headblendsdata", skin.headblendsdata);
                    command.Parameters.AddWithValue("@headoverlays", skin.headoverlays);
                    command.Parameters.AddWithValue("@clothesTop", skin.clothesTop);
                    command.Parameters.AddWithValue("@clothesTorso", skin.clothesTorso);
                    command.Parameters.AddWithValue("@clothesLeg", skin.clothesLeg);
                    command.Parameters.AddWithValue("@clothesFeet", skin.clothesFeet);
                    command.Parameters.AddWithValue("@clothesHat", skin.clothesHat);
                    command.Parameters.AddWithValue("@clothesGlass", skin.clothesGlass);
                    command.Parameters.AddWithValue("@clothesEarring", skin.clothesEarring);
                    command.Parameters.AddWithValue("@clothesNecklace", skin.clothesNecklace);
                    command.Parameters.AddWithValue("@clothesMask", skin.clothesMask);
                    command.Parameters.AddWithValue("@clothesArmor", skin.clothesArmor);
                    command.Parameters.AddWithValue("@clothesUndershirt", skin.clothesUndershirt);
                    command.Parameters.AddWithValue("@clothesBracelet", skin.clothesBracelet);
                    command.Parameters.AddWithValue("@clothesWatch", skin.clothesWatch);
                    command.Parameters.AddWithValue("@clothesBag", skin.clothesBag);
                    command.Parameters.AddWithValue("@clothesDecal", skin.clothesDecal);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"[SERVER] Neuer Account-Skin angelegt: {skin.accId}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void SetAccountClothes(int accId, string dbField, string clothesName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = $"UPDATE accounts_skin SET {dbField}=@dbField WHERE accId=@accId";

                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@dbField", clothesName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void CreateAccountLastPosition(int accountId, Position pos, int dimension)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO accounts_position (accId, posX, posY, posZ, dimension) VALUES (@accId, @posX, @posY, @posZ, @dimension)";

                    command.Parameters.AddWithValue("@accId", accountId);
                    command.Parameters.AddWithValue("@posX", pos.X);
                    command.Parameters.AddWithValue("@posY", pos.Y);
                    command.Parameters.AddWithValue("@posZ", pos.Z);
                    command.Parameters.AddWithValue("@dimension", dimension);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"[SERVER] Neue Account-Position angelegt: {accountId}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAccountTattoos()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_tattoos";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) Models.AccountsTattoos.AddEntryToList(reader.GetInt32("id"), reader.GetInt32("accId"), reader.GetInt32("tattooId"));
                    }
                    connection.Close();
                    Console.WriteLine($"[SERVER] {Models.AccountsTattoos.AccountsTattoos_.Count} Account-Tattoos geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAccountSkins()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_skin";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) Models.AccountsSkin.AddEntryToList(new Models.AccountsSkin.Accounts_Skin
                        {
                            id = reader.GetInt32("id"),
                            accId = reader.GetInt32("accId"),
                            facefeatures = reader.GetString("facefeatures"),
                            headblendsdata = reader.GetString("headblendsdata"),
                            headoverlays = reader.GetString("headoverlays"),
                            clothesTop = reader.GetString("clothesTop"),
                            clothesTorso = reader.GetString("clothesTorso"),
                            clothesLeg = reader.GetString("clothesLeg"),
                            clothesFeet = reader.GetString("clothesFeet"),
                            clothesHat = reader.GetString("clothesHat"),
                            clothesGlass = reader.GetString("clothesGlass"),
                            clothesEarring = reader.GetString("clothesEarring"),
                            clothesNecklace = reader.GetString("clothesNecklace"),
                            clothesMask = reader.GetString("clothesMask"),
                            clothesArmor = reader.GetString("clothesArmor"),
                            clothesUndershirt = reader.GetString("clothesUndershirt"),
                            clothesBracelet = reader.GetString("clothesBracelet"),
                            clothesWatch = reader.GetString("clothesWatch"),
                            clothesBag = reader.GetString("clothesBag"),
                            clothesDecal = reader.GetString("clothesDecal")
                        });
                    }
                    connection.Close();
                    Console.WriteLine($"[SERVER] {Models.AccountsSkin.AccountsSkin_.Count} Account-Skins geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAccountClothes()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_clothes";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) Models.AccountsClothes.AddEntryToList(reader.GetInt32("id"), reader.GetInt32("accountId"), reader.GetString("clothesName"));
                    }
                    connection.Close();
                    Console.WriteLine($"[SERVER] {Models.AccountsClothes.AccountsClothes_.Count} Accounts-Clothes geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void LoadAccount(MuchPlayer player, string username)
        {
            try 
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return;
                        reader.Read();
                        player.accountName = username;
                        player.accountId = reader.GetInt32("id");
                        player.adminlevel = reader.GetInt32("adminlevel");
                        player.gender = reader.GetInt32("gender");
                        player.isCrimeFlagged = reader.GetBoolean("isCrimeFlagged");
                        player.cash = reader.GetInt32("cash");
                        player.bank = reader.GetInt32("bank");
                    }
                    connection.Close();
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_inventory WHERE accId=@accId";
                    command.Parameters.AddWithValue("@accId", player.accountId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return;
                        while (reader.Read())
                        {
                            player.AccountInventory_.Add(new Models.AccountsInventory.Accounts_Inventory
                            {
                                id = reader.GetInt32("id"),
                                accId = reader.GetInt32("accId"),
                                itemName = reader.GetString("itemName"),
                                itemAmount = reader.GetInt32("itemAmount")
                            });
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                Alt.Log($"{e}");
            }
        }


        public static string GetAccountPassword(string username)
        {
            string password = "DSAOKJDWIAUHWDZDUWABDUWAO";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT password FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return password;
                        reader.Read();
                        password = reader.GetString("password");
                    }
                    connection.Close();
                    return password;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return password;
        }

        public static int GetAccountLastDimension(int accID)
        {
            int dim = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT dimension FROM accounts_position WHERE accId=@accId LIMIT 1";
                    command.Parameters.AddWithValue("@accId", accID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return dim;
                        reader.Read();
                        dim = reader.GetInt32("dimension");
                    }
                    connection.Close();
                    return dim;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return dim;
        }

        public static Position GetAccountLastPosition(int accID)
        {
            Position pos = new Position(0f,0f,0f);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_position WHERE accId=@accId LIMIT 1";
                    command.Parameters.AddWithValue("@accId", accID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return pos;
                        reader.Read();
                        pos.X = reader.GetFloat("posX");
                        pos.Y = reader.GetFloat("posY");
                        pos.Z = reader.GetFloat("posZ");
                    }
                    connection.Close();
                    return pos;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return pos;
        }

        public static string GetAccountName(int accountId)
        {
            string name = "";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE id=@id LIMIT 1";
                    command.Parameters.AddWithValue("@id", accountId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return name;
                        reader.Read();
                        name = reader.GetString("username");
                    }
                    connection.Close();
                }
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return name;
        }

        public static ulong GetAccountSocialClub(string username)
        {
            ulong sc = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT socialId FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return sc;
                        reader.Read();
                        sc = reader.GetUInt64("socialId");
                    }
                    connection.Close();
                    return sc;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return sc;
        }

        public static void UpdatePlayerCash(int accId, int newCash)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts SET cash=@cash WHERE id=@id";
                    command.Parameters.AddWithValue("@id", accId);
                    command.Parameters.AddWithValue("@cash", newCash);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdatePlayerBankCash(int accId, int newCash)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts SET bank=@bank WHERE id=@id";
                    command.Parameters.AddWithValue("@id", accId);
                    command.Parameters.AddWithValue("@bank", newCash);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdateAccountCrimeFlag(string username, bool isCrimeFlagged)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts SET isCrimeFlagged=@isCrimeFlagged WHERE username=@username";
                    command.Parameters.AddWithValue("@isCrimeFlagged", isCrimeFlagged);
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdateAccountGender(string username, int gender)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts SET gender=@gender WHERE username=@username";
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdateAccountLastPosition(int accId, Position pos, int dimension)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts_position SET posX=@posX, posY=@posY, posZ=@posZ, dimension=@dimension WHERE accId=@accId";
                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@posX", pos.X);
                    command.Parameters.AddWithValue("@posY", pos.Y);
                    command.Parameters.AddWithValue("@posZ", pos.Z);
                    command.Parameters.AddWithValue("@dimension", dimension);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdateInventoryItemAmount(int accId, string itemName, int newItemAmount)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts_inventory SET itemAmount=@itemAmount WHERE accId=@accId AND itemName=@itemName LIMIT 1";
                    command.Parameters.AddWithValue("@itemAmount", newItemAmount);
                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddCharacterItem(int accId, string itemName, int itemAmount)
        {
            try
            {
                if (accId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO accounts_inventory (accId, itemName, itemAmount) VALUES (@accId, @itemName, @itemAmount)";
                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@itemAmount", itemAmount);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"[SERVER] Neues Item ({itemName} ({itemAmount}x)) hinzugefügt: {accId}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void DeleteInventoryItem(int accId, string itemName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM accounts_inventory WHERE accId=@accId AND itemName=@itemName LIMIT 1";
                    command.Parameters.AddWithValue("@accId", accId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void UpdateAccountFirstLogin(string username, bool firstLogin)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE accounts SET isFirstLogin=@isFirstLogin WHERE username=@username";
                    command.Parameters.AddWithValue("@isFirstLogin", firstLogin);
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetAccountId(string username)
        {
            int id = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT id FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return id;
                        reader.Read();
                        id = reader.GetInt32("id");
                    }
                    connection.Close();
                    return id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return id;
        }

        public static bool ExistAccountSkin(int accId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts_skin WHERE accId=@accId LIMIT 1";
                    command.Parameters.AddWithValue("@accId", accId);

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

        public static bool ExistAccountName(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

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

        public static bool ExistSocialIdInDB(ulong socialId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE socialId=@socialId LIMIT 1";
                    command.Parameters.AddWithValue("@socialId", socialId);

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

        public static bool HasAccountWhitelist(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) { connection.Close(); return false; }
                        reader.Read();
                        bool hasWhitelist = reader.GetBoolean("hasWhitelist");
                        connection.Close();
                        return hasWhitelist;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool IsAccountFirstLogin(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM accounts WHERE username=@username LIMIT 1";
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) { connection.Close(); return false; }
                        reader.Read();
                        bool isFirstLogin = reader.GetBoolean("isFirstLogin");
                        connection.Close();
                        return isFirstLogin;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }
    }
}
