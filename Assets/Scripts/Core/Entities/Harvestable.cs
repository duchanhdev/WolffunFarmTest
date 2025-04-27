using System;
using Core.Configs;
using Core.Data;
using Core.Manager;
using Newtonsoft.Json;

namespace Core.Entities
{
    public class Harvestable
    {
        [JsonProperty]
        protected HarvestableData _data;
        protected HarvestableConfig.RowData _typeData;
        private GlobalConfig GlobalConfig => GameManager.Instance.Configs.GlobalConfig;
        private int EquipmentLevel => GameManager.Instance.PlayerResources.EquipmentLevel;
        public string Id => _data.Id;
        public int Type => _data.Type;
        public string Name => _typeData.Name;
        public int MaxYield => _typeData.MaxYield;
        public int ProducedCount => _data.ProducedCount;
        public int PendingProducts => _data.PendingProducts;
        public string LandId => _data.LandId;

        public float YieldTime => (float)(_typeData.YieldTime *
                                          Math.Pow(1 - GlobalConfig.GetFloat("Equipment_UpgradeEffectRate"),
                                              EquipmentLevel - 1));
        public DateTime LastProduceTime => _data.LastProduceTime;
        public DateTime GrowTime => _data.GrowTime;
        public bool IsDead => _data.IsDead;
        public DateTime DeathTime => _data.DeathTime;
        public float HarvestWindowDuration => GameManager.Instance.Configs.GlobalConfig.GetFloat("LastYield_ExpirationSeconds");

        public Harvestable(HarvestableData data)
        {
            _data = data;
            _typeData = GameManager.Instance.Configs.HarvestableConfig.FindById(data.Type);
        }

        public void Produce(DateTime updateTime)
        {
            if (IsDead) return;
            var timeSinceLastYield = (updateTime - LastProduceTime).TotalSeconds;

            if (timeSinceLastYield >= YieldTime && ProducedCount < MaxYield)
            {
                int count = (int)(timeSinceLastYield / YieldTime);
                if (count > MaxYield - ProducedCount) count = MaxYield - ProducedCount;
                _data.ProducedCount+=count;
                _data.PendingProducts+=count;
                _data.LastProduceTime = LastProduceTime.AddSeconds(count * YieldTime);
            }
            
            _data.LastUpdateTime = updateTime;
            
            if (ProducedCount >= MaxYield && updateTime.Subtract(LastProduceTime).TotalSeconds >= HarvestWindowDuration)
            {
                _data.IsDead = true;
                _data.PendingProducts = 0;
                _data.DeathTime = LastProduceTime.AddSeconds(HarvestWindowDuration);
                GameManager.Instance.HarvestableManager.RemoveHarvestable(this);
            }
        }

        public bool CanHarvest()
        {
            return !IsDead && PendingProducts > 0;
        }

        public void Harvest()
        {
            if (CanHarvest())
            {
                GameManager.Instance.PlayerResources.AddProduct(_typeData.ProductId, _data.ProducedCount);
                _data.PendingProducts = 0;
            }
        }

        public void UpdateTime(DateTime updateTime)
        {
            if (IsDead || updateTime < _data.LastUpdateTime) return;
            Produce(updateTime);
        }
    }
}