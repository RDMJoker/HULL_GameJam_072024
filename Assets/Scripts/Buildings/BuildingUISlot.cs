using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.Buildings
{
    public class BuildingUISlot : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] public Building buildingPrefab;
        [SerializeField] Image border;

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
    }
}