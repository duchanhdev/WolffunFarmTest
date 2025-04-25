using System;
using Core.Manager;

namespace Core.Data
{
    public abstract class HarvestableData
    {
        public string Id;
        public int Type;
        public int ProducedCount;
        public int PendingProducts;
        public DateTime GrowTime;
        public DateTime LastProduceTime;
        public float TimeSinceLastYield;
        public bool IsDead;
        public DateTime DeathTime;
        public string LandId;

        public HarvestableData(int type, DateTime growTime, string landId)
        {
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            
            Id = Guid.NewGuid().ToString();;
            Type = type;
            ProducedCount = 0;
            PendingProducts = 0;
            GrowTime = growTime;
            LastProduceTime = growTime.AddSeconds(globalConfig.GetFloat("Worker_ActionTimeSeconds"));
            TimeSinceLastYield = 0;
            IsDead = false;
            LandId = landId;
        }
    }
}