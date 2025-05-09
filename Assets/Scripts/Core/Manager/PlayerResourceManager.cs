﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Manager
{ 
     public class PlayerResourceManager
     {
        public static string FileName = "PlayerResourcesManager";
        [JsonProperty] private bool isNotFirstPlay;
         
        [JsonProperty] 
        public int Gold { get; private set; }

        [JsonProperty]
        private Dictionary<int, int> _seeds = new Dictionary<int, int>();

        [JsonProperty]
        private Dictionary<int, int> _products = new Dictionary<int, int>();

        [JsonProperty]
        public int TotalWorkers { get; private set; }
        
        [JsonProperty]
        public int IdleWorkers { get; private set; }
        
        [JsonProperty]
        public int EquipmentLevel { get; private set; } = 1;
        
        [JsonProperty]
        public int Goal_Gold { get; private set; }
        
        public event Action EndGameEvent;

        public PlayerResourceManager()
        {
        }
        
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                Save();
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            Save();
            if (Gold >= Goal_Gold)
            {
                EndGame();
            }
        }
        
        public void AddSeed(int seedAnimalId, int amount)
        {
            if (_seeds.ContainsKey(seedAnimalId))
                _seeds[seedAnimalId] += amount;
            else
                _seeds[seedAnimalId] = amount;
            Save();
        }

        public bool UseSeed(int seedAnimalId, int amount)
        {
            if (_seeds.TryGetValue(seedAnimalId, out int count) && count >= amount)
            {
                _seeds[seedAnimalId] -= amount;
                Save();
                return true;
            }
            return false;
        }

        public int GetSeedAmount(int seedAnimalId)
        {
            return _seeds.TryGetValue(seedAnimalId, out int count) ? count : 0;
        }


        public Dictionary<int, int> GetSeeds()
        {
            return _seeds;
        }
        
        public void AddProduct(int productId, int amount)
        {
            if (_products.ContainsKey(productId))
                _products[productId] += amount;
            else
                _products[productId] = amount;
            Save();
        }

        public bool UseProduct(int productId, int amount)
        {
            if (_products.TryGetValue(productId, out int count) && count >= amount)
            {
                _products[productId] -= amount;
                Save();
                return true;
            }
            return false;
        }

        public int GetProductAmount(int productId)
        {
            return _products.TryGetValue(productId, out int count) ? count : 0;
        }

        public void SellProduct(int productId, int amount)
        {
            if (UseProduct(productId, amount))
            {
                AddGold(GameManager.Instance.Configs.ProductConfig.FindById(productId).SellPrice * amount);
            }
        }

        public Dictionary<int, int> GetProducts()
        {
            return _products;
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
            Save();
            return true;
        }

        public bool AssignWorker()
        {
            if (IdleWorkers > 0)
            {
                IdleWorkers--;
                Save();
                return true;
            }
            return false;
        }

        public void FreeWorker()
        {
            IdleWorkers++;
            Save();
        }
        
        public bool UpgradeEquipment()
        {
            var price = GameManager.Instance.Configs.GlobalConfig.GetInt("Equipment_UpgradePrice");
            if (Gold < price)
            {
                return false;
            }
            SpendGold(price);
            EquipmentLevel++;
            Save();
            return true;
        }
        
        public void LoadConfig()
        {
            if (isNotFirstPlay) return;
            var globalConfig = GameManager.Instance.Configs.GlobalConfig;
            isNotFirstPlay = true;
            Gold = 0;
            Goal_Gold = globalConfig.GetInt("Game_Goal_Gold");
            TotalWorkers = globalConfig.GetInt("Worker_First");
            IdleWorkers = TotalWorkers;
            EquipmentLevel = globalConfig.GetInt("EquipmentLevel_First");
            int landFirst = globalConfig.GetInt("Land_First");
            for (int i = 0; i < landFirst; i++)
            {
                GameManager.Instance.LandManager.ExpandLand();
            }
            
            string seedAnimalFirst = globalConfig.GetString("SeedAnimal_First");
            var seedAnimals = seedAnimalFirst.Split('|');
            for (int i = 0; i < seedAnimals.Length; i++)
            {
                string seedAnimal = seedAnimals[i];
                var nums = seedAnimal.Split('=');
                AddSeed(int.Parse(nums[0]), int.Parse(nums[1]));
            }
            Save();
        }

        private void EndGame()
        {
            EndGameEvent?.Invoke();
        }

        public void Save()
        {
            SaveLoadManager.Save<PlayerResourceManager>(this, FileName);
        }
    }
}