using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Configs.Base
{
    public abstract class CsvConfigReader<T> where T : new()
    {
        public List<T> Table { get; protected set; } = new List<T>();

        protected abstract string FileName { get; }

        public void LoadFromResources()
        {
            TextAsset csvFile = Resources.Load<TextAsset>(FileName);
            if (csvFile == null)
            {
                Debug.LogError($"[CSV] File not found: {FileName}");
                return;
            }

            using (StringReader reader = new StringReader(csvFile.text))
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