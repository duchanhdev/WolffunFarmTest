using Configs;

namespace Data.Configs
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
            GlobalConfig.LoadFromResources();

            HarvestableConfig = new HarvestableConfig();
            HarvestableConfig.LoadFromResources();

            ProductConfig = new ProductConfig();
            ProductConfig.LoadFromResources();

            SeedAnimalConfig = new SeedAnimalConfig();
            SeedAnimalConfig.LoadFromResources();

            ShopConfig = new ShopConfig();
            ShopConfig.LoadFromResources();
        }
    }
}