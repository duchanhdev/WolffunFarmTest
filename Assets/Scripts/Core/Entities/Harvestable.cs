using System;
using Configs;
using Core.Data;
using Data.Configs;

namespace Core.Entities
{
    public class Harvestable
    {
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
        public float TimeSinceLastYield => _data.TimeSinceLastYield;
        public DateTime LastProduceTime => _data.LastProduceTime;
        public DateTime GrowTime => _data.GrowTime;
        public bool IsDead => _data.IsDead;
        public DateTime DeathTime => _data.DeathTime;
        public float HarvestWindowDuration => HarvestWindowDuration;

        public Harvestable(HarvestableData data)
        {
            _data = data;
            _typeData = GameManager.Instance.Configs.HarvestableConfig.FindById(data.Type);
        }

        public void Produce()
        {
            if (IsDead) return;

            if (ProducedCount < MaxYield)
            {
                int count = (int)(TimeSinceLastYield / YieldTime);
                if (count > MaxYield - ProducedCount) count = MaxYield - ProducedCount;
                _data.ProducedCount+=count;
                _data.PendingProducts+=count;
                _data.LastProduceTime = LastProduceTime.AddSeconds(count * YieldTime);
                _data.TimeSinceLastYield = 0;
            }

            if (ProducedCount >= MaxYield && DateTime.Now.Subtract(LastProduceTime).TotalSeconds >= HarvestWindowDuration)
            {
                _data.IsDead = true;
                _data.PendingProducts = 0;
                _data.DeathTime = LastProduceTime.AddSeconds(HarvestWindowDuration);
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

        public void UpdateTime(float deltaTime)
        {
            if (IsDead) return;

            _data.TimeSinceLastYield += deltaTime;

            if (TimeSinceLastYield >= YieldTime || 
                (ProducedCount >= MaxYield && DateTime.Now.Subtract(LastProduceTime).TotalSeconds >= HarvestWindowDuration))
            {
                Produce();
            }

        }
    }
}