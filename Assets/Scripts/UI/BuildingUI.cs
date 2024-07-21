using System;
using DefaultNamespace.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] Button upgradeButton;
        [SerializeField] TextMeshProUGUI upgradeButtonText;
        [SerializeField] TextMeshProUGUI targetButton;
        [SerializeField] TextMeshProUGUI attackSpeed;
        [SerializeField] TextMeshProUGUI attackDamage;
        [SerializeField] TextMeshProUGUI attackRange;
        [SerializeField] TextMeshProUGUI upgradeLevel;

        bool isMaxLevel;

        Building building;

        int upgradeCost => building.CurrencyCost * building.UpgradeLevel;

        void OnEnable()
        {
            SetSelectedBuilding();
            UpdateBuildingUI();
        }

        void SetSelectedBuilding()
        {
            building = BuildingManager.Instance.selectedBuilding;
        }

        public void UpdateBuildingUI()
        {
            SetSelectedBuilding();
            attackSpeed.text = building.AttackSpeed.ToString();
            attackDamage.text = building.AttackDamage.ToString();
            attackRange.text = building.AttackRange.ToString();
            upgradeLevel.text = building.UpgradeLevel.ToString();
            upgradeButton.enabled = true;
            if (building.UpgradeLevel >= building.MaxUpgradeLevel) upgradeButton.enabled = false;
            if (building.UpgradeLevel == building.MaxUpgradeLevel) upgradeButtonText.text = "Max Level";
            else upgradeButtonText.text = "Upgrade(" + upgradeCost + ")";
        }

        public void DoBuildingUpgrade()
        {
            if (ScoreManager.Instance.Currency < upgradeCost)
            {
                UIManager.Instance.PrintDisplayMessage("Not enough currency!", 2);
            }
            else
            {
                ScoreManager.Instance.DecreaseCurrencyScore(upgradeCost);
                building.Upgrade();
                UpdateBuildingUI();
            }
        }

        public void SwapTargetOption()
        {
            var currentOption = building.targetOption;
            if (currentOption == ETargetOption.Strongest) currentOption = ETargetOption.First;
            else currentOption++;
            building.targetOption = currentOption;
            targetButton.text = currentOption.ToString().Replace("ETargetOption.", "");
        }
    }
}