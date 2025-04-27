using Core.Manager;
using TMPro;
using UnityEngine;

namespace GameUI.Form
{
    public class ResourceForm:BaseForm
    {
        [SerializeField] private TextMeshProUGUI _text;
        public override void UpdateUI()
        {
            var playerResources = GameManager.Instance.PlayerResources;
            string text = "";
            text += "Vàng: " + "<color=#00FF00>" + playerResources.Gold + "</color>" + "\n";
            
            var seeds = playerResources.GetSeeds();
            var seedConfig = GameManager.Instance.Configs.SeedAnimalConfig;
            text += "Giống: ";
            foreach (var seed in seeds)
            {
                text += seedConfig.FindById(seed.Key).Name+" ";
                text += "<color=#00FF00>" + seed.Value + "</color>" + ", ";
            }
            text += "\n";
            
            var products = playerResources.GetProducts();
            var productConfig = GameManager.Instance.Configs.ProductConfig;
            text += "Sản phẩm: ";
            foreach (var product in products)
            {
                text += productConfig.FindById(product.Key).ProductName+" ";
                text += "<color=#00FF00>" + product.Value + "</color>" + ", ";
            }
            text += "\n";

            text += "Công nhân: ";
            text += "đang làm " + "<color=#00FF00>" + (playerResources.TotalWorkers - playerResources.IdleWorkers) +
                    "</color>" + ", ";
            text += "đang nghỉ " + "<color=#00FF00>" + playerResources.IdleWorkers + "</color>" + "\n";

            text += "Đất: ";
            var emptyLand = GameManager.Instance.LandManager.GetLandEmptyCount();
            var usingLand = GameManager.Instance.LandManager.TotalLand - emptyLand;
            text += "trống " + "<color=#00FF00>" + emptyLand + "</color>" + ", đang sử dụng " + "<color=#00FF00>" +
                    usingLand + "</color>" + "\n";

            text += "Level thiết bị nông trại: " + "<color=#00FF00>" + playerResources.EquipmentLevel + "</color>";
            
            _text.text = text;
        }
    }
}