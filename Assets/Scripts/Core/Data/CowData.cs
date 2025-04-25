using System;

namespace Core.Data
{
    public class CowData: AnimalData
    {
        public CowData(int type, DateTime growTime, string landId) : base(type, growTime,landId)
        {
        }
    }
}