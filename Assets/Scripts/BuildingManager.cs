﻿using System;
using DefaultNamespace.Buildings;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AudioSource))]
    public class BuildingManager : MonoBehaviour
    {
        public BuildingUISlot selectedBuildingUISlot;

        public Building selectedBuilding;

        public static Action<BuildingUISlot> OnPlacement;

        public static BuildingManager Instance;

        [SerializeField] AudioClip buildingSound;
        AudioSource source;
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

        void Start()
        {
            source = GetComponent<AudioSource>();
            source.clip = buildingSound;
        }

        void OnEnable()
        {
            BuildingUISlot.OnSelect += SetSelection;
            ClickManager.OnLeftClick += CheckSelection;
            ClickManager.OnRightClick += EndSelection;
        }

        void SetSelection(BuildingUISlot _buildingUISlot)
        {
            selectedBuildingUISlot = _buildingUISlot;
        }

        void CheckSelection(Vector3 _mousePosition)
        {
            if (selectedBuildingUISlot == null) return;
            if (GridManager.Instance.objectGrid.GetValue(_mousePosition) != ETileState.Free) return;
            GridManager.Instance.objectGrid.SetValue(_mousePosition,ETileState.Occupied);
            Instantiate(selectedBuildingUISlot.buildingPrefab.gameObject, GridManager.Instance.objectGrid.GetGridPositionFromMouse(_mousePosition) + new Vector3(1,1) * 0.5f, Quaternion.identity);
            source.Play();
            selectedBuildingUISlot.DisableSelection();
            selectedBuildingUISlot = null;
            if (selectedBuilding == null) return;
            selectedBuilding.SetHighlighter(false);
        }

        void EndSelection(Vector3 _mousePosition)
        {
            if (selectedBuilding != null)
            {
                UIManager.Instance.ShowBuildUI();
                selectedBuilding.SetHighlighter(false);
            }
            if (selectedBuildingUISlot == null) return;
            selectedBuildingUISlot.DisableByCancellation();
            selectedBuildingUISlot = null;
        }
    }
}