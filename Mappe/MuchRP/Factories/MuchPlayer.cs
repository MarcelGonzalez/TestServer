using AltV.Net.Elements.Entities;
using MuchRP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuchRP.Factories
{
    public class MuchPlayer : Player
    {
        public int accountId { get; set; } = 0;
        public string accountName { get; set; } = "undefined";
        public int adminlevel { get; set; } = 0;
        public int gender { get; set; } = 0; //0 male, 1 female
        public bool isCrimeFlagged { get; set; } = false;
        public int cash { get; set; } = 0; 
        public int bank { get; set; } = 0;
        public bool isLaptopActivated { get; set; } = false;
        public List<AccountsInventory.Accounts_Inventory> AccountInventory_ { get; set; } = new List<AccountsInventory.Accounts_Inventory>();

        public MuchPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
        }
    }
}
