using UnityEngine;

namespace DefaultNamespace
{
    public class HoverMovement : MonoBehaviour
    {

        [SerializeField] float amplitude;
        [SerializeField] float frequence;

        public Vector3 StartPosition; 
        private void OnEnable()
        {
            StartPosition = gameObject.transform.position;
        }

        private void Update()
        {
        
            transform.position = new Vector3(StartPosition.x,(Mathf.Sin(Time.time * frequence) * amplitude) + StartPosition.y);
        }
    }
}