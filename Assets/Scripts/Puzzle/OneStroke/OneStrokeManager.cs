using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace Puzzle.OneStroke
{
    public class OneStrokeManager : MiniGameManager
    {
        public OneStrokePuzzleInfo puzzleInfo;

        [SerializeField] private TileManager _tileManager;
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public List<Vector2> CheckPoints { get; private set; }
        public List<Vector2> AlreadyFilledTiles { get; private set; }

        protected override void Initialization()
        {
            puzzleInfo = Resources.Load<OneStrokePuzzleInfo>("OneStroke/Test");
            TileWidth = puzzleInfo.TileWidth;
            TileHeight = puzzleInfo.TileHeight;
            CheckPoints = puzzleInfo.CheckPoints;
            AlreadyFilledTiles = puzzleInfo.AlreadyFilledTiles;
            
            _tileManager.Initialization();
        }

        public void LevelClear()
        {
            GameManager.Instance.LevelOver(true);
            gameObject.SetActive(false);
        }
    }
}
