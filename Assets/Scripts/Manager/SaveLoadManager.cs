using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.Configs
{
    public static class SaveLoadManager
    {
        public static void Save<T>(T data,string fileName)
        {
            string savePath = Path.Combine(Application.persistentDataPath, fileName);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(savePath, json);
            Debug.Log($"[Save] Data saved to {savePath}");
        }

        public static T Load<T>(string fileName) where T : new()
        {
            string savePath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("[Load] No save file found.");
                return new T(); // default data
            }

            string json = File.ReadAllText(savePath);
            T data = JsonConvert.DeserializeObject<T>(json);
            Debug.Log($"[Load] Data loaded from {savePath}");
            return data;
        }

        public static void DeleteSave(string fileName)
        {
            string savePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("[Save] Save file deleted.");
            }
        }
    }
}