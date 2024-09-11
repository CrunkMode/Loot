using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace loot
{
    class SaveData
    {
        public Player playerData { get; set; }
        public MetaStatistics meta { get; set; }
    }
    class SaveLoadManager
    {
        public static void SavePlayer(SaveData data, string filePath)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static SaveData LoadData(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter to continue");
                Console.Read();
                return null;
            }
        }
    }
}
