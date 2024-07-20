using System;
using DefaultNamespace.Buildings;
using UnityEngine;

namespace DefaultNamespace
{
    public class BuildingManager : MonoBehaviour
    {
        Building selectedBuilding;

        public static Action<Building> OnPlacement;
        
        void OnEnable()
        {
            Building.OnSelect += SetSelection;
            ClickManager.OnLeftClick += CheckSelection;
        }

        void SetSelection(Building _building)
        {
            selectedBuilding = _building;
        }

        void CheckSelection(Vector3 _mousePosition)
        {
            if (selectedBuilding == null) return;
            if (GridManager.Instance.objectGrid.GetValue(_mousePosition) != ETileState.Free) return;
            GridManager.Instance.objectGrid.SetValue(_mousePosition,ETileState.Occupied);
        }

        void EndSelection(Vector3 _mousePosition)
        {
            
        }
    }
}