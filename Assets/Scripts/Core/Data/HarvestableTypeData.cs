namespace Core.Data
{
    public struct HarvestableTypeData
    {
        public int Type { get; private set; }
        public string Name { get; private set; }
        public int TotalYield { get; private set; }
        public float TimeToNextYield { get; private set; }
        public float HarvestWindowDuration;
    }
}