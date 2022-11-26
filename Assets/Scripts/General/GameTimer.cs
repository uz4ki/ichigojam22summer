using System;
using UI;
using UnityEngine;

namespace General
{
    public class GameTimer : SingletonMonoBehaviour<GameTimer>
    {
        [SerializeField] private ViewTimer viewTimer;
        [SerializeField] private float startTime;
        public float Timer { get; private set; }

        private int _prevSeconds;
        private void OnEnable()
        {
            Timer = startTime;
            _prevSeconds = (int) startTime;
            viewTimer.UpdateViewTimer();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlayingGame) return;

            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
                Timer = 0;
                GameManager.Instance.GameOver();
            }

            if (_prevSeconds != (int) Timer)
            {
                _prevSeconds = (int) Timer;
                viewTimer.UpdateViewTimer();
                if (Timer < 11f)
                {
                    viewTimer.Bump();
                }
            }
        }

        public void AddTime(float time)
        {
            Timer += time;
        }
    }
}
