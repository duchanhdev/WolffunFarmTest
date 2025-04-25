using System;

namespace Core.Data
{
    public class StrawberryData: PlantData
    {
        public StrawberryData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}