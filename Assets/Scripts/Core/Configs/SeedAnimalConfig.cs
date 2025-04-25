using System.IO;
using Core.Configs.Base;

namespace Core.Configs
{
    public class SeedAnimalConfig : CsvConfigReader<SeedAnimalConfig.RowData>
    {
        protected override string FilePath => Path.GetFullPath(@"Assets\Configs\SeedAnimalConfig.csv");

        public struct RowData
        {
            public int Id;
            public string Name;
            public int Type;
            public int HarvestableId;
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
                HarvestableId = int.Parse(values[3]),
            };
        }

    }
}