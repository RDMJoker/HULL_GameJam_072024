using System;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        public int Currency { get; private set; }
        public int Wave { get; private set; }

        void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void SetWave(int _value)
        {
            Wave = _value;
            UIManager.Instance.UpdateWaveScore();
        }

        public void IncreaseCurrencyScore(int _amount)
        {
            Currency += _amount;
            UIManager.Instance.UpdateCurrencyValue();
        }

        public void DecreaseCurrencyScore(int _amount)
        {
            Currency -= _amount;
            UIManager.Instance.UpdateCurrencyValue();
        }
    }
}