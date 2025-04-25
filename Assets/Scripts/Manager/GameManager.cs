using UnityEngine;

namespace Data.Configs
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ConfigManager Configs { get; private set; }
        
        public PlayerResourceManager PlayerResources { get; private set; }
        public HarvestableManager HarvestableManager { get; private set; }
        public ShopManager ShopManager { get; private set; }
        
        public LandManager LandManager { get; private set; }

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
            LoadManager();
            ShopManager = new ShopManager();
            UpdateManagerAfterLoad();
            
        }

        public void LoadManager()
        {
            PlayerResources = SaveLoadManager.Load<PlayerResourceManager>(PlayerResourceManager.FileName);
            LandManager = SaveLoadManager.Load<LandManager>(LandManager.FileName);
            HarvestableManager = SaveLoadManager.Load<HarvestableManager>(HarvestableManager.FileName);
        }

        public void UpdateManagerAfterLoad()
        {
            PlayerResources.LoadConfig();
            LandManager.UpdateAfterLoad();
            HarvestableManager.UpdateAfterLoad();
        }
        
        public bool ExpandLand()
        {
            var price = GameManager.Instance.Configs.GlobalConfig.GetInt("Land_ExpansionPrice");
            if (PlayerResources.Gold < price)
            {
                return false;
            }

            PlayerResources.SpendGold(price);
            LandManager.ExpandLand();
            return true;
        }

        public void Update()
        {
            HarvestableManager.UpdateTimeAll(Time.deltaTime);
        }
    }
}