using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class ClickManager : MonoBehaviour
    {
        Vector3 currentMousePositionWorld;

        public static Action<Vector3> OnLeftClick;
        public static Action<Vector3> OnRightClick;

        public void Point(InputAction.CallbackContext _context)
        {
            var value = _context.ReadValue<Vector2>();
            if (Camera.main != null)
            {
                currentMousePositionWorld = Camera.main.ScreenToWorldPoint(value);
                currentMousePositionWorld.z = 0f;
            }

        }

        public void LeftClick(InputAction.CallbackContext _context)
        {
                OnLeftClick.Invoke(currentMousePositionWorld);
            
        }

        public void RightClick(InputAction.CallbackContext _context)
        {
                OnRightClick.Invoke(currentMousePositionWorld);
        }
    }
}