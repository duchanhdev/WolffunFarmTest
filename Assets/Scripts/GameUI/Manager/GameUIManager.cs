using System.Threading;
using Core.Manager;
using GameUI.Form;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace GameUI.Manager
{
    public class GameUIManager:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI GoldText;
        [SerializeField] private float _timeToUpdate = 1;
        [SerializeField] Button[] _buttons;
        [SerializeField] BaseForm[] _forms;
        private float _timeCounter = 0;
        void Start()
        {
            _timeCounter = 0;
            TaskOnClick(_buttons[0]);
            UpdateUI();
        }

        void Update()
        {
            GameManager.Instance.Update(Time.deltaTime);
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= _timeToUpdate)
            {
                _timeCounter -= _timeToUpdate;
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            UpdateGoldText();
            foreach (var form in _forms)
            {
                if (form.gameObject.activeInHierarchy)
                {
                    form.UpdateUI();
                }
            }
        }

        void UpdateGoldText()
        {
            GoldText.text = "Vàng: " + GameManager.Instance.PlayerResources.Gold;
        }
        
        public void TaskOnClick(Button button)
        {
            foreach (var form in _forms) form.gameObject.SetActive(false);
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == button)
                {
                    _forms[i].gameObject.SetActive(true);
                    _forms[i].UpdateUI();
                }
            }
        }
    }
}