using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ser_ver
{
    internal static class Serialization
    {
        public static List<T> Input<T>()
        {
            List<T> list = new List<T>();

            string fs = File.ReadAllText(@"D:\Универ\курсовая\ser_ver\user.json");
            if (fs.Length != 0)
            {
                list = JsonSerializer.Deserialize<List<T>>(fs) ?? new List<T>();
            }

            return list;
        }

        public static void Output<T>(List<T> list)
        {

            string us = JsonSerializer.Serialize(list);
            File.WriteAllText(@"D:\Универ\курсовая\ser_ver\user.json", us);
        }
        public static List<T> UserIn<T>()
        {
            List<T> user = new List<T>();

            string fs = File.ReadAllText(@"D:\Универ\курсовая\ser_ver\profilUser.json");
            if (fs.Length != 0)
            {
                user = JsonSerializer.Deserialize<List<T>>(fs);
            }

            return user;
        }
        public static void UserOut<T>(List<T> user)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string us = JsonSerializer.Serialize(user);
            File.WriteAllText(@"D:\Универ\курсовая\ser_ver\profilUser.json", us);
        }
        public static void LogOut<T>(List<T> user)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using FileStream fs = new FileStream(@"D:\Универ\курсовая\ser_ver\profilUser.json", FileMode.Create);
            JsonSerializer.SerializeAsync(fs, user, options);
        }
    }
}
