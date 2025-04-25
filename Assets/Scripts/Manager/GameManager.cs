using UnityEngine;

namespace Data.Configs
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ConfigManager Configs { get; private set; }
        
        public PlayerResourceManager PlayerResources { get; private set; }
        public HarvestableManager HarvestableManager { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Configs = new ConfigManager();
            PlayerResources = new PlayerResourceManager();
            HarvestableManager = new HarvestableManager();
            Debug.Log("Gold: "+Configs.GlobalConfig.GetInt("Game_Goal_Gold"));
            Debug.Log("Name: "+Configs.ProductConfig.Table[2].ProductName);
        }

        public void Update()
        {
            HarvestableManager.UpdateTimeAll(Time.deltaTime);
        }
    }
}