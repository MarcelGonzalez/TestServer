using MuchRP.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Database
{
    class SystemDatabase
    {
        public static void UpdateTattooShopBank(int shopId, int newCash)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_tattooshops SET bank=@bank WHERE id=@id";
                    command.Parameters.AddWithValue("@id", shopId);
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

        public static void UpdateShopBank(int shopId, int newCash)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_shops SET bank=@bank WHERE id=@id";
                    command.Parameters.AddWithValue("@id", shopId);
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

        public static void UpdateShopItemPrice(int shopId, string itemName, int newPrice)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_shops_items SET itemPrice=@itemPrice WHERE shopId=@shopId AND itemName=@itemName LIMIT 1";
                    command.Parameters.AddWithValue("@itemPrice", newPrice);
                    command.Parameters.AddWithValue("@shopId", shopId);
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

        public static void UpdateShopItemAmount(int shopId, string itemName, int newAmount)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_shops_items SET itemAmount=@itemAmount WHERE shopId=@shopId AND itemName=@itemName LIMIT 1";
                    command.Parameters.AddWithValue("@itemAmount", newAmount);
                    command.Parameters.AddWithValue("@shopId", shopId);
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

        internal static void RemoveFactionMember(int factionId, int accountId)
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM server_faction_members WHERE factionId=@factionId AND accountId=@accountId";
                    command.Parameters.AddWithValue("@factionId", factionId);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static void AddFactionMember(int factionId, int accountId, int rankId)
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO server_faction_members (factionId, accountId, rankId) VALUES (@factionId, @accountId, @rankId)";
                    command.Parameters.AddWithValue("@factionId", factionId);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@rankId", rankId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static void UpdateFactionMemberRank(int factionId, int accountId, int newRank)
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE server_faction_members SET rankId=@rankId WHERE factionId=@factionId AND accountId=@accountId";
                    command.Parameters.AddWithValue("@rankId", newRank);
                    command.Parameters.AddWithValue("@factionId", factionId);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AddShopItem(int shopId, string itemName, int amount, int price)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO server_shops_items (shopId, itemName, itemPrice, itemAmount) VALUES (@shopId, @itemName, @itemPrice, @itemAmount)";
                    command.Parameters.AddWithValue("@shopId", shopId);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@itemAmount", amount);
                    command.Parameters.AddWithValue("@itemPrice", price);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void DeleteShopItem(int shopId, string itemName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Constants.DatabaseConfig.connectionString))
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM server_shops_items WHERE shopId=@shopId AND itemName=@itemName LIMIT 1";
                    command.Parameters.AddWithValue("@shopId", shopId);
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
    }
}
