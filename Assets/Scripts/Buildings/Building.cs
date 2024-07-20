using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Buildings
{
    public class Building : MonoBehaviour, IPointerClickHandler
    {

        public static Action<Building> OnSelect;
        public void OnPointerClick(PointerEventData _eventData)
        {
            OnSelect.Invoke(this);
        }
    }
}