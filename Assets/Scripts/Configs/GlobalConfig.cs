using Configs.Base;

namespace Configs
{
    public class GlobalConfig: CsvConfigReader<GlobalConfig.RowData>
    {
        
        public struct RowData
        {
            public string Key;
            public string Value;
        }
        
        protected override string FileName => "Configs/GlobalConfig";
        
        public string GetString(string key)
        {
            var entry = Table.Find(config => config.Key == key);
            return entry.Equals(default) ? null : entry.Value;
        }
        
        public int GetInt(string key)
        {
            var entry = Table.Find(config => config.Key == key);
            return int.Parse(entry.Value);
        }
        
        public float GetFloat(string key)
        {
            var entry = Table.Find(config => config.Key == key);
            return float.Parse(entry.Value);
        }

        protected override RowData ParseRow(string[] values)
        {
            return new RowData()
            {
                Key = values[0],
                Value = values[1],
            };
        }
    }
}