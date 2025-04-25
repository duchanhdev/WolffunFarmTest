using System;
using System.Threading;
using Data.Configs;

namespace Core.Entities
{
    public class Land
    {
        public enum LandStatus
        {
            Empty = 0,
            Growing,
            Using
        }

        public string Id;
        public int Status { get; private set; }
        public string HarvestableId { get; private set; }
        public int HarvestableType { get; private set; }
        public DateTime GrowTime;
        static Timer timer;

        public Land()
        {
            Id = Guid.NewGuid().ToString();
            Free();
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

        public void UpdateGrow()
        {
            if (Status != (int)LandStatus.Growing) return;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            float growSeconds = globalConfig.GetFloat("Worker_ActionTimeSeconds");
            if (GrowTime.AddSeconds(growSeconds) >= DateTime.Now)
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

        public void Save()
        {
            GameManager.Instance.LandManager.Save();
        }
    }
}