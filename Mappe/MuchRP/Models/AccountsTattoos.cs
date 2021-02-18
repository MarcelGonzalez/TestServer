using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class AccountsTattoos
    {
        public partial class Accounts_Tattoos
        {
            public int id { get; set; }
            public int accId { get; set; }
            public int tattooId { get; set; }
        }

        public static List<Accounts_Tattoos> AccountsTattoos_ = new List<Accounts_Tattoos>();

        public static void RemoveAccountTattoo(int accId, int tatooId)
        {
            try
            {
                var entry = AccountsTattoos_.FirstOrDefault(x => x.accId == accId && x.tattooId == tatooId);
                if (entry != null) AccountsTattoos_.Remove(entry);

                Database.PlayerDatabase.DeleteAccountTattoo(accId, tatooId);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void CreateNewEntry(int accId, int tattooId)
        {
            try
            {
                int id = 0;
                if (AccountsTattoos_.Any())
                    id = AccountsTattoos_.Last().id + 1;

                AddEntryToList(id, accId, tattooId);
                Database.PlayerDatabase.CreateAccountTattooEntry(accId, tattooId);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void AddEntryToList(int id, int accId, int tattooId)
        {
            try
            {
                AccountsTattoos_.Add(new Accounts_Tattoos
                {
                    id = id,
                    accId = accId,
                    tattooId = tattooId
                });
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static bool ExistAccountTattoo(int accId, int id)
        {
            try
            {
                return AccountsTattoos_.ToList().Exists(x => x.accId == accId && x.tattooId == id);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static string GetAccountOwnTattoos(int accId)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(AccountsTattoos_.ToList().Where(x => x.accId == accId).Select(x => new
                {
                    x.tattooId,
                    name = ServerTattoos.GetTattooName(x.tattooId),
                    price = ServerTattoos.GetTattooPrice(x.tattooId),
                }).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }

        public static string GetAccountTattoos(int accId)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(AccountsTattoos_.ToList().Where(x => x.accId == accId).Select(x => new
                {
                    collection = ServerTattoos.GetTattooCollection(x.tattooId),
                    hash = ServerTattoos.GetTattooNameHash(x.tattooId),
                }).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }
    }
}
