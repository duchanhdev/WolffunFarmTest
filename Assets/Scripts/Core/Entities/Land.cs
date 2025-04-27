using System;
using System.Threading;
using Core.Manager;
using Newtonsoft.Json;

namespace Core.Entities
{
    public class Land
    {
        public enum LandStatus
        {
            Empty = 0,
            Growing,
            Harvesting,
            Using
        }

        public string Id;
        
        [JsonProperty]
        public int Status { get; private set; }
        
        [JsonProperty]
        public string HarvestableId { get; private set; }
        
        [JsonProperty]
        public int HarvestableType { get; private set; }
        public DateTime GrowTime;
        public DateTime HarvestTime;
        static Timer timer;

        public Land()
        {
            Id = Guid.NewGuid().ToString();
            HarvestableId = "";
            Status = (int)LandStatus.Empty;
        }

        public void Free()
        {
            HarvestableId = "";
            Status = (int)LandStatus.Empty;
            Save();
        }

        public void Grow(int harvestableType)
        {
            var playerResources = GameManager.Instance.PlayerResources;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            if (playerResources.IdleWorkers <= 0) return;
            Status = (int)LandStatus.Growing;
            HarvestableType = harvestableType;
            GrowTime = DateTime.Now;
            playerResources.AssignWorker();
            timer = new Timer(Use, null, TimeSpan.FromSeconds(globalConfig.GetFloat("Worker_ActionTimeSeconds")),
                Timeout.InfiniteTimeSpan);
            Save();
        }

        private void Use(object state)
        {
            if (Status != (int)LandStatus.Growing) return;
            var playerResources = GameManager.Instance.PlayerResources;
            playerResources.FreeWorker();
            Status = (int)LandStatus.Using;
            HarvestableId = GameManager.Instance.HarvestableManager.CreateHarvestable(HarvestableType, GrowTime, Id).Id;
            Save();
        }

        public void Harvest()
        {
            if (!GameManager.Instance.HarvestableManager.GetHarvestable(HarvestableId).CanHarvest()) return;
            var playerResources = GameManager.Instance.PlayerResources;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            if (playerResources.IdleWorkers <= 0) return;
            Status = (int)LandStatus.Harvesting;
            HarvestTime = DateTime.Now;
            playerResources.AssignWorker();
            timer = new Timer(HarvestEnd, null, TimeSpan.FromSeconds(globalConfig.GetFloat("Worker_ActionTimeSeconds")),
                Timeout.InfiniteTimeSpan);
            Save();
            
        }

        private void HarvestEnd(object state)
        {
            if (Status != (int)LandStatus.Harvesting) return;
            var playerResources = GameManager.Instance.PlayerResources;
            playerResources.FreeWorker();
            Status = (int)LandStatus.Using;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            float growSeconds = globalConfig.GetFloat("Worker_ActionTimeSeconds");
            GameManager.Instance.HarvestableManager.GetHarvestable(HarvestableId)
                .UpdateTime(GrowTime.AddSeconds(growSeconds));
            GameManager.Instance.HarvestableManager.Harvest(HarvestableId);
            Save();
        }

        public void UpdateGrow()
        {
            if (Status != (int)LandStatus.Growing) return;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            float growSeconds = globalConfig.GetFloat("Worker_ActionTimeSeconds");
            if (GrowTime.AddSeconds(growSeconds) < DateTime.Now)
            {
                Use(null);
            }
            else
            {
                timer = new Timer(Use, null,
                    TimeSpan.FromSeconds((float)(growSeconds - DateTime.Now.Subtract(GrowTime).TotalSeconds)),
                    Timeout.InfiniteTimeSpan);
            }
        }

        public void UpdateHarvest()
        {
            if (Status != (int)LandStatus.Harvesting) return;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            float growSeconds = globalConfig.GetFloat("Worker_ActionTimeSeconds");
            if (HarvestTime.AddSeconds(growSeconds) < DateTime.Now)
            {
                HarvestEnd(null);
            }
            else
            {
                timer = new Timer(HarvestEnd, null,
                    TimeSpan.FromSeconds((float)(growSeconds - DateTime.Now.Subtract(HarvestTime).TotalSeconds)),
                    Timeout.InfiniteTimeSpan);
            }
        }

        public string GetNameHarvestable()
        {
            if (Status == (int)LandStatus.Empty) return string.Empty;
            return GameManager.Instance.Configs.HarvestableConfig.FindById(HarvestableType).Name;
        }

        public void Save()
        {
            GameManager.Instance.LandManager.Save();
        }
    }
}