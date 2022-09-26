using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class MiniGameLoader : SingletonMonoBehaviour<MiniGameLoader>
    {
        [SerializeField] private List<MiniGameManager> gameList;
        [field: SerializeField] public Queue<MiniGameManager> LoadedGames { get; private set; }

        private int _hintCounts;
        
        public void Initialization()
        {
            if (gameList[0] == null) throw new ArgumentNullException("ゲームリストがnullです");

            LoadedGames = new Queue<MiniGameManager>();

            var random = new System.Random();
            EnqueueMiniGame(gameList[random.Next(0,gameList.Count)]);
            EnqueueMiniGame(gameList[random.Next(0,gameList.Count)]);
            EnqueueMiniGame(gameList[random.Next(0,gameList.Count)]);
            
            _hintCounts = 0;
            
            LoadedGames.Peek().gameObject.SetActive(true);
        }

        public void MoveToNextLevel()
        {
            _hintCounts = 0;
            var random = new System.Random();
            DequeueMiniGame();
            EnqueueMiniGame(gameList[random.Next(0,gameList.Count)]);
            LoadedGames.Peek().gameObject.SetActive(true);
        }

        public void GetHints()
        {
            if (LoadedGames.Peek().NumberOfHints <= _hintCounts) return;
            _hintCounts++;
            LoadedGames.Peek().SendHint(_hintCounts);
        }
        
        private void EnqueueMiniGame(MiniGameManager game)
        {
            var gameInstance = Instantiate(game.gameObject, transform);
            LoadedGames.Enqueue(gameInstance.GetComponent<MiniGameManager>());
            gameInstance.SetActive(false);
        }
        
        private void DequeueMiniGame()
        {
            Destroy(LoadedGames.Peek().gameObject);
            LoadedGames.Dequeue();
        }
    }
}
