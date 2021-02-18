using AltV.Net;
using AltV.Net.Async;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerTattoos : IScript
    {
        public partial class Server_Tattoos
        {
            public int id { get; set; }
            public string collection { get; set; }
            public string nameHash { get; set; }
            public string name { get; set; }
            public string part { get; set; }
            public int price { get; set; }
            public int gender { get; set; } //0 male, 1 female
        }

        public static List<Server_Tattoos> ServerTattoos_ = new List<Server_Tattoos>();

        public static async void GetAllTattoos(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                var tattooItems = ServerTattoos_.ToList().Where(x => x.gender == player.gender).Select(x => new
                {
                    x.id,
                    x.name,
                    x.nameHash,
                    x.part,
                    x.price,
                    x.collection,
                }).OrderBy(x => x.name).ToList();

                var itemCount = (int)tattooItems.Count;
                var iterations = Math.Floor((decimal)(itemCount / 30));
                var rest = itemCount % 30;
                for(var i = 0; i < iterations; i++)
                {
                    var skip = i * 30;
                    HelperMethods.TriggerClientEvent(player, "Client:TattooShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(tattooItems.Skip(skip).Take(30).ToList()));
                }
                if (rest != 0) HelperMethods.TriggerClientEvent(player, "Client:TattooShop:sendItemsToClient", System.Text.Json.JsonSerializer.Serialize(tattooItems.Skip((int)iterations * 30).ToList()));
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool ExistTattoo(int tattooId)
        {
            try
            {
                return ServerTattoos_.ToList().Exists(x => x.id == tattooId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static string GetTattooCollection(int tattooId)
        {
            try
            {
                var tattoo = ServerTattoos_.ToList().FirstOrDefault(x => x.id == tattooId);
                if (tattoo != null) return tattoo.collection;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetTattooNameHash(int tattooId)
        {
            try
            {
                var tattoo = ServerTattoos_.ToList().FirstOrDefault(x => x.id == tattooId);
                if (tattoo != null) return tattoo.nameHash;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetTattooName(int tattooId)
        {
            try
            {
                var tattoo = ServerTattoos_.ToList().FirstOrDefault(x => x.id == tattooId);
                if (tattoo != null) return tattoo.name;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static int GetTattooPrice(int tattooId)
        {
            try
            {
                var tattoo = ServerTattoos_.ToList().FirstOrDefault(x => x.id == tattooId);
                if (tattoo != null) return tattoo.price;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void AddTattooToList(int id, string collection, string nameHash, string name, string part, int price, int gender)
        {
            try
            {
                ServerTattoos_.Add(new Server_Tattoos
                {
                    id = id,
                    collection = collection,
                    name = name,
                    nameHash = nameHash,
                    part = part,
                    price = price,
                    gender = gender
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
