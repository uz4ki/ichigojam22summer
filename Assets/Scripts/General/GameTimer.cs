using System;
using UnityEngine;

namespace General
{
    public class GameTimer : SingletonMonoBehaviour<GameTimer>
    {
        [SerializeField] private float startTime;
        public float Timer { get; private set; }

        private void OnEnable()
        {
            Timer = startTime;
        }

        private void Update()
        {
            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
                Timer = 0;
                GameManager.Instance.GameOver();
            }
        }

        public void AddTime(float time)
        {
            Timer += time;
        }
    }
}
