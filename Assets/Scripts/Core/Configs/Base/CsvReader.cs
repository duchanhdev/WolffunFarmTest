using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Configs.Base
{
    public abstract class CsvConfigReader<T> where T : new()
    {
        public List<T> Table { get; protected set; } = new List<T>();

        protected abstract string FilePath { get; }

        public void LoadFromFile()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine($"[CSV] File not found: {FilePath}");
                return;
            }

            using (var reader = new StreamReader(FilePath))
            {
                string line;
                int lineIndex = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineIndex++;
                    if (lineIndex <= 2) continue;

                    var values = line.Split(',');

                    T row = ParseRow(values);
                    Table.Add(row);
                }
            }
        }

        protected abstract T ParseRow(string[] values);
    }
}