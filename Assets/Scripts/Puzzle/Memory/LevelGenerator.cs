using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Memory
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] answerTexts;
        public string questionText;
        [SerializeField] private Sprite[] dogSprites;
        [SerializeField] private Sprite[] catSprites;
        [SerializeField] private Sprite[] birdSprites;
        [SerializeField] private Sprite[] horseSprites;
        [SerializeField] private Image animalPrefab;
        
        private List<Sprite[]> _animalImages;
        public void GeneratePainting(QuestionTypes questionType, AnswerPositions answerPosition)
        {
            _animalImages = new List<Sprite[]>{dogSprites, catSprites, birdSprites, horseSprites};
            switch (questionType)
            {
                case QuestionTypes.AmountAll:
                    LevelOnAmountAll(answerPosition);
                    break;
                case QuestionTypes.AmountSpecie:
                    LevelOnAmountSpecie(answerPosition);
                    break;
                case QuestionTypes.MaxSpecie:
                    LevelOnMaxSpecie(answerPosition);
                    break;
                case QuestionTypes.MinSpecie:
                    LevelOnMinSpecie(answerPosition);
                    break;
            }
            Debug.Log(questionType);
        }

        private void LevelOnAmountAll(AnswerPositions answerPosition)
        {
            questionText = "ぜんぶでなんびき?";
            answerTexts[0].text = "7";
            answerTexts[1].text = "8";
            answerTexts[2].text = "9";
            answerTexts[3].text = "10";
            
            var answer = 0;
            switch (answerPosition)
            {
                case AnswerPositions.UpperLeft:
                    answer = 7;
                    break;
                case AnswerPositions.UpperRight:
                    answer = 8;
                    break;
                case AnswerPositions.LowerLeft:
                    answer = 9;
                    break;
                case AnswerPositions.LowerRight:
                    answer = 10;
                    break;
            }
            
            InstantiateImage(dogSprites);
            InstantiateImage(catSprites);
            InstantiateImage(birdSprites);
            InstantiateImage(horseSprites);

            for (var i = 0; i < answer - 4; i++)
            {
                var randomInt = UnityEngine.Random.Range(0, 3);
                InstantiateImage(_animalImages[randomInt]);
                Debug.Log("Hoge");
            }
        }
        
        private void LevelOnAmountSpecie(AnswerPositions answerPosition)
        {
            answerTexts[0].text = "1";
            answerTexts[1].text = "2";
            answerTexts[2].text = "3";
            answerTexts[3].text = "4";
            
            var answer = 0;
            switch (answerPosition)
            {
                case AnswerPositions.UpperLeft:
                    answer = 1;
                    break;
                case AnswerPositions.UpperRight:
                    answer = 2;
                    break;
                case AnswerPositions.LowerLeft:
                    answer = 3;
                    break;
                case AnswerPositions.LowerRight:
                    answer = 4;
                    break;
            }
            
            InstantiateImage(dogSprites);
            InstantiateImage(catSprites);
            InstantiateImage(birdSprites);
            InstantiateImage(horseSprites);
            
            var randomInt = UnityEngine.Random.Range(0, 3);
            
            for (var i = 0; i < answer - 1; i++)
            {
                InstantiateImage(_animalImages[randomInt]);
            }

            switch (randomInt)
            {
                case 0:
                    questionText = "いぬはなんびき?";
                    break;
                case 1:
                    questionText = "ねこはなんびき?";
                    break;
                case 2:
                    questionText = "とりはなんびき?";
                    break;
                case 3:
                    questionText = "うまはなんびき?";
                    break;
            }

            _animalImages.Remove(_animalImages[randomInt]);
            var amountAll = UnityEngine.Random.Range(7, 10);
            for (var i = 0; i < amountAll - answer - 3; i++)
            {
                randomInt = UnityEngine.Random.Range(0, 2);
                InstantiateImage(_animalImages[randomInt]);
            }
        }
        
        private void LevelOnMaxSpecie(AnswerPositions answerPosition)
        {
            questionText = "いちばんおおいのは?";
            answerTexts[0].text = "いぬ";
            answerTexts[1].text = "ねこ";
            answerTexts[2].text = "とり";
            answerTexts[3].text = "うま";
            
            var answer = 0;
            switch (answerPosition)
            {
                case AnswerPositions.UpperLeft:
                    answer = 0;
                    break;
                case AnswerPositions.UpperRight:
                    answer = 1;
                    break;
                case AnswerPositions.LowerLeft:
                    answer = 2;
                    break;
                case AnswerPositions.LowerRight:
                    answer = 3;
                    break;
            }

            InstantiateImage(dogSprites);
            InstantiateImage(catSprites);
            InstantiateImage(birdSprites);
            InstantiateImage(horseSprites);
            
            var maxSpecieAmount = UnityEngine.Random.Range(4, 5);
            for (var i = 0; i < maxSpecieAmount - 1; i++)
            {
                InstantiateImage(_animalImages[answer]);
            }
            
            _animalImages.Remove(_animalImages[answer]);
            var amount = UnityEngine.Random.Range(7, 9);
            for (var i = 0; i < amount - maxSpecieAmount - 3; i++)
            {
                var randomInt = UnityEngine.Random.Range(0, 2);
                InstantiateImage(_animalImages[randomInt]);
            }
        }
        
        private void LevelOnMinSpecie(AnswerPositions answerPosition)
        {
            questionText = "いちばんすくないのは?";
            answerTexts[0].text = "いぬ";
            answerTexts[1].text = "ねこ";
            answerTexts[2].text = "とり";
            answerTexts[3].text = "うま";
            
            var answer = 0;
            switch (answerPosition)
            {
                case AnswerPositions.UpperLeft:
                    answer = 0;
                    break;
                case AnswerPositions.UpperRight:
                    answer = 1;
                    break;
                case AnswerPositions.LowerLeft:
                    answer = 2;
                    break;
                case AnswerPositions.LowerRight:
                    answer = 3;
                    break;
            }

            foreach (var animal in _animalImages)
            {
                InstantiateImage(animal);
            }
            
            _animalImages.Remove(_animalImages[answer]);
            
            foreach (var animal in _animalImages)
            {
                InstantiateImage(animal);
            }
            
            var addAmount = UnityEngine.Random.Range(0, 3);
            for (var i = 0; i < addAmount; i++)
            {
                var randomInt = UnityEngine.Random.Range(0, 2);
                InstantiateImage(_animalImages[randomInt]);
            }
        }

        private void InstantiateImage(Sprite[] spriteList)
        {
            var randomInt = UnityEngine.Random.Range(0, spriteList.Length);
            var obj = Instantiate(animalPrefab, transform);
            obj.transform.localPosition = new Vector3(UnityEngine.Random.Range(-210f, 210f),
                UnityEngine.Random.Range(-90f, 120f), 0f);
            obj.sprite = spriteList[randomInt];
            obj.SetNativeSize();
        }
    }
}
