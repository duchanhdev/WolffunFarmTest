using Core.Configs;

namespace Core.Manager
{
    public class ConfigManager
    {
        public GlobalConfig GlobalConfig { get; private set; }
        public HarvestableConfig HarvestableConfig { get; private set; }
        public ProductConfig ProductConfig { get; private set; }
        public SeedAnimalConfig SeedAnimalConfig { get; private set; }
        public ShopConfig ShopConfig { get; private set; }

        public ConfigManager()
        {
            LoadAllConfigs();
        }

        private void LoadAllConfigs()
        {
            GlobalConfig = new GlobalConfig();
            GlobalConfig.LoadFromFile();

            HarvestableConfig = new HarvestableConfig();
            HarvestableConfig.LoadFromFile();

            ProductConfig = new ProductConfig();
            ProductConfig.LoadFromFile();

            SeedAnimalConfig = new SeedAnimalConfig();
            SeedAnimalConfig.LoadFromFile();

            ShopConfig = new ShopConfig();
            ShopConfig.LoadFromFile();
        }
    }
}