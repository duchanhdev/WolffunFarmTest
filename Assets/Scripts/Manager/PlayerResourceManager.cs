using System.Collections.Generic;
using Core.Entities;

namespace Data.Configs
{
     public class PlayerResourceManager
    {
        public int Gold { get; private set; }

        private Dictionary<int, int> _seeds = new Dictionary<int, int>();

        private Dictionary<int, int> _products = new Dictionary<int, int>();

        public int TotalWorkers { get; private set; }
        public int IdleWorkers { get; private set; }

        public int EquipmentLevel { get; private set; } = 1;

        public PlayerResourceManager()
        {
            Gold = 0;
            TotalWorkers = 1;
            IdleWorkers = 1;

            AddSeed(1, 10); // Cà chua
            AddSeed(2, 10); // Việt quất
            AddSeed(4, 2);  // Bò giống
        }
        
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }
        
        public void AddSeed(int seedAnimalId, int amount)
        {
            if (_seeds.ContainsKey(seedAnimalId))
                _seeds[seedAnimalId] += amount;
            else
                _seeds[seedAnimalId] = amount;
        }

        public bool UseSeed(int seedAnimalId, int amount)
        {
            if (_seeds.TryGetValue(seedAnimalId, out int count) && count >= amount)
            {
                _seeds[seedAnimalId] -= amount;
                return true;
            }
            return false;
        }

        public int GetSeedAmount(int seedAnimalId)
        {
            return _seeds.TryGetValue(seedAnimalId, out int count) ? count : 0;
        }
        
        public void AddProduct(int productId, int amount)
        {
            if (_products.ContainsKey(productId))
                _products[productId] += amount;
            else
                _products[productId] = amount;
        }

        public bool UseProduct(int productId, int amount)
        {
            if (_products.TryGetValue(productId, out int count) && count >= amount)
            {
                _products[productId] -= amount;
                return true;
            }
            return false;
        }

        public int GetProductAmount(int productId)
        {
            return _products.TryGetValue(productId, out int count) ? count : 0;
        }
        
        public bool HireWorker()
        {
            var price = GameManager.Instance.Configs.GlobalConfig.GetInt("Worker_HirePrice");
            if (Gold < price)
            {
                return false;
            }
            SpendGold(price);
            TotalWorkers++;
            IdleWorkers++;
            return true;
        }

        public bool AssignWorker()
        {
            if (IdleWorkers > 0)
            {
                IdleWorkers--;
                return true;
            }
            return false;
        }

        public void FreeWorker()
        {
            IdleWorkers++;
        }
        
        public void UpgradeEquipment()
        {
            EquipmentLevel++;
        }

        public void SaveGameMeta()
        {
            
        }
    }
}