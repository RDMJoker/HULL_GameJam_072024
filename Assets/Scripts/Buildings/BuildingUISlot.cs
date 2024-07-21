using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.Buildings
{
    public class BuildingUISlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] public Building buildingPrefab;
        [SerializeField] Image border;
        [SerializeField] TextMeshProUGUI towerTextLabel;
        [SerializeField] TextMeshProUGUI towerCostLabel;

        public int currencyCost;
        
        public static Action<BuildingUISlot> OnSelect;
        
        public void OnPointerClick(PointerEventData _eventData)
        {
            if (ScoreManager.Instance.Currency < currencyCost)
            {
                Debug.Log("Not enough Currency!");
                return;
            }
            ScoreManager.Instance.DecreaseCurrencyScore(currencyCost);
            border.gameObject.SetActive(true);
            OnSelect.Invoke(this);
        }

        public void DisableSelection()
        {
            border.gameObject.SetActive(false);
        }

        public void DisableByCancellation()
        {
            ScoreManager.Instance.IncreaseCurrencyScore(currencyCost);
            border.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            towerTextLabel.gameObject.SetActive(true);
            towerCostLabel.gameObject.SetActive(true);
            towerTextLabel.text = buildingPrefab.Name + " : ";
            towerCostLabel.text = currencyCost + " Currency";
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            towerTextLabel.gameObject.SetActive(false);
            towerCostLabel.gameObject.SetActive(false);
        }
    }
}