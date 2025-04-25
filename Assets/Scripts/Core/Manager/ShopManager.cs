using System.Collections.Generic;
using Core.Configs;

namespace Core.Manager
{
    public class ShopManager
    {
        public List<ShopConfig.RowData> Items => GameManager.Instance.Configs.ShopConfig.Table;

        public void BuySeedAnimal(int itemId)
        {
            var item = GetSeedAnimal(itemId);
            if (GameManager.Instance.PlayerResources.Gold >= item.BuyPrice)
            {
                GameManager.Instance.PlayerResources.SpendGold(item.BuyPrice);
                GameManager.Instance.PlayerResources.AddSeed(itemId,item.BuyUnit);
            }
        }

        public ShopConfig.RowData GetSeedAnimal(int itemId)
        {
            return Items.Find(item => item.Id == itemId);
        }
    }
}