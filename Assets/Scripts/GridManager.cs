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
        ObjectGrid<ETileState> objectGrid;

        void OnEnable()
        {
            ClickManager.OnLeftClick += 
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
                   if (tilemap.GetTile(vectorIntPos).name == "whitePixel")
                   {
                       objectGrid.SetValue(vectorIntPos, ETileState.Path);
                   }
                   else if (tilemap.GetTile(vectorIntPos).name == "blackPixel")
                   {
                       objectGrid.SetValue(vectorIntPos,ETileState.Free);
                   }
                }
            }

            int debugPathCount = 0;
            int debugFreeCount = 0;
            for (int y = 0; y < objectGrid.Height; y++)
            {
                for (int x = 0; x < objectGrid.Width; x++)
                {
                    Debug.Log("X: " + x + "| Y: " + y + "| Type: " + objectGrid.GetValue(x,y));
                    if (objectGrid.GetValue(x, y) == ETileState.Path) debugPathCount++;
                    if (objectGrid.GetValue(x,y) == ETileState.Free) debugFreeCount++;
                }
            }
            Debug.Log("Paths: " + debugPathCount);
            Debug.Log("Free: " + debugFreeCount);
        }
    }
}