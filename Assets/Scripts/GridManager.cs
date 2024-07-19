using System;
using LL_Unity_Utils.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;
        ObjectGrid<ETileState> objectGrid;

        void Awake()
        {
            objectGrid = new ObjectGrid<ETileState>(5, 5, 1, Vector3.zero);
            for (int y = 0; y < objectGrid.Height; y++)
            {
                for (int x = 0; x < objectGrid.Width; x++)
                {
                   var vectorPos = objectGrid.GetWorldPosition(x, y);
                   var vectorIntPos = new Vector3Int((int)vectorPos.x, (int)vectorPos.y, (int)vectorPos.z);
                   if (tilemap.GetTile(vectorIntPos) == null) continue;
                   if (tilemap.GetTile(vectorIntPos).name == "whitePixel")
                   {
                       objectGrid.SetValue(vectorIntPos, ETileState.Occupied);
                   }
                }
            }
            for (int y = 0; y < objectGrid.Height; y++)
            {
                for (int x = 0; x < objectGrid.Width; x++)
                {
                    Debug.Log("X: " + x + "| Y: " + y + "| Type: " + objectGrid.GetValue(x,y));
                }
            }
        }
    }
}