using System.Collections.Generic;
using Core.Entities;

namespace Data.Configs
{
    public class LandManager
    {
        public readonly List<Land> Lands = new List<Land>();
        public int TotalLand => Lands.Count;

        public LandManager()
        {
            Lands.Clear();
        }
        
        public void ExpandLand()
        {
            Lands.Add(new Land());
        }
        
        public bool UseLand(string landId, int harvestableType)
        {
            var land = GetLand(landId);
            if (land == null || land.Status != (int)Land.LandStatus.Empty) return false;
            land.Grow(harvestableType);
            return true;
        }

        public void FreeLand(string landId)
        {
            var land = GetLand(landId);
            if (land != null) land.Free();
        }

        public Land GetLand(string landId)
        {
            for (int i = 0; i < Lands.Count; i++)
            {
                if (landId == Lands[i].Id) return Lands[i];
            }
            return null;
        }
    }
}