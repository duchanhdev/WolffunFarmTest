using Core.Manager;
using GameUI.Form;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Manager
{
    public class GameUIManager:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI GoldText;
        [SerializeField] private float _timeToUpdate = 1;
        [SerializeField] Button[] _buttons;
        [SerializeField] BaseForm[] _forms;
        [SerializeField] private GameObject _endGameForm;
        private float _timeCounter = 0;
        public GameManager GameManager { get; private set; }

        private void Awake()
        {
            GameManager = GameManager.Instance;
        }

        void Start()
        {
            Init();
        }

        private void Init()
        {
            _timeCounter = 0;
            GameManager.Instance.PlayerResources.EndGameEvent += EndGame;
            TaskOnClick(_buttons[0]);
            UpdateUI();
        }

        private void EndGame()
        {
            _endGameForm.SetActive(true);
            GameManager.Instance.PlayerResources.EndGameEvent -= EndGame;
            GameManager.Instance.ResetGame();
            Init();
        }
        
        void Update()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= _timeToUpdate)
            {
                _timeCounter -= _timeToUpdate;
                GameManager.Instance.Update();
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