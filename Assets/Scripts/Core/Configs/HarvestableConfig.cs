using System.IO;
using Core.Configs.Base;

namespace Core.Configs
{
    public class HarvestableConfig : CsvConfigReader<HarvestableConfig.RowData>
    {
        protected override string FilePath => Path.GetFullPath(@"Assets\Configs\HarvestableConfig.csv");
        
        
        public struct RowData
        {
            public int Id;
            public string Name;
            public int Type; 
            public int YieldTime;
            public int YieldCount;
            public int MaxYield;
            public int ProductId;
        }
        
        public RowData FindById(int id)
        {
            return Table.Find(config => config.Id == id);
        }

        protected override RowData ParseRow(string[] values)
        {
            return new RowData
            {
                Id = int.Parse(values[0]),
                Name = values[1],
                Type = int.Parse(values[2]),
                YieldTime = int.Parse(values[3]),
                YieldCount = int.Parse(values[4]),
                MaxYield = int.Parse(values[5]),
                ProductId = int.Parse(values[6]),
            };
        }
    }
}