using Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Form
{
    public class UpgradeForm:BaseForm
    {
        [SerializeField]
        private Button landBuyBtn;
        [SerializeField]
        private Button workerHireBtn;
        [SerializeField]
        private Button equipmentLevelBtn;
        
        public override void UpdateUI()
        {
            
        }

        public void EquipmentLevelOnClick()
        {
            GameManager.Instance.PlayerResources.UpgradeEquipment();
        }

        public void LandBuyOnClick()
        {
            GameManager.Instance.ExpandLand();
        }

        public void WorkerHireOnClick()
        {
            GameManager.Instance.PlayerResources.HireWorker();
        }
    }
}