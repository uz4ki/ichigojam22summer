using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public class MiniGameLoader : SingletonMonoBehaviour<MiniGameLoader>
    {
        [SerializeField] private List<MiniGameManager> gameList;
        [field: SerializeField] public Queue<MiniGameManager> LoadedGames { get; private set; }
        
        public UnityEvent<int,bool> OnGetHint;

        public int HintCounts { get; private set; }
        
        public void Initialization()
        {
            if (gameList[0] == null) throw new ArgumentNullException("ゲームリストがnullです");

            LoadedGames = new Queue<MiniGameManager>();

            var random = UnityEngine.Random.Range(0, gameList.Count);
            EnqueueMiniGame(gameList[random]);
            random = UnityEngine.Random.Range(0, gameList.Count);
            EnqueueMiniGame(gameList[random]);
            random = UnityEngine.Random.Range(0, gameList.Count);
            EnqueueMiniGame(gameList[random]);
            
            HintCounts = 0;
            
            LoadedGames.Peek().gameObject.SetActive(true);
        }

        public void MoveToNextLevel()
        {
            HintCounts = 0;
            var random = new System.Random();
            DequeueMiniGame();
            EnqueueMiniGame(gameList[random.Next(0,gameList.Count)]);
            LoadedGames.Peek().gameObject.SetActive(true);
        }

        public void GetHints()
        {
            if (LoadedGames.Peek().NumberOfHints <= HintCounts) return;
            HintCounts++;
            
            if (LoadedGames.Peek().NumberOfHints == HintCounts)
            {
                OnGetHint.Invoke(HintCounts, true);
                StartCoroutine(JoyconGyro.Instance.RumbleOnGetMaxHint());
            }
            else
            {
                OnGetHint.Invoke(HintCounts, false);
                JoyconGyro.Instance.RumbleOnGetNormalHint();
            }

            LoadedGames.Peek().SendHint(HintCounts);
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
