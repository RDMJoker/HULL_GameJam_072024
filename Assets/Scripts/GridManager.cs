using System;
using LL_Unity_Utils.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;
        [SerializeField] int gridWidth;
        [SerializeField] int gridHeight;
        public ObjectGrid<ETileState> objectGrid;

        public static GridManager Instance;

        void OnEnable()
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

        void Awake()
        {
            objectGrid = new ObjectGrid<ETileState>(gridWidth, gridHeight, 1, Vector3.zero);
            for (int y = 0; y < objectGrid.Height; y++)
            {
                for (int x = 0; x < objectGrid.Width; x++)
                {
                    var vectorPos = objectGrid.GetWorldPosition(x, y);
                    var vectorIntPos = new Vector3Int((int)vectorPos.x, (int)vectorPos.y, (int)vectorPos.z);
                    if (tilemap.GetTile(vectorIntPos) == null) continue;
                    string tileName = tilemap.GetTile(vectorIntPos).name[..3];
                    switch (tileName)
                    {
                        case "PAT":
                            objectGrid.SetValue(vectorIntPos, ETileState.Path);
                            break;
                        case "GRO":
                            objectGrid.SetValue(vectorIntPos, ETileState.Free);
                            break;
                    }
                }
            }

            int debugPathCount = 0;
            int debugFreeCount = 0;
            for (int y = 0; y < objectGrid.Height; y++)
            {
                for (int x = 0; x < objectGrid.Width; x++)
                {
                    Debug.Log("X: " + x + "| Y: " + y + "| Type: " + objectGrid.GetValue(x, y));
                    if (objectGrid.GetValue(x, y) == ETileState.Path) debugPathCount++;
                    if (objectGrid.GetValue(x, y) == ETileState.Free) debugFreeCount++;
                }
            }
            // Debug.Log("Paths: " + debugPathCount);
            // Debug.Log("Free: " + debugFreeCount);
        }
    }
}