using UnityEngine;

namespace DefaultNamespace
{
    public static class Extensions
    {
        public static bool VectorApproximately(Vector2 _left, Vector2 _right)
        {
            return Mathf.Approximately(_left.x, _right.x) && Mathf.Approximately(_left.y, _right.y);

        }
    }
}