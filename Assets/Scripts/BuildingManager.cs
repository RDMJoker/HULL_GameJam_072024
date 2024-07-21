using System;
using System.Collections;
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

        [SerializeField] GameObject previewPrefab;

        [SerializeField] AudioClip buildingSound;

        [SerializeField] Color illegalColor;
        [SerializeField] Color legalColor;
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
            previewPrefab = Instantiate(previewPrefab);
            previewPrefab.SetActive(false);
            StartCoroutine(DrawPreview());
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

        IEnumerator DrawPreview()
        {
            while (ScoreManager.Instance.HasLost == false)
            {
                while (selectedBuildingUISlot != null)
                {
                    previewPrefab.GetComponent<SpriteRenderer>().sprite = selectedBuildingUISlot.buildingPrefab.GetComponent<SpriteRenderer>().sprite;
                    var mousePosition = GetMousePosition();
                    previewPrefab.transform.position = GridManager.Instance.objectGrid.GetGridPositionFromMouse(mousePosition) + new Vector3(1, 1) * 0.5f;
                    previewPrefab.GetComponent<HoverMovement>().StartPosition = previewPrefab.transform.position;
                    if (GridManager.Instance.objectGrid.IsOutsideBounds(mousePosition)) previewPrefab.SetActive(false);
                    else previewPrefab.SetActive(true);
                    if (GridManager.Instance.objectGrid.GetValue(GridManager.Instance.objectGrid.GetGridPositionFromMouse(mousePosition) + new Vector3(1, 1) * 0.5f) != ETileState.Free)
                        previewPrefab.GetComponent<SpriteRenderer>().color = illegalColor;
                    else previewPrefab.GetComponent<SpriteRenderer>().color = legalColor;
                    yield return null;
                }
                if(previewPrefab.activeInHierarchy) previewPrefab.SetActive(false);
                yield return new WaitForSeconds(0.15f);
            }
        }

        void CheckSelection(Vector3 _mousePosition)
        {
            if (selectedBuildingUISlot == null) return;
            if (GridManager.Instance.objectGrid.GetValue(_mousePosition) != ETileState.Free) return;
            GridManager.Instance.objectGrid.SetValue(_mousePosition, ETileState.Occupied);
            Instantiate(selectedBuildingUISlot.buildingPrefab.gameObject, GridManager.Instance.objectGrid.GetGridPositionFromMouse(_mousePosition) + new Vector3(1, 1) * 0.5f, Quaternion.identity);
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
        
        public static Vector3 GetMousePosition()
        {
            Vector3 vector = GetMousePositionWithZ(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;

        }

        public static Vector3 GetMousePositionWithZ()
        {
            return GetMousePositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMousePositionWithZ(Camera _camera)
        {
            return GetMousePositionWithZ(Input.mousePosition, _camera);
        }

        public static Vector3 GetMousePositionWithZ(Vector3 _screenPos, Camera _camera)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(_screenPos);
            return worldPos;
        }
    }
}