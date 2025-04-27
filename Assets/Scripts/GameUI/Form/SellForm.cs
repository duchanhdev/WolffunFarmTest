using System.Collections.Generic;
using Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Form
{
    public class SellForm:BaseForm
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
            var table = GameManager.Instance.Configs.ProductConfig.Table;
            _buttons = new List<DynamicButton>();
            for (int i = 0; i < table.Count; i++)
            {
                var rowData = table[i];
                var button = GameObject.Instantiate(SellBuyButtonPrefab, _horizontalLayoutGroup.transform);
                button.Id = rowData.Id.ToString();
                button.Text = rowData.ProductName + " giá " + rowData.SellPrice + "V";
                button.AddListener(OnSellButtonClick);
                _buttons.Add(button);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformContent);
            Vector2 size = _rectTransformContent.sizeDelta;
            size.x = _horizontalLayoutGroup.preferredWidth;
            _rectTransformContent.sizeDelta = size;
        }

        private void OnSellButtonClick(string id)
        {
            GameManager.Instance.PlayerResources.SellProduct(int.Parse(id),1);   
        }

        public override void UpdateUI()
        {
            
        }
    }
}