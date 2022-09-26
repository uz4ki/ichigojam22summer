using System;
using UnityEngine;

namespace General
{
    public class MiniGameManager : MonoBehaviour
    {
        public int NumberOfHints { get; protected set; }
        private void OnEnable()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            
        }

        public virtual void LevelClear()
        {
            
        }

        public virtual void SendHint(int hintCount)
        {
            
        }
    }
}
