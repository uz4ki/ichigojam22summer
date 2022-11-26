using System;
using System.Collections.Generic;
using System.Linq;
using General;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.BlackJack
{
    [System.SerializableAttribute]
    public class Number
    {
        public List<Sprite> sameNumberCard;

        public Number(List<Sprite> list){
            sameNumberCard = list;
        }
    }
    public class BlackJackManager : MiniGameManager
    {
        [SerializeField] private List<AudioClip> sounds;
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private List<Number> cardSprites;
        [SerializeField] private List<Transform> players;
        [SerializeField] private Image cardPrefab;
        [SerializeField] private Image cursor;
        
        private int[] _playerScores;
        private int[] _playerHasAce;
        private List<Sprite>[] _playerCards;

        private Vector2? _cursorPosition;
        private int _maxScore;

        private float _inputDuration = 0.5f;
        private float _inputCoolDown; 
        protected override void Initialization()
        {
            _playerScores = new int[4];
            _playerHasAce = new int[4];
            _playerCards = new List<Sprite>[4];

            NumberOfHints = 3;
            cursor.gameObject.SetActive(false);
            for (var i = 0; i < 4; i++)
            {
                _playerCards[i] = new List<Sprite>();
                while (_playerScores[i] < 16)
                {
                    var cardNumber = UnityEngine.Random.Range(1, 13);
                    if (cardSprites[cardNumber-1].sameNumberCard.Count == 0) continue;
                    var randomCardSuit = UnityEngine.Random.Range(0, cardSprites[cardNumber-1].sameNumberCard.Count - 1);
                    var card = cardSprites[cardNumber-1].sameNumberCard[randomCardSuit];
                    cardSprites[cardNumber-1].sameNumberCard.Remove(card);
                    if (cardNumber == 1)
                    {
                        cardNumber = 11;
                        _playerHasAce[i]++;
                    }
                    else if(cardNumber > 10)
                    {
                        cardNumber = 10;
                    }
                    _playerScores[i] += cardNumber;
                    _playerCards[i].Add(card);
                    if (_playerScores[i] <= 21) continue;
                    if (_playerHasAce[i] == 0) continue;
                    _playerScores[i] -= 10;
                    _playerHasAce[i]--;
                }
                if (_playerScores[i] > 21) _playerScores[i] = 0;
            }

            _inputCoolDown = 0f;
            _maxScore = _playerScores.Max();
            DealCards();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlayingGame) return;
            if (JoyconInput.Instance.InputStick()) 
            {
                SelectPlayer(JoyconInput.Instance.stickVec);
            }

            if (_inputCoolDown < _inputDuration)
            {
                _inputCoolDown += Time.deltaTime;
                return;
            }
            
            if (JoyconInput.Instance.JoyconR.GetButton(Joycon.Button.SL) &&
                JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.SR))
            {
                PassLevel();
            }
            else if (JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                JudgeAnswer();
            }
        }

        public override void SendHint(int hintCount)
        {
            cursor.gameObject.SetActive(false);
            _cursorPosition = null;
            var cardBack = cardSprites[13].sameNumberCard[0];
            var minScore = _playerScores.Min();
            var minIndex = Array.IndexOf(_playerScores, minScore);
            _playerScores[minIndex] = 99;
            foreach (Transform card in players[minIndex])
            {
                card.gameObject.GetComponent<Image>().sprite = cardBack;
            }
        }

        private void SelectPlayer(Vector2 selectVec)
        {
            if (_cursorPosition != selectVec) audioSource.PlayOneShot(sounds[0]);
            cursor.gameObject.SetActive(true);
            _cursorPosition = selectVec;
            if (_cursorPosition == Vector2.up)
            {
                cursor.transform.localPosition = players[0].localPosition + Vector3.down * 90f;
                cursor.transform.localRotation = Quaternion.Euler(0f, 0f, 0);
            }
            else if (_cursorPosition == Vector2.right)
            {
                cursor.transform.localPosition = players[1].localPosition + Vector3.left * 90f;
                cursor.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if (_cursorPosition == Vector2.down)
            {
                cursor.transform.localPosition = players[2].localPosition + Vector3.up * 90f;
                cursor.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (_cursorPosition == Vector2.left)
            {
                cursor.transform.localPosition = players[3].localPosition + Vector3.right * 90f;
                cursor.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }

        private void JudgeAnswer()
        {
            var answer = 0;
            if (_cursorPosition == Vector2.up)
            {
                answer = 0;
            }
            else if (_cursorPosition == Vector2.right)
            {
                answer = 1;
            }
            else if (_cursorPosition == Vector2.down)
            {
                answer = 2;
            }
            else if (_cursorPosition == Vector2.left)
            {
                answer = 3;
            }

            if (_playerScores[answer] == _maxScore)
            {
                LevelClear();
            }
            else
            {
                LevelFailure();
            }
        }

        private void DealCards()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < _playerCards[i].Count; j++)
                {
                    var obj = Instantiate(cardPrefab, players[i]);
                    obj.sprite = _playerCards[i][j];
                    obj.transform.localPosition = Vector3.right * (j - (_playerCards[i].Count - 1)/ 2f) * 60f;
                }
                
                switch (i)
                {
                    case 0:
                        players[i].localRotation = Quaternion.Euler(0f, 0f, 180f);
                        players[i].localPosition = Vector3.up * 330f;
                        break;
                    case 1:
                        players[i].localRotation = Quaternion.Euler(0f, 0f, 90f);
                        players[i].localPosition = Vector3.right * 350f;
                        break;
                    case 2:
                        players[i].localRotation = Quaternion.Euler(0f, 0f, 0f);
                        players[i].localPosition = Vector3.down * 330f;
                        break;
                    case 3:
                        players[i].localRotation = Quaternion.Euler(0f, 0f, -90f);
                        players[i].localPosition = Vector3.left * 350f;
                        break;
                }
            }
        }
    }
}
