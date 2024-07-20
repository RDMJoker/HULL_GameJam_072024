using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ResolutionForcer : MonoBehaviour
    {
        void Awake()
        {
            Screen.SetResolution(1920,1080,FullScreenMode.FullScreenWindow);
        }
    }
}