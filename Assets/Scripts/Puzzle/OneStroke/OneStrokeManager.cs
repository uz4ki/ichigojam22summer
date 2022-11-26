using System;
using System.Collections.Generic;
using General;
using UnityEditor;
using UnityEngine;

namespace Puzzle.OneStroke
{
    public class OneStrokeManager : MiniGameManager
    {
        [field: SerializeField] public OneStrokePuzzleInfo PuzzleInfo { get; private set; }

        [SerializeField] private TileManager tileManager;

        
        protected override void Initialization()
        {
            var randomInt = UnityEngine.Random.Range(1, 10);
            PuzzleInfo = Resources.Load<OneStrokePuzzleInfo>($"OneStroke/{randomInt}");
            NumberOfHints = PuzzleInfo.Hints.Count;

            tileManager.Initialization(PuzzleInfo);
        }

        public override void SendHint(int hintCount)
        {
            tileManager.ApplyHintMoves(hintCount);
        }
        
    }
}
