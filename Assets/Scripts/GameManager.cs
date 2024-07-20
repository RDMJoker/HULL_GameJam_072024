using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Enemies;
using DefaultNamespace.UI;
using LL_Unity_Utils.Timers;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        [SerializeField] Enemy bossEnemy;
        [SerializeField] int enemyIncreasePerWave;
        [SerializeField] int maxDurationBetweenWaves;
        [SerializeField] int enemyStartAmount;

        int wave = 1;
        int enemyAmount;

        Timer waveTimer;

        readonly List<GameObject> aliveEnemies = new();
        bool spawnWaves;

        void RegisterEnemy(Enemy _enemy)
        {
            _enemy.OnDeath += OnRegisterEnemyDeath;
        }


        void Start()
        {
            enemyAmount = enemyStartAmount;
            ScoreManager.Instance.IncreaseCurrencyScore(5);
            ScoreManager.Instance.SetWave(wave);
            UIManager.Instance.UpdateWaveScore();
            spawnWaves = true;
            StartCoroutine(WaveSpawning());
        }

        IEnumerator WaveSpawning()
        {
            while (spawnWaves)
            {
                waveTimer = new Timer(maxDurationBetweenWaves);
                waveTimer.StartTimer();
                if (ScoreManager.Instance.Wave % 15 == 0)
                {
                    var spawnedEnemyObject = SpawnEnemy(enemy.gameObject);
                    aliveEnemies.Add(spawnedEnemyObject);
                    var spawnedEnemy = spawnedEnemyObject.GetComponent<Enemy>();
                    RegisterEnemy(spawnedEnemy);
                    yield return new WaitUntil((() => aliveEnemies.Count == 0));
                }
                else
                {
                    for (int i = 0; i < enemyAmount; i++)
                    {
                        var spawnedEnemyObject = SpawnEnemy(enemy.gameObject);
                        aliveEnemies.Add(spawnedEnemyObject);
                        var spawnedEnemy = spawnedEnemyObject.GetComponent<Enemy>();
                        RegisterEnemy(spawnedEnemy);
                        yield return new WaitForSeconds(0.75f);
                    }

                    enemyAmount = enemyStartAmount + (enemyIncreasePerWave * wave);
                    yield return new WaitUntil(() => aliveEnemies.Count == 0 || waveTimer.CheckTimer());
                }
                wave++;
                ScoreManager.Instance.SetWave(wave);
            }
        }

        void OnRegisterEnemyDeath(GameObject _enemy)
        {
            aliveEnemies.Remove(_enemy);
            var enemyComponent = _enemy.GetComponent<Enemy>();
            enemyComponent.OnDeath -= OnRegisterEnemyDeath;
        }

        void EndGame()
        {
            spawnWaves = false;
        }

        GameObject SpawnEnemy(GameObject _enemy)
        {
            return Instantiate(_enemy.gameObject, new Vector3(0, 0, 0) + new Vector3(1, 1) * 0.5f, Quaternion.identity);
        }
    }
}