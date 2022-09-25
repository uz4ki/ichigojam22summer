#nullable enable
using System;
using System.Collections.Generic;
using General;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Puzzle.OneStroke
{
    public struct TileInfo
    {
        public bool IsFilled;
        public GameObject InstanceObject;
    }

    public class TileMap
    {
        public readonly TileInfo[,] MapData;

        public TileMap(int tileWidth, int tileHeight)
        {
            MapData = new TileInfo[tileWidth, tileHeight];

            for (var i = 0; i < tileHeight; i++)
            {
                for (var j = 0; j < tileWidth; j++)
                {
                    MapData[i, j].IsFilled = false;
                }
            }
        }
    }

    public class TileManager : MonoBehaviour
    {
        [SerializeField] private GameObject viewPrefab;

        [SerializeField] private OneStrokeManager oneStrokeManager;

        [SerializeField] private List<Vector2> route = new List<Vector2>();
        public TileMap? TileMap { get; private set; }

        private int _tileWidth;
        private int _tileHeight;
        private List<Vector2> _checkPoints = null!;
        private List<Vector2>? _alreadyFilledTiles;

        private int _nowCheckPoint = 0;
        public void Initialization()
        {
            _tileWidth = oneStrokeManager.TileWidth;
            _tileHeight = oneStrokeManager.TileHeight;
            _checkPoints = oneStrokeManager.CheckPoints;
            _alreadyFilledTiles = oneStrokeManager.AlreadyFilledTiles;
            
            TileMap = new TileMap(_tileWidth, _tileHeight);

            if (_alreadyFilledTiles != null)
            {
                foreach (var alreadyFilledTile in _alreadyFilledTiles)
                {
                    if (alreadyFilledTile.x > _tileWidth - 1 || alreadyFilledTile.y > _tileHeight - 1)
                    {
                        throw new IndexOutOfRangeException("すでに埋まったタイルの座標が不適です");
                    }

                    TileMap.MapData[(int) alreadyFilledTile.x, (int) alreadyFilledTile.y].IsFilled = true;
                }
            }

            if (_checkPoints[0].x > _tileWidth - 1 || _checkPoints[0].y > _tileHeight - 1)
            {
                throw new IndexOutOfRangeException("スタート座標が不適です");
            }

            if (TileMap.MapData[(int) _checkPoints[0].x, (int) _checkPoints[0].y].IsFilled)
            {
                throw new ArgumentOutOfRangeException("スタート座標が埋まっています");
            }

            route.Add(_checkPoints[0]);
            TileMap.MapData[(int) _checkPoints[0].x, (int) _checkPoints[0].y].IsFilled = true;
        }

        private void Start()
        {
            for (var y = 0; y < oneStrokeManager.TileHeight; y++)
            {
                for (var x = 0; x < oneStrokeManager.TileWidth; x++)
                {
                    var pos = x * Vector2.right + y * Vector2.up - new Vector2(oneStrokeManager.TileWidth, oneStrokeManager.TileHeight) / 2;
                    TileMap.MapData[x, y].InstanceObject = Instantiate(viewPrefab, pos, Quaternion.identity, transform);
                    TileMap.MapData[x, y].InstanceObject.transform.GetChild(0).gameObject.SetActive(TileMap.MapData[x, y].IsFilled);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) MoveTile(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveTile(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveTile(Vector2.right);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveTile(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.Z)) UndoTile();
        }


        public void MoveTile(Vector2 moveVec)
        {
            var currentPos = route[^1];
            var targetVec = currentPos + moveVec;

            if (targetVec.x > oneStrokeManager.TileWidth - 1 ||
                targetVec.x < 0 ||
                targetVec.y > oneStrokeManager.TileHeight - 1 ||
                targetVec.y < 0 ||
                TileMap.MapData[(int) targetVec.x, (int) targetVec.y].IsFilled)
            {
                return;
            }

            for (var i = 1; i < _checkPoints.Count; i++)
            {
                if (targetVec != _checkPoints[i]) continue;
                if (_nowCheckPoint == i - 1)
                {
                    _nowCheckPoint++;
                    break;
                }
                else
                {
                    return;
                }
            }
            
            route.Add(targetVec);
            TileMap.MapData[(int) targetVec.x, (int) targetVec.y].IsFilled = true;
            TileMap.MapData[(int) targetVec.x, (int) targetVec.y].InstanceObject.transform.GetChild(0).gameObject.SetActive(true);

            if (route.Count == _tileWidth * _tileWidth - (_alreadyFilledTiles?.Capacity ?? 0))
            {
                GameManager.Instance.LevelOver(false);
            }
        }

        public void UndoTile()
        {
            if (route.Count == 1) return;
            var currentPos = route[^1];
            for (var i = 1; i < _checkPoints.Count; i++)
            {
                if (currentPos != _checkPoints[i]) continue;
                _nowCheckPoint--;
                break;
            }
            route.RemoveAt(route.Count - 1);
            TileMap.MapData[(int) currentPos.x, (int) currentPos.y].IsFilled = false;
            TileMap.MapData[(int) currentPos.x, (int) currentPos.y].InstanceObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}

