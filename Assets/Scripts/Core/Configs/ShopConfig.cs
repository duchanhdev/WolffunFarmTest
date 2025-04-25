using System.IO;
using Core.Configs.Base;

namespace Core.Configs
{
    public class ShopConfig : CsvConfigReader<ShopConfig.RowData>
    {
        protected override string FilePath => Path.GetFullPath(@"Assets\Configs\ShopConfig.csv");
        
        public struct RowData
        {
            public int Id;
            public int SeedAnimalId;
            public int BuyUnit;
            public int BuyPrice;
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
                SeedAnimalId = int.Parse(values[1]),
                BuyUnit = int.Parse(values[2]),
                BuyPrice = int.Parse(values[3]),
            };
        }

    }
}