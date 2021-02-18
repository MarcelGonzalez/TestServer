using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class AccountsClothes
    {
        public partial class Accounts_Clothes
        {
            public int id { get; set; }
            public int accId { get; set; }
            public string clothesName { get; set; }
        }

        public static List<Accounts_Clothes> AccountsClothes_ = new List<Accounts_Clothes>();

        public static void AddCharacterClothes(int accId, string clothesName)
        {
            try
            {
                if (ExistCharacterClothes(accId, clothesName)) return;
                int id = 0;
                if (AccountsClothes_.Any())
                    id = AccountsClothes_.Last().id + 1;

               
                AddEntryToList(id, accId, clothesName);
                Database.PlayerDatabase.CreateAccountClothesEntry(accId, clothesName);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void AddEntryToList(int id, int accId, string clothesName)
        {
            try
            {
                AccountsClothes_.Add(new Accounts_Clothes
                {
                    id = id,
                    accId = accId,
                    clothesName = clothesName
                });
            }
            catch(Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static bool ExistCharacterClothes(int accId, string clothesName)
        {
            try
            {
                return AccountsClothes_.ToList().Exists(x => x.accId == accId && x.clothesName == clothesName);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }
    }
}
