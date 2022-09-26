using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace Puzzle.OneStroke
{
    public class OneStrokeManager : MiniGameManager
    {
        public OneStrokePuzzleInfo PuzzleInfo { get; private set; }

        [SerializeField] private TileManager _tileManager;
        protected override void Initialization()
        {
            PuzzleInfo = Resources.Load<OneStrokePuzzleInfo>("OneStroke/Test");
            NumberOfHints = PuzzleInfo.Hints.Count;

            _tileManager.Initialization(PuzzleInfo);
        }

        public override void LevelClear()
        {
            GameManager.Instance.LevelOver(true);
            gameObject.SetActive(false);
        }

        public override void SendHint(int hintCount)
        {
            _tileManager.ApplyHintMoves(hintCount);
        }
    }
}
