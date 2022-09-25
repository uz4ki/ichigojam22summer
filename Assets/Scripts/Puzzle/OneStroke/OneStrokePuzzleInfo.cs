using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.OneStroke
{
    
    [CreateAssetMenu]
    public class OneStrokePuzzleInfo : ScriptableObject
    {
        [SerializeField] private int tileWidth;
        [SerializeField] private int tileHeight;
        [SerializeField] private List<Vector2> checkPoints;
        [SerializeField] private List<Vector2> alreadyFilledTiles;
        
        public int TileWidth => tileWidth;

        public int TileHeight => tileHeight;

        public List<Vector2> CheckPoints => checkPoints;

        public List<Vector2> AlreadyFilledTiles => alreadyFilledTiles;
    }
}
