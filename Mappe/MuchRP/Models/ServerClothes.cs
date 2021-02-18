using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuchRP.Models
{
    class ServerClothes
    {
        public partial class Server_Clothes
        {
            public int id { get; set; }
            public string clothesName { get; set; }
            public string type { get; set; }
            public int draw { get; set; }
            public int texture { get; set; }
            public int gender { get; set; } // 0 = Male, 1 = Female, 2 = Unisex
            public int faction { get; set; }
        }

        public static List<Server_Clothes> ServerClothes_ = new List<Server_Clothes>();

        public static void AddEntryToList(int id, string clothesName, string type, int draw, int texture, int gender, int faction)
        {
            try
            {
                ServerClothes_.Add(new Server_Clothes
                {
                    id = id,                    
                    clothesName = clothesName,
                    type = type,
                    draw = draw,
                    texture = texture,
                    gender = gender,
                    faction = faction
                });
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static int GetClothesGender(string clothesName)
        {
            try
            {
                var clothes = ServerClothes_.ToList().FirstOrDefault(x => x.clothesName == clothesName);
                if (clothes != null) return clothes.gender;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static string GetClothesType(string clothesName)
        {
            try
            {
                var clothes = ServerClothes_.ToList().FirstOrDefault(x => x.clothesName == clothesName);
                if (clothes != null) return clothes.type;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return "";
        }

        public static int GetClothesDraw(string clothesName)
        {
            try
            {
                var clothes = ServerClothes_.ToList().FirstOrDefault(x => x.clothesName == clothesName);
                if (clothes != null) return clothes.draw;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static bool ExistClothes(string clothesName)
        {
            try
            {
                return ServerClothes_.ToList().Exists(x => x.clothesName == clothesName);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static int GetClothesTexture(string clothesName)
        {
            try
            {
                var clothes = ServerClothes_.ToList().FirstOrDefault(x => x.clothesName == clothesName);
                if (clothes != null) return clothes.texture;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }
    }
}
