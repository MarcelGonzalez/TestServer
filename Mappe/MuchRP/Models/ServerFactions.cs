using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerFactions
    {
        //ToDo: Notiz
		/*
		 * 1 LSPD
		 * 2 FIB
         * 3 Army
		 * 4 LSMC
		 * 5 GOV
		 * 6 DPOS
		 * 7 Fahrschule
		 * 
		 * 
		 * 10 Grove
		 * 11 Ballas
		 * 12 Yakuza
		 * 13 LCN
		 * 14 Triaden
		 * 15 
		 * 25 Lost MC
		 * */
         

        #region Partial Classes
        public partial class Server_Factions
        {
            public int id { get; set; }
            public string name { get; set; }
            public string shortcut { get; set; }
            public int money { get; set; }
            public int phoneNumber { get; set; } //Service Number, default 0 = not available later in phone.
            public int type { get; set; } // 1 = Staatlich, 2 = Gang, 3 = Mafia
            public Position managePos { get; set; }
            public Position laborPos { get; set; } // Labor-System only for Factions with Type 2 or 3.

            [NotMapped]
            public bool isLaborLocked { get; set; } = true;
        }

        public partial class Server_Faction_Labor_Items
        {
            public int id { get; set; }
            public int factionId { get; set; }
            public int accountId { get; set; }
            public string itemName { get; set; } 
            public int itemAmount { get; set; }
        }
        
        public partial class Server_Faction_Ranks
        {
            public int id { get; set; }
            public int factionId { get; set; }
            public int rankId { get; set; }
            public string name { get; set; }
            public int payday { get; set; }
        }

        public partial class Server_Faction_Members
        {
            public int id { get; set; }
            public int factionId { get; set; }
            public int accountId { get; set; }
            public int rankId { get; set; }
            public bool isDuty { get; set; } = false; //only for factions with type 1.
        }
        #endregion

        public static List<Server_Factions> ServerFactions_ = new List<Server_Factions>();
        public static List<Server_Faction_Ranks> ServerFactionRanks_ = new List<Server_Faction_Ranks>();
        public static List<Server_Faction_Members> ServerFactionMembers_ = new List<Server_Faction_Members>();
        public static List<Server_Faction_Labor_Items> ServerFactionLaborItems_ = new List<Server_Faction_Labor_Items>();

        #region Labor Functions
        public static bool IsLaborLocked(int factionId)
        {
            try
            {
                var x = ServerFactions_.ToList().FirstOrDefault(y => y.id == factionId);
                if (x != null) return x.isLaborLocked;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        public static int GetLaborItemAmount(int factionId, int accountId, string itemName)
        {
            try
            {
                var x = ServerFactionLaborItems_.ToList().FirstOrDefault(y => y.factionId == factionId && y.accountId == accountId && y.itemName == itemName);
                if (x != null) return x.itemAmount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static void AddLaborItem(int factionId, int accountId, string itemName, int itemAmount)
        {
            try
            {
                if (factionId <= 0 || accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var existItem = ServerFactionLaborItems_.FirstOrDefault(x => x.factionId == factionId && x.accountId == accountId && x.itemName == itemName);
                if(existItem != null)
                {
                    existItem.itemAmount += itemAmount;
                    Database.UtilsDatabase.UpdateLaborItemAmount(accountId, itemName, existItem.itemAmount);
                }
                else
                {
                    Database.UtilsDatabase.AddLaborItem(factionId, accountId, itemName, itemAmount);
                    ServerFactionLaborItems_.Add(new Server_Faction_Labor_Items
                    {
                        factionId = factionId,
                        accountId = accountId,
                        itemAmount = itemAmount,
                        itemName = itemName
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void RemoveLaborItemAmount(int factionId, int accountId, string itemName, int itemAmount)
        {
            try
            {
                if (factionId <= 0 || accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                var item = ServerFactionLaborItems_.FirstOrDefault(x => x.factionId == factionId && x.accountId == accountId && x.itemName == itemName);
                if (item == null) return;
                item.itemAmount -= itemAmount;
                if (item.itemAmount > 0) Database.UtilsDatabase.UpdateLaborItemAmount(accountId, itemName, item.itemAmount);
                else
                {
                    Database.UtilsDatabase.DeleteLaborItem(accountId, itemName);
                    ServerFactionLaborItems_.Remove(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static Position GetLaborExitPosition(int factionId)
        {
            try
            {
                var x = ServerFactions_.ToList().FirstOrDefault(y => y.id == factionId);
                if(x != null)
                {
                    switch(x.type)
                    {
                        case 2: return Utils.Constants.Positions.weedLabor_ExitPosition;
                        case 3: return Utils.Constants.Positions.methLabor_ExitPosition;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return new Position(0,0,0);
        }

        public static void SetLaborLocked(int factionId, bool state)
        {
            try
            {
                var x = ServerFactions_.FirstOrDefault(y => y.id == factionId);
                if (x == null) return;
                x.isLaborLocked = state;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        #region Add To List Functions
        public static void AddFactionLaborItemToList(int id, int factionId, int accountId, string itemName, int itemAmount)
        {
            try
            {
                ServerFactionLaborItems_.Add(new Server_Faction_Labor_Items
                {
                    id = id,
                    factionId = factionId,
                    accountId = accountId,
                    itemAmount = itemAmount,
                    itemName = itemName
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AddFactionEntryToList(int id, string name, string shortcut, int money, int phoneNumber, int type, Position managePos, Position laborPos)
        {
            try
            {
                ServerFactions_.Add(new Server_Factions
                {
                    id = id,
                    name = name,
                    shortcut = shortcut,
                    money = money,
                    phoneNumber = phoneNumber,
                    type = type,
                    managePos = managePos,
                    laborPos = laborPos
                });

                ServerMarker.AddMarkerToList(1, managePos.X, managePos.Y, managePos.Z - 1, 1f, 0, 204, 102, 150, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }        

        public static void AddFactionRankEntryToList(int id, int factionId, int rankId, string name, int payday)
        {
            try
            {
                ServerFactionRanks_.Add(new Server_Faction_Ranks
                {
                    id = id,
                    factionId = factionId,
                    rankId = rankId,
                    name = name,
                    payday = payday
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddFactionMemberEntryToList(int id, int factionId, int accountId, int rankId)
        {
            try
            {
                ServerFactionMembers_.Add(new Server_Faction_Members
                {
                    id = id,
                    factionId = factionId,
                    accountId = accountId,
                    rankId = rankId,
                    isDuty = false
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region Faction Functions
        public static string GetFactionName(int factionId)
        {

            try
            {
                var i = ServerFactions_.ToList().FirstOrDefault(x => x.id == factionId);
                if (i != null) return i.name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }

        public static string GetFactionShortcut(int factionId)
        {

            try
            {
                var i = ServerFactions_.ToList().FirstOrDefault(x => x.id == factionId);
                if (i != null) return i.shortcut;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }

        public static int GetFactionMoney(int factionId)
        {

            try
            {
                var i = ServerFactions_.ToList().FirstOrDefault(x => x.id == factionId);
                if (i != null) return i.money;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetFactionPhonenumber(int factionId)
        {

            try
            {
                var i = ServerFactions_.ToList().FirstOrDefault(x => x.id == factionId);
                if (i != null) return i.phoneNumber;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetFactionType(int factionId)
        {

            try
            {
                var i = ServerFactions_.ToList().FirstOrDefault(x => x.id == factionId);
                if (i != null) return i.type;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
        #endregion

        #region Rank Functions
        public static bool IsRankALeaderShipment(int factionId, int rankId)
        {

            try
            {
                int maxRanks = GetFactionRankCount(factionId);
                return rankId >= (maxRanks - 2);               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        public static int GetFactionRankCount(int factionId)
        {

            try
            {
                return ServerFactionRanks_.ToList().Where(x => x.factionId == factionId).Count();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetFactionRankPayday(int factionId, int rankId)
        {

            try
            {
                var i = ServerFactionRanks_.ToList().FirstOrDefault(x => x.factionId == factionId && x.rankId == rankId);
                if (i != null) return i.payday;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static string GetFactionRankName(int factionId, int rankId)
        {

            try
            {
                var i = ServerFactionRanks_.ToList().FirstOrDefault(x => x.factionId == factionId && x.rankId == rankId);
                if (i != null) return i.name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }
        #endregion

        #region Member Functions
        public static string GetFactionMemberJSON(int factionId)
        {

            try
            {
                return System.Text.Json.JsonSerializer.Serialize(ServerFactionMembers_.ToList().Where(x => x.factionId == factionId).Select(x => new
                {
                    x.accountId,
                    name = Database.PlayerDatabase.GetAccountName(x.accountId),
                    x.rankId,
                    rankName = GetFactionRankName(x.factionId, x.rankId),
                }).OrderByDescending(x => x.rankId).ThenBy(y => y.name).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "[]";
        }

        public static void SetAccountDuty(int accountId, bool isDuty)
        {

            try
            {
                var i = ServerFactionMembers_.FirstOrDefault(x => x.accountId == accountId);
                if (i == null) return;
                i.isDuty = isDuty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void RemoveFactionMember(int factionId, int accountId)
        {

            try
            {
                var i = ServerFactionMembers_.FirstOrDefault(x => x.accountId == accountId && x.factionId == factionId);
                if (i == null) return;
                ServerFactionMembers_.Remove(i);
                Database.SystemDatabase.RemoveFactionMember(factionId, accountId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AddFactionMember(int factionId, int accountId, int rankId)
        {

            try
            {
                if (IsAccountInAnyFaction(accountId) && GetAccountFaction(accountId) == factionId) return;
                AddFactionMemberEntryToList(ServerFactionMembers_.Last().id + 1, factionId, accountId, rankId);
                Database.SystemDatabase.AddFactionMember(factionId, accountId, rankId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void SetAccountRank(int factionId, int accountId, int newRank)
        {

            try
            {
                var i = ServerFactionMembers_.FirstOrDefault(x => x.factionId == factionId && x.accountId == accountId);
                if (i == null) return;
                i.rankId = newRank;
                Database.SystemDatabase.UpdateFactionMemberRank(factionId, accountId, newRank);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static bool IsAccountInAnyFaction(int accountId)
        {

            try
            {
                return ServerFactionMembers_.ToList().Exists(x => x.accountId == accountId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        public static int GetAccountFaction(int accountId)
        {

            try
            {
                var i = ServerFactionMembers_.ToList().FirstOrDefault(x => x.accountId == accountId);
                if (i != null) return i.factionId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public static int GetAccountRank(int accountId)
        {

            try
            {
                var i = ServerFactionMembers_.ToList().FirstOrDefault(x => x.accountId == accountId);
                if (i != null) return i.rankId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
        #endregion
    }
}
