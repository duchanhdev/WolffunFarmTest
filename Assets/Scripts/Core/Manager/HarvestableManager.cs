using System;
using System.Collections.Generic;
using Core.Data;
using Core.Entities;
using Newtonsoft.Json;

namespace Core.Manager
{
    public enum HarvestableEnum
    {
        Tomato = 1,
        Blueberry = 2,
        Strawberry = 3,
        Cow = 4,
    }
    public class HarvestableManager
    {
        public static string FileName = "HarvestableManager";
        [JsonProperty]
        private readonly List<HarvestableData> _harvestableDatas = new List<HarvestableData>();
        
        private readonly List<Harvestable> _harvestables = new List<Harvestable>();
        public HarvestableManager()
        {
        }
        public Harvestable CreateHarvestable(int type, DateTime growTime, string landId, HarvestableData data = null)
        {
            Harvestable harvestable = null;
            if (data == null)
            {
                data = new HarvestableData(type, growTime, landId);
                _harvestableDatas.Add(data);
            }
            
            switch (type)
            {
                case (int)HarvestableEnum.Tomato:
                    harvestable = new Tomato(data);
                    break;
                case (int)HarvestableEnum.Blueberry:
                    harvestable = new Blueberry(data);
                    break;
                case (int)HarvestableEnum.Strawberry:
                    harvestable = new Strawberry(data);
                    break;
                case (int)HarvestableEnum.Cow:
                    harvestable = new Cow(data);
                    break;
                default:
                    harvestable = null;
                    break;
            }
            _harvestables.Add(harvestable);
            Save();
            return harvestable;
        }

        public void UpdateTimeNowAll()
        {
            foreach (Harvestable harvestable in _harvestables)
            {
                harvestable.UpdateTime(DateTime.Now);
            }
        }

        public void InitAfterLoad()
        {
            foreach (var data in _harvestableDatas)
            {
                CreateHarvestable(data.Type, data.GrowTime, data.LandId, data);
            }
        }

        public void UpdateAfterLoad()
        {
            UpdateTimeNowAll();
        }

        public void RemoveHarvestable(Harvestable harvestable)
        {
            _harvestableDatas.RemoveAll(data => data.Id == harvestable.Id);
            if (GameManager.Instance.LandManager.GetLand(harvestable.LandId).HarvestableId == harvestable.Id)
            {
                GameManager.Instance.LandManager.FreeLand(harvestable.LandId);
            }
            Save();
        }

        public void Harvest(string harvestableId)
        {
            var harvestable = GetHarvestable(harvestableId);
            harvestable.Harvest();
            Save();
        }

        public Harvestable GetHarvestable(string harvestableId)
        {
            return _harvestables.Find(harvestable => harvestable.Id == harvestableId);
        }

        public void Save()
        {
            SaveLoadManager.Save<HarvestableManager>(this, FileName);
        }
    }
}