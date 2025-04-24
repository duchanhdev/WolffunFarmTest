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
        public int Id => _data.Id;
        public int Type => _data.Type;
        public string Name => _typeData.Name;
        public int MaxYield => _typeData.MaxYield;
        public int ProducedCount => _data.ProducedCount;
        public int PendingProducts => _data.PendingProducts;
        public float YieldTime => _typeData.YieldTime;
        public float TimeSinceLastYield => _data.TimeSinceLastYield;
        public DateTime LastProduceTime => _data.LastProduceTime;
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