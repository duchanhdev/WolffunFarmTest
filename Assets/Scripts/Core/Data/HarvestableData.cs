using System;

namespace Core.Data
{
    public abstract class HarvestableData
    {
        public int Id;
        public int Type;
        public int ProducedCount;
        public int PendingProducts;
        public float TimeSinceLastYield;
        public DateTime LastProduceTime;
        public bool IsDead;
        public DateTime DeathTime;
    }
}