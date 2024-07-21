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

        public int Life { get; private set; }

        public bool HasLost { get; private set; }

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

            Life = 10;
            UIManager.Instance.UpdateLifeUI();
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

        public void IncreaseLife()
        {
        }

        public void DecreaseLife(int _amount)
        {
            Life = Mathf.Max(Life - _amount, 0);
            UIManager.Instance.UpdateLifeUI();
            if (Life == 0) HasLost = true;
        }
    }
}