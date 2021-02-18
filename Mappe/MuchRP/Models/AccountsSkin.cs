using AltV.Net;
using AltV.Net.Async;
using MuchRP.Factories;
using MuchRP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuchRP.Models
{
    class AccountsSkin : IScript
    {
        public partial class Accounts_Skin
        {
            public int id { get; set; }
            public int accId { get; set; }
            public string facefeatures { get; set; }
            public string headblendsdata { get; set; }
            public string headoverlays { get; set; }
            public string clothesTop { get; set; }
            public string clothesTorso { get; set; }
            public string clothesLeg { get; set; }
            public string clothesFeet { get; set; }
            public string clothesHat { get; set; }
            public string clothesGlass { get; set; }
            public string clothesEarring { get; set; }
            public string clothesNecklace { get; set; }
            public string clothesMask { get; set; }
            public string clothesArmor { get; set; }
            public string clothesUndershirt { get; set; }
            public string clothesBracelet { get; set; }
            public string clothesWatch { get; set; }
            public string clothesBag { get; set; }
            public string clothesDecal { get; set; }
        }

        public static List<Accounts_Skin> AccountsSkin_ = new List<Accounts_Skin>();

        public static void AddEntryToList(Accounts_Skin skin)
        {
            try
            {
                AccountsSkin_.Add(skin);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void CreateNewEntry(Accounts_Skin skin)
        {
            try
            {
                int id = 0;
                if (AccountsSkin_.Any())
                    id = AccountsSkin_.Last().id + 1;

                AddEntryToList(skin);
                Database.PlayerDatabase.CreateAccountSkin(skin);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static string GetClothes(int accId, string type)
        {
            try
            {
                var chars = AccountsSkin_.ToList().FirstOrDefault(x => x.accId == accId);
                if (chars == null) return "None";
                switch(type)
                {
                    case "Top":
                        return chars.clothesTop;
                    case "Torso":
                        return chars.clothesTorso;
                    case "Leg":
                        return chars.clothesLeg;
                    case "Feet":
                        return chars.clothesFeet;
                    case "Hat":
                        return chars.clothesHat;
                    case "Glass":
                        return chars.clothesGlass;
                    case "Necklace":
                        return chars.clothesNecklace;
                    case "Mask":
                        return chars.clothesMask;
                    case "Armor":
                        return chars.clothesArmor;
                    case "Undershirt":
                        return chars.clothesUndershirt;
                    case "Decal":
                        return chars.clothesDecal;
                    case "Bracelet":
                        return chars.clothesBracelet;
                    case "Watch":
                        return chars.clothesWatch;
                    case "Earring":
                        return chars.clothesEarring;
                    case "Bag":
                        return chars.clothesBag;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "None";
        }

        public static string GetFacefeatures(int accId)
        {
            try
            {
                var i = AccountsSkin_.ToList().FirstOrDefault(x => x.accId == accId);
                if (i != null) return i.facefeatures;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }

        public static string GetHeadblend(int accId)
        {
            try
            {
                var i = AccountsSkin_.ToList().FirstOrDefault(x => x.accId == accId);
                if (i != null) return i.headblendsdata;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }

        public static string GetHeadoverlay(int accId)
        {
            try
            {
                var i = AccountsSkin_.ToList().FirstOrDefault(x => x.accId == accId);
                if (i != null) return i.headoverlays;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "[]";
        }

        public static void SetCharacterClothes(int accId, string type, string clothesName)
        {
            try
            {
                if (accId <= 0) return;
                var chars = AccountsSkin_.FirstOrDefault(p => p.accId == accId);
                if (chars != null)
                {
                    switch (type)
                    {
                        case "Top":
                            chars.clothesTop = clothesName;
                            break;
                        case "Torso":
                            chars.clothesTorso = clothesName;
                            break;
                        case "Leg":
                            chars.clothesLeg = clothesName;
                            break;
                        case "Feet":
                            chars.clothesFeet = clothesName;
                            break;
                        case "Bag":
                            chars.clothesBag = clothesName;
                            break;
                        case "Hat":
                            chars.clothesHat = clothesName;
                            break;
                        case "Glass":
                            chars.clothesGlass = clothesName;
                            break;
                        case "Necklace":
                            chars.clothesNecklace = clothesName;
                            break;
                        case "Mask":
                            chars.clothesMask = clothesName;
                            break;
                        case "Armor":
                            chars.clothesArmor = clothesName;
                            break;
                        case "Undershirt":
                            chars.clothesUndershirt = clothesName;
                            break;
                        case "Decal":
                            chars.clothesDecal = clothesName;
                            break;
                        case "Bracelet":
                            chars.clothesBracelet = clothesName;
                            break;
                        case "Watch":
                            chars.clothesWatch = clothesName;
                            break;
                        case "Earring":
                            chars.clothesEarring = clothesName;
                            break;
                    }
                    Database.PlayerDatabase.SetAccountClothes(accId, $"clothes{type}", clothesName);
                }
            }
            catch(Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static async Task SwitchCharacterClothes(MuchPlayer player, string Type, string clothesName)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(Type) || string.IsNullOrWhiteSpace(clothesName)) return;
                if (clothesName != "None")
                {
                    SetCharacterClothes(player.accountId, Type, clothesName);
                    switch (Type)
                    {
                        case "Hat":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 0, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Glass":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 1, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Earring":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 2, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Watch":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 6, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Bracelet":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 7, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Mask":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 1, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Necklace":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 7, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Undershirt":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Decal":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 10, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Armor":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 9, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Feet":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 6, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Top":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 11, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Torso":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 3, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                        case "Leg":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, ServerClothes.GetClothesDraw(clothesName), ServerClothes.GetClothesTexture(clothesName));
                            break;
                    }
                }
                else
                {
                    SetCharacterClothes(player.accountId, Type, "None");
                    switch (Type)
                    {
                        case "Hat":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 0);
                            break;
                        case "Glass":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 1);
                            break;
                        case "Earring":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 2);
                            break;
                        case "Watch":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 6);
                            break;
                        case "Bracelet":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 7);
                            break;
                        case "Mask":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 1, 0, 0);
                            break;
                        case "Necklace":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 7, 0, 0);
                            break;
                        case "Undershirt":
                            if (player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, 57, 0);
                            else if (player.gender == 1) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, 34, 0);
                            break;
                        case "Decal":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 10, 0, 0);
                            break;
                        case "Armor":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 9, 0, 0);
                            break;
                        case "Feet":
                            if (player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 34, 0);
                            else if (player.gender == 1) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 35, 0);
                            break;
                        case "Leg":
                            if (player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, 21, 0);
                            else if (player.gender == 1) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, 15, 0);
                            break;
                        case "Top":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 11, 15, 0);
                            break;
                        case "Torso":
                            HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 3, 15, 0);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Utilities:RequestCurrentSkin")]
        public static async Task requestCurrentSkin(MuchPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0) return;
                if (GetClothes(player.accountId, "Top") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 11, 15, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 11, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Top")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Top")));

                if (GetClothes(player.accountId, "Torso") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 3, 15, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 3, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Torso")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Torso")));

                if (GetClothes(player.accountId, "Leg") == "None")
                {
                    if (player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, 21, 0);
                    else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, 15, 0);
                }
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 4, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Leg")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Leg")));

                if(GetClothes(player.accountId, "Feet") == "None")
                {
                    if(player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 6, 34, 0);
                    else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 6, 35, 0);
                }
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 6, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Feet")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Feet")));

                if (GetClothes(player.accountId, "Mask") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 1, 0, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 1, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Mask")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Mask")));
                
                if (GetClothes(player.accountId, "Necklace") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 7, 0, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 7, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Necklace")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Necklace")));

                if (GetClothes(player.accountId, "Armor") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 9, 0, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 9, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Armor")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Armor")));

                if (GetClothes(player.accountId, "Hat") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 0, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Hat")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Hat")));

                if (GetClothes(player.accountId, "Glass") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 1);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 1, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Glass")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Glass")));

                if (GetClothes(player.accountId, "Earring") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 2);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 2, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Earring")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Earring")));

                if (GetClothes(player.accountId, "Watch") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 6);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 6, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Watch")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Watch")));

                if (GetClothes(player.accountId, "Bracelet") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:clearAccessory", 7);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setAccessory", 7, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Bracelet")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Bracelet")));

                if (GetClothes(player.accountId, "Decal") == "None") HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 10, 0, 0);
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 10, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Decal")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Decal")));

                if(GetClothes(player.accountId, "Undershirt") == "None")
                {
                    if(player.gender == 0) HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, 57, 0);
                    else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, 34, 0);
                }
                else HelperMethods.TriggerClientEvent(player, "Client:Utilities:setClothes", 8, ServerClothes.GetClothesDraw(GetClothes(player.accountId, "Undershirt")), ServerClothes.GetClothesTexture(GetClothes(player.accountId, "Undershirt")));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
