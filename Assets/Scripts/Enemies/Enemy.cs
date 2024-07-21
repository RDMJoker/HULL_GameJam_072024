using System;
using UnityEngine;

namespace DefaultNamespace.Enemies
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float maxHP;
        [SerializeField] int goldAmount;
        public float currentHP;
        public float MaxHP => maxHP;
        public EEnemyType EnemyType;

        Vector2 nextPosition;
        Vector2 correctedNextPosition
        {
            get
            {
                float correctedX = Mathf.FloorToInt(nextPosition.x) + 0.5f;
                float correctedY = Mathf.FloorToInt(nextPosition.y) + 0.5f;
                return new Vector3(correctedX, correctedY);
            }
        }
        Vector3 direction;

        public int TilesPassed;

        public Action<GameObject> OnDeath;

        void Start()
        {
            (nextPosition, direction) = GetNextPosition();
        }

        void FixedUpdate()
        {
            GridManager.Instance.objectGrid.GetXY(nextPosition, out int x, out int y);
            Vector2 nextPos = new Vector2(x, y);
            if ((correctedNextPosition - (Vector2)transform.position).magnitude < 0.05f)
            {
                (nextPosition, direction) = GetNextPosition();
                TilesPassed++;
            }
            Move();
            transform.up = direction;
        }

        void Move()
        {
            transform.position += direction * speed;
            float correctedY = transform.position.y;
            float correctedX = transform.position.x;
            if (direction == Vector3.left || direction == Vector3.right)
            {
                correctedY = Mathf.FloorToInt(transform.position.y) + 0.5f;
            }
            else
            {
                correctedX = Mathf.FloorToInt(transform.position.x) + 0.5f;
            }
            transform.position = new Vector3(correctedX, correctedY, transform.position.z);
        }

        public void TakeDamage(float _damageValue)
        {
            currentHP = Mathf.Max(currentHP - _damageValue, 0);
            if (currentHP != 0) return;
            ScoreManager.Instance.IncreaseCurrencyScore(goldAmount);
            if (gameObject == null) return;
            if (gameObject != null)OnDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }


        (Vector2, Vector3) GetNextPosition()
        {
            var currentPos = transform.position;
            var directions = new[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left, };
            foreach (var localDirection in directions)
            {
                if (GridManager.Instance.objectGrid.GetValue(currentPos + localDirection) == ETileState.Path)
                {
                    if (localDirection == Vector3.right || localDirection == Vector3.left)
                    {
                        return (currentPos + localDirection, localDirection);
                    }

                    return ((currentPos + localDirection), localDirection);
                }

                Debug.Log("Nothing found!");
            }

            throw new NotImplementedException();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(nextPosition, 0.25f);
        }
    }
}