using System;
using System.Collections;
using UI;
using UnityEngine;

namespace General
{
    public class MiniGameManager : MonoBehaviour
    {
        public int NumberOfHints { get; protected set; }
        [field: SerializeField] public string MissionMassage { get; protected set; } 
        private void OnEnable()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            
        }

        public virtual void LevelClear()
        {
            ViewGameEffect.Instance.Correct();
            GameManager.Instance.LevelOver(false);
        }

        public virtual void PassLevel()
        {
            ViewGameEffect.Instance.Pass();
            GameManager.Instance.LevelOver(true);
        }
        
        public virtual void LevelFailure()
        {
            ViewGameEffect.Instance.Failure();
            GameManager.Instance.LevelOver(true);
        }

        public virtual void SendHint(int hintCount)
        {
            
        }
    }
}
