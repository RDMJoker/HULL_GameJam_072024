using System;
using DefaultNamespace.Enemies;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        void Awake()
        {
            ScoreManager.Instance.IncreaseCurrencyScore(5);
            SpawnEnemy(enemy.gameObject);
        }

        void SpawnEnemy(GameObject _enemy)
        {
            Instantiate(_enemy.gameObject, new Vector3(0, 0, 0) + new Vector3(1, 1) * 0.5f, Quaternion.identity);
        }
    }
}