﻿using System.Collections.Generic;
using Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Form
{
    public class StoreForm:BaseForm
    {
        [SerializeField]
        private DynamicButton SellBuyButtonPrefab;
        [SerializeField]
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField]
        private RectTransform _rectTransformContent;

        private List<DynamicButton> _buttons;

        public void Start()
        {
            var shopConfigTable = GameManager.Instance.Configs.ShopConfig.Table;
            var seedConfig = GameManager.Instance.Configs.SeedAnimalConfig;
            _buttons = new List<DynamicButton>();
            for (int i = 0; i < shopConfigTable.Count; i++)
            {
                var shopRowData = shopConfigTable[i];
                var seedRowData = seedConfig.FindById(shopRowData.SeedAnimalId);
                var button = GameObject.Instantiate(SellBuyButtonPrefab, _horizontalLayoutGroup.transform);
                button.Id = shopRowData.Id.ToString();
                button.Text = seedRowData.Name + " giá " + shopRowData.BuyPrice + "V / "+shopRowData.BuyUnit+" đơn vị";
                button.AddListener(OnBuyButtonClick);
                _buttons.Add(button);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformContent);
            Vector2 size = _rectTransformContent.sizeDelta;
            size.x = _horizontalLayoutGroup.preferredWidth;
            _rectTransformContent.sizeDelta = size;
        }

        private void OnBuyButtonClick(string id)
        {
            GameManager.Instance.ShopManager.BuySeedAnimal(int.Parse(id));
        }
        public override void UpdateUI()
        {
            
        }
    }
}