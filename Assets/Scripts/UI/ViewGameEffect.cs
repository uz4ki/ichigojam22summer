using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Result;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ViewGameEffect : SingletonMonoBehaviour<ViewGameEffect>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> soundEffects;

        [SerializeField] private Image startTimerView;
        
        [SerializeField] private List<Sprite> numberImages;

        [FormerlySerializedAs("BumpAnimationCurve")]
        public AnimationCurve bumpScaleAnimationCurve;
        
        [SerializeField] private Image correctView;

        [SerializeField] private float correctAnimationDuration;
        
        [FormerlySerializedAs("CorrectAnimationCurve")]
        public AnimationCurve correctScaleAnimationCurve;
        
        [SerializeField] private Image failureView;

        [SerializeField] private float failureAnimationDuration;

        [SerializeField] private Image passView;
        
        [SerializeField] private float passAnimationDuration;
        
        [FormerlySerializedAs("PassAnimationCurve")]
        public AnimationCurve passScaleAnimationCurve;
        
        private Vector3 _initialCorrectViewScale;
        
        [SerializeField] private Image finishView;
        
        [SerializeField] private float finishAnimationDuration;
        
        [FormerlySerializedAs("PassAnimationCurve")]
        public AnimationCurve finishScaleAnimationCurve;
        
        private Vector3 _initialFinishViewScale;

        private void Start()
        {
            _initialCorrectViewScale = passView.transform.localScale;
            _initialFinishViewScale = finishView.transform.localScale;
        }

        public void CountDown()
        {
            StartCoroutine(StartCountDownCoroutine());
        }

        public void Correct()
        {
            StartCoroutine(CorrectCoroutine());
        }

        public void Pass()
        {
            StartCoroutine(PassCoroutine());
        }

        public void Failure()
        {
            StartCoroutine(FailureCoroutine());
        }

        public IEnumerator StartCountDownCoroutine()
        {
            startTimerView.gameObject.SetActive(true);
            var journey = 0f;

            for (var i = 0; i < 3; i++)
            {
                audioSource.PlayOneShot(soundEffects[0]);
                journey = 0f;
                startTimerView.sprite = numberImages[i];
                while (journey <= 1f)
                {
                    journey += Time.deltaTime;
                    var percent = Mathf.Clamp01(journey / 1f);
                    var curvePercent = bumpScaleAnimationCurve.Evaluate(percent);
                    startTimerView.transform.localScale = curvePercent * Vector3.one * 5f;

                    yield return null;
                }
            }
            audioSource.PlayOneShot(soundEffects[1]);

            startTimerView.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator CorrectCoroutine()
        {
            var journey = 0f;
            correctView.gameObject.SetActive(true);
            audioSource.PlayOneShot(soundEffects[2]);


            while (journey <= correctAnimationDuration)
            {
                journey += Time.deltaTime;
                var percent = Mathf.Clamp01(journey / correctAnimationDuration);
                var curvePercent = correctScaleAnimationCurve.Evaluate(percent);
                correctView.transform.localScale =  curvePercent * _initialCorrectViewScale;

                yield return null;
            }

            correctView.gameObject.SetActive(false);
            yield return null;
        }
        
        private IEnumerator FailureCoroutine()
        {
            var journey = 0f;
            failureView.gameObject.SetActive(true);
            audioSource.PlayOneShot(soundEffects[5]);


            while (journey <= failureAnimationDuration)
            {
                journey += Time.deltaTime;
                var percent = Mathf.Clamp01(journey / failureAnimationDuration);
                var curvePercent = correctScaleAnimationCurve.Evaluate(percent);
                failureView.transform.localScale =  curvePercent * _initialCorrectViewScale;

                yield return null;
            }

            failureView.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator PassCoroutine()
        {
            var journey = 0f;
            passView.gameObject.SetActive(true);
            audioSource.PlayOneShot(soundEffects[3]);


            while (journey <= passAnimationDuration)
            {
                journey += Time.deltaTime;
                var percent = Mathf.Clamp01(journey / passAnimationDuration);
                var curvePercent = passScaleAnimationCurve.Evaluate(percent);
                passView.transform.localPosition =  Vector2.right * (curvePercent * -2400f + 1200f);

                yield return null;
            }
            
            passView.gameObject.SetActive(false);
            yield return null;
        }

        public IEnumerator GameOverCoroutine()
        {
            var journey = 0f;
            finishView.gameObject.SetActive(true);
            audioSource.PlayOneShot(soundEffects[4]);


            while (journey <= finishAnimationDuration)
            {
                journey += Time.deltaTime;
                var percent = Mathf.Clamp01(journey / finishAnimationDuration);
                var curvePercent = finishScaleAnimationCurve.Evaluate(percent);
                finishView.transform.localScale =  curvePercent * _initialFinishViewScale;

                yield return null;
            }
            
            yield return new WaitForSeconds(2f);

            yield return null;
        }
    }
}
