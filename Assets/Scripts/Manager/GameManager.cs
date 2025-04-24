using UnityEngine;

namespace Data.Configs
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ConfigManager Configs { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Configs = new ConfigManager();
            Debug.Log("Gold: "+Configs.GlobalConfig.GetInt("Game_Goal_Gold"));
            Debug.Log("Name: "+Configs.ProductConfig.Table[2].ProductName);
        }
    }
}