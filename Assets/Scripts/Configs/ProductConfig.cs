using Configs.Base;

namespace Configs
{
    public class ProductConfig : CsvConfigReader<ProductConfig.RowData>
    {
        protected override string FileName => "Configs/ProductConfig";
        
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