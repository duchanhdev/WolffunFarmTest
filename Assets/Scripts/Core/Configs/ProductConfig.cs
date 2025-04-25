using System.IO;
using Core.Configs.Base;

namespace Core.Configs
{
    public class ProductConfig : CsvConfigReader<ProductConfig.RowData>
    {
        protected override string FilePath => Path.GetFullPath(@"Assets\Configs\ProductConfig.csv");
        
        public struct RowData
        {
            public int Id;
            public string ProductName;
            public int SellPrice;
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
                ProductName = values[1],
                SellPrice = int.Parse(values[2]),
            };
        }

    }
}