using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerItems
    {
        public partial class Server_Items
        {
            public int id { get; set; }
            public string itemName { get; set; }
            public float itemWeight { get; set; }
            public bool hasItemAnimation { get; set; }
            public string itemAnimationName { get; set; }
            public bool isItemDroppable { get; set; }
            public bool isItemUseable { get; set; }
            public string itemPic { get; set; }
        }

        public static List<Server_Items> ServerItems_ = new List<Server_Items>();

        public static float GetItemWeight(string itemName)
        {
            try
            {
                var item = ServerItems_.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) return item.itemWeight;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0f;
        }

        public static string GetItemPic(string itemName)
        {
            try
            {
                var item = ServerItems_.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) return item.itemPic;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static bool IsItemDroppable(string itemName)
        {
            try
            {
                var item = ServerItems_.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) return item.isItemDroppable;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static string GetNormalItemName(string itemName)
        {
            try
            {
                if (itemName == "test") itemName = "test";

                return itemName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return itemName;
        }
    }
}
