using System;

namespace Core.Data
{
    public abstract class AnimalData:HarvestableData
    {
        protected AnimalData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}