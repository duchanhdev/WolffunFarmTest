using System;

namespace Core.Data
{
    public class TomatoData: PlantData
    {
        public TomatoData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}