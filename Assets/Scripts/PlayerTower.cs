using System;
using System.Collections;
using DefaultNamespace.Enemies;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerTower : MonoBehaviour
    {
        [SerializeField] Volume volume;
        bool isFlashing;
        void OnTriggerEnter2D(Collider2D _collider)
        {
            if (!_collider.gameObject.TryGetComponent(out Enemy enemy)) return;
            ScoreManager.Instance.DecreaseLife(enemy.EnemyType == EEnemyType.Boss ? 10 : 1);
            StartCoroutine(FlashScreen(0.15f));
            enemy.OnDeath.Invoke(enemy.gameObject);
            Destroy(enemy.gameObject);
        }

        IEnumerator FlashScreen(float _duration)
        {
            if (isFlashing) yield break;
            volume.gameObject.SetActive(true);
            yield return new WaitForSeconds(_duration);
            volume.gameObject.SetActive(false);
        }
    }
}