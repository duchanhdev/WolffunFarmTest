using System.Collections.Generic;
using Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Form
{
    public class GrowForm:MonoBehaviour
    {
        [SerializeField]
        private DynamicButton SellBuyButtonPrefab;
        [SerializeField]
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField]
        private LandForm _landForm;
        [SerializeField]
        private DynamicButton _cancelButton;
        [SerializeField]
        private RectTransform _rectTransformContent;

        private List<DynamicButton> _buttons;

        public void Start()
        {
            var table = GameManager.Instance.Configs.SeedAnimalConfig.Table;
            _cancelButton.Id = "-1";
            _cancelButton.AddListener(OnGrowButtonClick);
            _buttons = new List<DynamicButton>();
            for (int i = 0; i < table.Count; i++)
            {
                var rowData = table[i];
                var button = GameObject.Instantiate(SellBuyButtonPrefab, _horizontalLayoutGroup.transform);
                button.Id = rowData.Id.ToString();
                button.Text = rowData.Name;
                button.AddListener(OnGrowButtonClick);
                _buttons.Add(button);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformContent);
            Vector2 size = _rectTransformContent.sizeDelta;
            size.x = _horizontalLayoutGroup.preferredWidth;
            _rectTransformContent.sizeDelta = size;
        }

        private void OnGrowButtonClick(string id)
        {
              _landForm.Grow(int.Parse(id));
        }
    }
}