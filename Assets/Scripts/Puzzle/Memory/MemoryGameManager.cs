using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Memory
{
    public enum QuestionTypes
    {
        AmountAll = 0,
        AmountSpecie,
        MaxSpecie,
        MinSpecie,
    }
    
    public enum AnswerPositions
    {
        UpperLeft = 0,
        UpperRight,
        LowerLeft,
        LowerRight,
    }
    
    public class MemoryGameManager : MiniGameManager
    {
        [SerializeField] private List<AudioClip> sounds;
        [SerializeField] private AudioSource audioSource;
        
        private QuestionTypes _questionType;
        private AnswerPositions _answerPosition;
        private AnswerPositions _nowPosition;

        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private Image cursor;
        [SerializeField] private Transform memorizedButton;
        [SerializeField] private Transform[] answerButtons;
        [SerializeField] private TextMeshProUGUI massage;
        [SerializeField] private CanvasGroup cover;
        
        [SerializeField] private float inputDuration = 0.05f;
        private float _inputCoolDown;
        protected override void Initialization()
        {
            NumberOfHints = 3;
            GenerateLevel();
            StartCoroutine(MemoryPhase());
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
                PassLevel();
            }
        }

        private IEnumerator MemoryPhase()
        {
            cover.gameObject.SetActive(false);
            cursor.gameObject.SetActive(true);
            cursor.transform.position = memorizedButton.position;

            _inputCoolDown = 0f;
            yield return new WaitUntil(() => _inputCoolDown > inputDuration);
            
            while (!JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                yield return null;
            }
            
            _inputCoolDown = 0f;
            memorizedButton.gameObject.SetActive(false);
            yield return AnswerPhase();
            yield return null;
        }
        
        private IEnumerator AnswerPhase()
        {
            cover.gameObject.SetActive(true);
            massage.text = levelGenerator.questionText;
            foreach (var button in answerButtons)
            {
                button.gameObject.SetActive(true);
            }
            cursor.transform.position = answerButtons[0].position;
            
            yield return new WaitUntil(() => _inputCoolDown > inputDuration);
            
            while (!JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                if (JoyconInput.Instance.InputStick()) 
                { 
                    _inputCoolDown = 0f;
                    MoveCursor(JoyconInput.Instance.stickVec);
                }
                yield return null;
            }

            if (_nowPosition == _answerPosition) LevelClear();
            else LevelFailure();
            
            yield return null;
        }

        public override void SendHint(int hintCount)
        {
            if (hintCount == 1) cover.alpha = 0.95f;
            else if (hintCount == 2) cover.alpha = 0.85f;
            else if (hintCount == 3) cover.alpha = 0.5f;
        }

        private void MoveCursor(Vector2 moveVec)
        {
            switch (_nowPosition)
            {
                case AnswerPositions.UpperLeft:
                    if (moveVec == Vector2.right)
                    {
                        _nowPosition = AnswerPositions.UpperRight;
                        cursor.transform.position = answerButtons[1].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    else if (moveVec == Vector2.down)
                    {
                        _nowPosition = AnswerPositions.LowerLeft;
                        cursor.transform.position = answerButtons[2].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    break;
                case AnswerPositions.UpperRight:
                    if (moveVec == Vector2.left)
                    {
                        _nowPosition = AnswerPositions.UpperLeft;
                        cursor.transform.position = answerButtons[0].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    else if (moveVec == Vector2.down)
                    {
                        _nowPosition = AnswerPositions.LowerRight;
                        cursor.transform.position = answerButtons[3].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    break;
                case AnswerPositions.LowerLeft:
                    if (moveVec == Vector2.right)
                    {
                        _nowPosition = AnswerPositions.LowerRight;
                        cursor.transform.position = answerButtons[3].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    else if (moveVec == Vector2.up)
                    {
                        _nowPosition = AnswerPositions.UpperLeft;
                        cursor.transform.position = answerButtons[0].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    break;
                case AnswerPositions.LowerRight:
                    if (moveVec == Vector2.left)
                    {
                        _nowPosition = AnswerPositions.LowerLeft;
                        cursor.transform.position = answerButtons[2].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    else if (moveVec == Vector2.up)
                    {
                        _nowPosition = AnswerPositions.UpperRight;
                        cursor.transform.position = answerButtons[1].transform.position;
                        audioSource.PlayOneShot(sounds[0]);
                    }
                    break;
            }
        }

        private void GenerateLevel()
        {
            var randomInt = UnityEngine.Random.Range(0, 3);
            _questionType = (QuestionTypes) randomInt;
            randomInt = UnityEngine.Random.Range(0, 3);
            _answerPosition = (AnswerPositions) randomInt;
            levelGenerator.GeneratePainting(_questionType, _answerPosition);
        }
    }
}
