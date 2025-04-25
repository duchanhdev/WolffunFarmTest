using System;
using Data.Configs;
using DG.Tweening;

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
            DOVirtual.DelayedCall(globalConfig.GetFloat("Worker_ActionTimeSeconds"), () => {
                Use();
            });
            Save();
        }

        private void Use()
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
                Use();
            }
            else
            {
                DOVirtual.DelayedCall((float)(growSeconds - DateTime.Now.Subtract(GrowTime).TotalSeconds), () => {
                    Use();
                });
            }
        }

        public void Save()
        {
            GameManager.Instance.LandManager.Save();
        }
    }
}