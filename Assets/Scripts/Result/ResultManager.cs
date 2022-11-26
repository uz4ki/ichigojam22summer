using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Result
{
    [System.SerializableAttribute]
    public struct AchievementInfo
    {
        public int score;
        public Sprite achievementSprite;
        public Sprite achievementNameSprite;
    }

    public class ResultManager : SingletonMonoBehaviour<ResultManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float rotationSecond;

        [SerializeField] private Button returnToTitleButton;
        
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Slider spinTypeSlider;
        
        [SerializeField] private Image achievementImage;
        [SerializeField] private Image achievementNameImage;
        
        [SerializeField]
        private List<AchievementInfo> achievementInfos = new List<AchievementInfo> ();

        private int _score;
        private float _spinTypeValue;

        public void StartResult()
        {
            _score = GameManager.Instance.Score;
            _spinTypeValue = GameManager.Instance.SpinPercent;
            for (var i = 0; i < achievementInfos.Count; i++)
            {
                var info = achievementInfos[i];
                if (_score > info.score) continue;
                achievementImage.sprite = info.achievementSprite;
                achievementNameImage.sprite = info.achievementNameSprite;
                break;
            }
            StartCoroutine(ResultCoroutine());
        }

        private IEnumerator ResultCoroutine()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("Title");
            asyncLoad.allowSceneActivation = false;
            var isClicked = false;
            returnToTitleButton.onClick.AddListener(() => isClicked = true);
            
            var journey = 0f;
            achievementNameImage.gameObject.SetActive(false);
            audioSource.PlayOneShot(audioSource.clip);
            while (journey < rotationSecond)
            {
                journey += Time.deltaTime;
                spinTypeSlider.value =
                    _spinTypeValue * (journey / rotationSecond) + 0.5f * (1f - (journey / rotationSecond));
                scoreText.text = $"{((int)(_score * (journey / rotationSecond))):D2}";
                achievementImage.transform.localRotation *= Quaternion.Euler(Vector3.forward * 100f);
                yield return null;
            }
            spinTypeSlider.value = _spinTypeValue;
            scoreText.text = $"{_score:D2}";
            achievementImage.transform.localRotation = Quaternion.identity;
            achievementNameImage.gameObject.SetActive(true);

            yield return new WaitUntil(() => isClicked);
            
            asyncLoad.allowSceneActivation = true;
            yield return asyncLoad;
        }
        
    }
}
