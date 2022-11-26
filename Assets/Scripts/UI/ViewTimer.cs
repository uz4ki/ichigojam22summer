using System.Collections;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ViewTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float bumpDuration;
        [FormerlySerializedAs("BumpAnimationCurve")]
        public AnimationCurve bumpScaleAnimationCurve = new AnimationCurve(new Keyframe(1, 1), new Keyframe(0.3f, 1.05f), new Keyframe(1, 1));

        private Vector3 _initialScale;
        
        private void Start()
        {
            _initialScale = transform.localScale;
        }

        public void Bump()
        {
            StartCoroutine(BumpCoroutine());
        }

        public void UpdateViewTimer()
        {
            timerText.text = $"{(int)GameTimer.Instance.Timer}";
        }
        
        private IEnumerator BumpCoroutine()
        {
            var journey = 0f;

            while (journey <= bumpDuration)
            {
                journey += Time.deltaTime;
                var percent = Mathf.Clamp01(journey / bumpDuration);
                var curvePercent = bumpScaleAnimationCurve.Evaluate(percent);
                timerText.transform.localScale = curvePercent * _initialScale;

                yield return null;
            }

            yield return null;
        }
    }
}
