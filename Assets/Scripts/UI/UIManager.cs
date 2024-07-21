using System;
using System.Collections;
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
        [SerializeField] TextMeshProUGUI waveScore;
        [SerializeField] TextMeshProUGUI lifeValue;
        [SerializeField] TextMeshProUGUI messageField;
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

        public void UpdateWaveScore()
        {
            waveScore.text = ScoreManager.Instance.Wave.ToString();
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

        public void UpdateLifeUI()
        {
            lifeValue.text = ScoreManager.Instance.Life.ToString();
        }

        public void UpdateBuildingUI()
        {
            buildingUI.UpdateBuildingUI();
        }

        public void PrintDisplayMessage(string _message, float _displayDuration)
        {
            messageField.text = _message;
            StartCoroutine(ShowMessage(_displayDuration));
        }

        IEnumerator ShowMessage(float _displayDuration)
        {
            messageField.gameObject.SetActive(true);
            yield return new WaitForSeconds(_displayDuration);
            messageField.gameObject.SetActive(false);
        }
    }
}