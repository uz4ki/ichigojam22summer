using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.OneStroke
{
    [System.SerializableAttribute]
    public class Phase{
        public List<Vector2> route;

        public Phase(List<Vector2> list){
            route = list;
        }
    }
    
    [CreateAssetMenu]
    public class OneStrokePuzzleInfo : ScriptableObject
    {
        [SerializeField] private int tileWidth;
        [SerializeField] private int tileHeight;
        [SerializeField] private List<Vector2> checkPoints;
        [SerializeField] private List<Vector2> alreadyFilledTiles;
        [SerializeField] private List<Phase> hints;
        
        public int TileWidth => tileWidth;

        public int TileHeight => tileHeight;

        public List<Vector2> CheckPoints => checkPoints;

        public List<Vector2> AlreadyFilledTiles => alreadyFilledTiles;
        public List<Phase> Hints => hints;
    }
}
