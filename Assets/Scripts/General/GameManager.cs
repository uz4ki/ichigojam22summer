using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [field: SerializeField] public int Score { get; private set; }
        [field: SerializeField] public bool IsPlayingGame { get; private set; }
        
        [SerializeField] private GameTimer gameTimer;
        
        public void GameStart()
        {
            IsPlayingGame = true;
            gameTimer.gameObject.SetActive(true);
            
            MiniGameLoader.Instance.Initialization();
        }

        public void LevelOver(bool isPassed)
        {
            if (!isPassed) Score++;
            MiniGameLoader.Instance.MoveToNextLevel();
        }

        public void GameOver()
        {
            IsPlayingGame = false;
            gameTimer.gameObject.SetActive(false);
        }
    }
}
