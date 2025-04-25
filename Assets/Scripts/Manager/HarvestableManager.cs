using System;
using System.Collections.Generic;
using Core.Data;
using Core.Entities;
using Unity.VisualScripting;

namespace Data.Configs
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
        public readonly List<Harvestable> Harvestables = new List<Harvestable>();
        public HarvestableManager()
        {
            Harvestables.Clear();
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
            return harvestable;
        }

        public void UpdateTimeAll(float delta)
        {
            foreach (Harvestable harvestable in Harvestables)
            {
                harvestable.UpdateTime(delta);
            }
        }

        public void UpdateAll()
        {
            foreach (Harvestable harvestable in Harvestables)
            {
                harvestable.UpdateTime((float)DateTime.Now.Subtract(harvestable.GrowTime).TotalSeconds);
            }
        }
    }
}