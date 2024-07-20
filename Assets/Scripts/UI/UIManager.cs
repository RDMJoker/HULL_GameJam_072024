using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace DefaultNamespace.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI currencyValue;
        [SerializeField] GameObject buildUI;
        [SerializeField] BuildingUI buildingUI;
        public static UIManager Instance;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void UpdateCurrencyValue()
        {
            currencyValue.text = ScoreManager.Instance.Currency.ToString();
        }

        public void ShowBuildUI()
        {
            buildingUI.gameObject.SetActive(false);
            buildUI.SetActive(true);
        }
        
        public void ShowBuildingUI()
        {
            buildUI.SetActive(false);
            buildingUI.gameObject.SetActive(true);
        }

        public void UpdateBuildingUI()
        {
            buildingUI.UpdateBuildingUI();
        }
    }
}