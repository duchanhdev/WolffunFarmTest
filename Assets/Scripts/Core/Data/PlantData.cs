using System;

namespace Core.Data
{
    public abstract class PlantData: HarvestableData
    {
        protected PlantData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}