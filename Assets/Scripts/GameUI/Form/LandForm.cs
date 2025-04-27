using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Form
{
    public class LandForm:BaseForm
    {
        [SerializeField]
        private DynamicButton _landButtonPrefab;
        [SerializeField]
        private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField]
        private GrowForm _growForm;
        [SerializeField]
        private RectTransform _rectTransformContent;

        private List<DynamicButton> _lands = new List<DynamicButton>();
        
        private string _landClickId ="";

        public void Start()
        {
            _growForm.gameObject.SetActive(false);
            UpdateListLand();
        }

        private void UpdateListLand()
        {
            var landLogics = GameManager.Instance.LandManager.Lands;
            for (int i = 0; i < landLogics.Count; i++)
            {
                if (_lands.Count <= i)
                {
                    var land = GameObject.Instantiate(_landButtonPrefab, _verticalLayoutGroup.transform);
                    land.Id = landLogics[i].Id;
                    land.Text = GetText(landLogics[i]);
                    land.AddListener(OnLandClick);
                    _lands.Add(land);
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformContent);
            Vector2 size = _rectTransformContent.sizeDelta;
            size.y = _verticalLayoutGroup.preferredHeight;
            _rectTransformContent.sizeDelta = size;
            
            for (int i = 0; i < _lands.Count; i++)
            {
                _lands[i].Text = GetText(landLogics[i]);
            }
        }

        private string GetText(Land landLogic)
        {
            string name = "<color=#00FF00>"+landLogic.GetNameHarvestable()+"</color>";
            float workerActionTime = GameManager.Instance.Configs.GlobalConfig.GetFloat("Worker_ActionTimeSeconds");
            switch ((Land.LandStatus)landLogic.Status)
            {
                case Land.LandStatus.Empty:
                    return "Trống";
                case Land.LandStatus.Growing:
                    return name + " - Đang trồng trong " + GetRemainingTime(landLogic.GrowTime, workerActionTime);
                case Land.LandStatus.Harvesting:
                    return name + " - Đang thu hoạch trong " + GetRemainingTime(landLogic.HarvestTime, workerActionTime);
                case Land.LandStatus.Using:
                    var havestable = GameManager.Instance.HarvestableManager.GetHarvestable(landLogic.HarvestableId);
                    string text = name + " - Đang có: " + "<color=#00FF00>" + havestable.PendingProducts + "</color>";
                    if (havestable.ProducedCount < havestable.MaxYield)
                    {
                        text += " - Thu hoạch tới: " +
                                GetRemainingTime(havestable.LastProduceTime, havestable.YieldTime);
                    }
                    else
                    {
                        text += " - Phân hủy trong: " +
                                GetRemainingTime(havestable.LastProduceTime, havestable.HarvestWindowDuration);
                    }
                    return text;
                default: 
                    return "Trống";
            }
        }

        public string GetRemainingTime(DateTime startTime, float durationInSeconds)
        {
            DateTime endTime = startTime.AddSeconds(durationInSeconds);
            TimeSpan remaining = endTime - DateTime.Now;
            string time;
            
            if (remaining.TotalSeconds < 0)
            {
                time =  "00:00";
            }
            
            time = string.Format("{0:D2}:{1:D2}", (int)remaining.TotalMinutes, remaining.Seconds);
            time = "<color=#00FF00>" + time + "</color>";
            return time;
        }

        private void OnLandClick(string Id)
        {
            var landLogic = GameManager.Instance.LandManager.GetLand(Id);
            switch ((Land.LandStatus)landLogic.Status)
            {
                case Land.LandStatus.Empty:
                    _landClickId = Id;
                    _growForm.gameObject.SetActive(true);
                    break;
                case Land.LandStatus.Using:
                    landLogic.Harvest();
                    UpdateUI();
                    break;
            }
        }

        public void Grow(int type)
        {
            _growForm.gameObject.SetActive(false);
            if (type < 0)
            {
                _landClickId = "";
                return;
            }
            
            GameManager.Instance.LandManager.Grow(type, _landClickId);
            _landClickId = "";
            UpdateUI();
        }
        
        public override void UpdateUI()
        {
            UpdateListLand();
        }
    }
}