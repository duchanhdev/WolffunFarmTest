using System;
using Core.Data;

namespace Core.Entities
{
    public class Harvestable
    {
        protected HarvestableData _data;
        protected HarvestableTypeData _typeData;
        public int Id => _data.Id;
        public int Type => _data.Type;
        public string Name => _typeData.Name;
        public int TotalYield => _typeData.TotalYield;
        public int ProducedCount => _data.ProducedCount;
        public int PendingProducts => _data.PendingProducts;
        public float TimeToNextYield => _typeData.TimeToNextYield;
        public float TimeSinceLastYield => _data.TimeSinceLastYield;
        public DateTime LastProduceTime => _data.LastProduceTime;
        public bool IsDead => _data.IsDead;
        public DateTime DeathTime => _data.DeathTime;
        public float HarvestWindowDuration => HarvestWindowDuration;

        public Harvestable(HarvestableData data)
        {
            _data = data;
        }

        public void Produce()
        {
            if (IsDead) return;

            if (ProducedCount < TotalYield)
            {
                int count = (int)(TimeSinceLastYield / TimeToNextYield);
                if (count > TotalYield - ProducedCount) count = TotalYield - ProducedCount;
                _data.ProducedCount+=count;
                _data.PendingProducts+=count;
                _data.LastProduceTime = LastProduceTime.AddSeconds(count * TimeToNextYield);
                _data.TimeSinceLastYield = 0;
            }

            if (ProducedCount >= TotalYield && DateTime.Now.Subtract(LastProduceTime).TotalSeconds >= HarvestWindowDuration)
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

            if (TimeSinceLastYield >= TimeToNextYield || 
                (ProducedCount >= TotalYield && DateTime.Now.Subtract(LastProduceTime).TotalSeconds >= HarvestWindowDuration))
            {
                Produce();
            }

        }
    }
}