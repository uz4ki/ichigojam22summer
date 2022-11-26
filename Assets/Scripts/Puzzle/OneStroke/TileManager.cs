#nullable enable
using System;
using System.Collections.Generic;
using General;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
                    MapData[j, i].IsFilled = false;
                }
            }
        }
    }

    public class TileManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> sounds;
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private GameObject viewPrefab;
        [SerializeField] private List<SpriteRenderer> checkPointImages;
        private float _tileSize;

        [SerializeField] private List<Vector2> route = new List<Vector2>();
        public TileMap? TileMap { get; private set; }

        private int _tileWidth;
        private int _tileHeight;
        private List<Vector2> _checkPoints = null!;
        private List<Vector2>? _alreadyFilledTiles;
        private List<Phase> _hints;

        private int _nowCheckPoint = 0;
        private int _nowHintMoves = 1;
        [SerializeField] private float inputDuration = 0.1f;
        private float _inputCoolDown;
        public void Initialization(OneStrokePuzzleInfo puzzleInfo)
        {
            Debug.Log(puzzleInfo);
            _tileSize = 2.25f;
            _tileWidth = puzzleInfo.TileWidth;
            _tileHeight = puzzleInfo.TileHeight;
            _checkPoints = puzzleInfo.CheckPoints;
            _alreadyFilledTiles = puzzleInfo.AlreadyFilledTiles;
            _hints = puzzleInfo.Hints;
            
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

            for (var y = 0; y < _tileHeight; y++)
            {
                for (var x = 0; x < _tileWidth; x++)
                {
                    var pos = x * Vector2.right + y * Vector2.up;
                    pos -= (_tileWidth * Vector2.right + _tileHeight * Vector2.up - Vector2.one) / 2;
                    pos *= _tileSize;
                    TileMap.MapData[x, y].InstanceObject = Instantiate(viewPrefab, transform.GetChild(0));
                    TileMap.MapData[x, y].InstanceObject.transform.localPosition = pos;
                    TileMap.MapData[x, y].InstanceObject.SetActive(!TileMap.MapData[x, y].IsFilled);
                }
            }
            
            route.Add(_checkPoints[0]);
            TileMap.MapData[(int) _checkPoints[0].x, (int) _checkPoints[0].y].IsFilled = true;
            
            for (var i = 0; i < _checkPoints.Count; i++)
            {
                checkPointImages[i].transform.parent = TileMap.MapData[(int) _checkPoints[i].x, (int) _checkPoints[i].y]
                    .InstanceObject.transform;
                checkPointImages[i].transform.localPosition = Vector3.zero;
                checkPointImages[i].gameObject.SetActive(true);
            }
            
            transform.GetChild(0).localScale = Vector3.one * puzzleInfo.TileSize;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlayingGame) return;

            if (_inputCoolDown < inputDuration)
            {
                _inputCoolDown += Time.deltaTime;
                return;
            }

            if (JoyconInput.Instance.JoyconR.GetButton(Joycon.Button.SL) &&
                JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.SR))
            {
                gameObject.GetComponent<OneStrokeManager>().PassLevel();
            }
            else if (JoyconInput.Instance.InputStick())
            {
                _inputCoolDown = 0f;
                MoveTile(JoyconInput.Instance.stickVec);
                audioSource.PlayOneShot(sounds[0]);
            }
            else if (JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
            {
                _inputCoolDown = 0f;
                UndoTile();
                audioSource.PlayOneShot(sounds[1]);
            }
            
        }


        public void MoveTile(Vector2 moveVec)
        {
            var currentPos = route[^1];
            var targetVec = currentPos + moveVec;

            if (targetVec.x > _tileWidth - 1 ||
                targetVec.x < 0 ||
                targetVec.y > _tileHeight - 1 ||
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
            var bar = TileMap.MapData[(int) targetVec.x, (int) targetVec.y].InstanceObject.transform.GetChild(0)
                .transform.GetChild(0).gameObject;
            bar.transform.position =
                (TileMap.MapData[(int) route[^2].x, (int) route[^2].y].InstanceObject.transform.position +
                TileMap.MapData[(int) route[^1].x, (int) route[^1].y].InstanceObject.transform.position) / 2;
            if (route[^1].y - route[^2].y != 0)
            {
                bar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }
            else
            {
                bar.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            bar.SetActive(true);

            var filled = 0;
            if (_alreadyFilledTiles == null) filled = 0;
            else
            {
                filled = _alreadyFilledTiles.Count;
            }
   
            if (route.Count == (_tileHeight * _tileWidth) - filled)
            {
                gameObject.GetComponent<OneStrokeManager>().LevelClear();
            }
        }

        public void UndoTile()
        {
            if (route.Count == _nowHintMoves) return;
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

        public void ApplyHintMoves(int hintCount)
        {
            for (var i = 0; i < _tileWidth * _tileHeight; i++)
            {
                UndoTile();
            }

            var moves = _hints[hintCount - 1].route;
            for (var i = 0; i < moves.Count; i++)
            {
                MoveTile(moves[i]);
            }

            audioSource.PlayOneShot(sounds[0]);
            _nowHintMoves = route.Count;
        }
    }

}

