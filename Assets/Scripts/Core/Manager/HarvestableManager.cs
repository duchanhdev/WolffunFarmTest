using System;
using System.Collections.Generic;
using Core.Data;
using Core.Entities;

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
        public readonly List<Harvestable> Harvestables = new List<Harvestable>();
        public HarvestableManager()
        {
        }
        public Harvestable CreateHarvestable(int type, DateTime growTime, string landId)
        {
            Harvestable harvestable = null;
            switch (type)
            {
                case (int)HarvestableEnum.Tomato:
                    harvestable = new Tomato(new TomatoData(type, growTime, landId));
                    break;
                case (int)HarvestableEnum.Blueberry:
                    harvestable = new Blueberry(new BlueberryData(type, growTime, landId));
                    break;
                case (int)HarvestableEnum.Strawberry:
                    harvestable = new Strawberry(new StrawberryData(type, growTime, landId));
                    break;
                case (int)HarvestableEnum.Cow:
                    harvestable = new Cow(new CowData(type, growTime, landId));
                    break;
                default:
                    harvestable = null;
                    break;
            }
            Harvestables.Add(harvestable);
            Save();
            return harvestable;
        }

        public void UpdateTimeAll(float delta)
        {
            foreach (Harvestable harvestable in Harvestables)
            {
                harvestable.UpdateTime(delta);
            }
        }

        public void UpdateAfterLoad()
        {
            foreach (Harvestable harvestable in Harvestables)
            {
                harvestable.UpdateTime((float)DateTime.Now.Subtract(harvestable.GrowTime).TotalSeconds);
            }
        }

        public void RemoveHarvestable(Harvestable harvestable)
        {
            Harvestables.Remove(harvestable);
            if (GameManager.Instance.LandManager.GetLand(harvestable.LandId).HarvestableId == harvestable.LandId)
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
            return Harvestables.Find(harvestable => harvestable.LandId == harvestableId);
        }

        public void Save()
        {
            SaveLoadManager.Save<HarvestableManager>(this, FileName);
        }
    }
}