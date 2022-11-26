using System;
using System.Collections;
using System.Collections.Generic;
using Result;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace General
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [field: SerializeField] public int Score { get; private set; }

        private int _totalHints;
        private int _acquiredHints;
        public float SpinPercent { get; private set; }
        public bool IsPlayingGame { get; private set; }
        public bool isCorrectingGyro;
        
        public UnityEvent onChangeLevel;

        protected override void Awake()
        {
            CheckInstance();
            DontDestroyOnLoad(gameObject);
        }


        public void StartGame()
        {
            Score = 0;
            _totalHints = 0;
            _acquiredHints = 0;
            StartCoroutine(GameStartCoroutine());
        }

        public IEnumerator GameStartCoroutine()
        {
            while (SceneManager.GetActiveScene().name != "MainGame")
            {
                Debug.Log("Hoge1");
                yield return null;
            }

            yield return ViewGameEffect.Instance.StartCountDownCoroutine();
            
            IsPlayingGame = true;
            MiniGameLoader.Instance.Initialization();
            onChangeLevel.Invoke();
            yield return null;
        }

        public void LevelOver(bool isPassed)
        {
            if (!isPassed)
            {
                Score++;
                _totalHints += MiniGameLoader.Instance.LoadedGames.Peek().NumberOfHints;
                _acquiredHints += MiniGameLoader.Instance.HintCounts;
            }
            MiniGameLoader.Instance.MoveToNextLevel();
            onChangeLevel.Invoke();
        }

        public void GameOver()
        {
            IsPlayingGame = false;
            if (_totalHints == 0) SpinPercent = 0.5f;
            else
            {
                SpinPercent = (_acquiredHints * 1f) /  (_totalHints * 1f);
            }

            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("Result");
            asyncLoad.allowSceneActivation = false;
            
            yield return ViewGameEffect.Instance.GameOverCoroutine();

            asyncLoad.allowSceneActivation = true;
            yield return asyncLoad;
            
            ResultManager.Instance.StartResult();
        }
        
    }
}
