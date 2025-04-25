using System;
using System.IO;
using Newtonsoft.Json;

namespace Core.Manager
{
    public static class SaveLoadManager
    {
        private static string GetSavePath(string fileName)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WolffunFarmTestGame");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return Path.Combine(folderPath, fileName);
        }

        public static void Save<T>(T data, string fileName)
        {
            string savePath = GetSavePath(fileName);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(savePath, json);
            Console.WriteLine($"[Save] Data saved to {savePath}");
        }

        public static T Load<T>(string fileName) where T : new()
        {
            string savePath = GetSavePath(fileName);
            if (!File.Exists(savePath))
            {
                Console.WriteLine("[Load] No save file found.");
                return new T();
            }

            string json = File.ReadAllText(savePath);
            T data = JsonConvert.DeserializeObject<T>(json);
            Console.WriteLine($"[Load] Data loaded from {savePath}");
            return data;
        }

        public static void DeleteSave(string fileName)
        {
            string savePath = GetSavePath(fileName);
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Console.WriteLine("[Save] Save file deleted.");
            }
            else
            {
                Console.WriteLine("[DeleteSave] File not found.");
            }
        }
    }
}
