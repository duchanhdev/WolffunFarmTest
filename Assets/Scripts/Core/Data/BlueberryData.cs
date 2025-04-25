using System;

namespace Core.Data
{
    public class BlueberryData: PlantData
    {
        public BlueberryData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}