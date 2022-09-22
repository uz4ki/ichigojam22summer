#nullable enable
using System;
using System.Collections.Generic;
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
        public int tileWidth;
        public int tileHeight;
        public List<Vector2> checkPoints;
        public List<Vector2> alreadyFilledTiles;
        
        [SerializeField] private GameObject viewPrefab;

        [SerializeField] private List<Vector2> route = new List<Vector2>();
        public TileMap? TileMap { get; private set; }

        private void Awake()
        {
            TileMap = new TileMap(tileWidth, tileHeight);

            if (alreadyFilledTiles != null)
            {
                foreach (var alreadyFilledTile in alreadyFilledTiles)
                {
                    if (alreadyFilledTile.x > tileWidth - 1 || alreadyFilledTile.y > tileHeight - 1)
                    {
                        throw new IndexOutOfRangeException("すでに埋まったタイルの座標が不適です");
                    }

                    TileMap.MapData[(int) alreadyFilledTile.x, (int) alreadyFilledTile.y].IsFilled = true;
                }
            }

            if (checkPoints[0].x > tileWidth - 1 || checkPoints[0].y > tileHeight - 1)
            {
                throw new IndexOutOfRangeException("スタート座標が不適です");
            }

            if (TileMap.MapData[(int) checkPoints[0].x, (int) checkPoints[0].y].IsFilled)
            {
                throw new ArgumentOutOfRangeException("スタート座標が埋まっています");
            }

            route.Add(checkPoints[0]);
            TileMap.MapData[(int) checkPoints[0].x, (int) checkPoints[0].y].IsFilled = true;
        }

        private void Start()
        {
            for (var y = 0; y < tileHeight; y++)
            {
                for (var x = 0; x < tileWidth; x++)
                {
                    var pos = x * Vector2.right + y * Vector2.up - new Vector2(tileWidth, tileHeight) / 2;
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

            if (targetVec.x > tileWidth - 1 ||
                targetVec.x < 0 ||
                targetVec.y > tileHeight - 1 ||
                targetVec.y < 0 ||
                TileMap.MapData[(int) targetVec.x, (int) targetVec.y].IsFilled)
            {
                return;
            }
            Debug.Log("Hoge");
            route.Add(targetVec);
            TileMap.MapData[(int) targetVec.x, (int) targetVec.y].IsFilled = true;
            TileMap.MapData[(int) targetVec.x, (int) targetVec.y].InstanceObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        public void UndoTile()
        {
            if (route.Count == 1) return;
            var currentPos = route[^1];
            route.RemoveAt(route.Count - 1);
            TileMap.MapData[(int) currentPos.x, (int) currentPos.y].IsFilled = false;
            TileMap.MapData[(int) currentPos.x, (int) currentPos.y].InstanceObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}

