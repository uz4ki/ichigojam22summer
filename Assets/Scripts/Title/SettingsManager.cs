using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class SettingsManager : SingletonMonoBehaviour<SettingsManager>
    {
        [SerializeField] private Image viewJoyconL;
        [SerializeField] private Image viewJoyconR;

        [SerializeField] private Image viewCorrectingText;
        [SerializeField] private List<Sprite> viewCorrectingTexts;
        [SerializeField] private TextMeshProUGUI gyroText;

        [SerializeField] private Button returnButton;
        
        [SerializeField] private float inputDuration = 0.5f;
        
        protected override void Awake()
        {
            CheckInstance();
            TitleManager.Instance.onTryConnection.AddListener(() =>
            {
                viewJoyconL.gameObject.SetActive(TitleManager.Instance.JoyconL != null);
                viewJoyconR.gameObject.SetActive(TitleManager.Instance.JoyconR != null);
            });
        }

        private void Update()
        {
            if (!TitleManager.Instance.JoyconConnected) return;
            if (GameManager.Instance.isCorrectingGyro) return;
            if (!JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP)) return;
            StartCoroutine(CorrectGyro());
        }

        private IEnumerator CorrectGyro()
        {
            GameManager.Instance.isCorrectingGyro = true;
            returnButton.gameObject.SetActive(false);
            gyroText.gameObject.SetActive(false);
            JoyconGyro.Instance.nowAngle = 0f;
            viewCorrectingText.sprite = viewCorrectingTexts[1];
            
            yield return new WaitForSeconds(inputDuration);
            
            while (!JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                yield return null;
            }
            
            JoyconGyro.Instance.ResetViewBrain();
            JoyconGyro.Instance.loopValue = Mathf.Abs(JoyconGyro.Instance.nowAngle / 3f);
            JoyconGyro.Instance.nowAngle = 0f;
            gyroText.text = $"現在の設定 : {JoyconGyro.Instance.loopValue:F2}";
            viewCorrectingText.sprite = viewCorrectingTexts[2];
            
            yield return new WaitForSeconds(inputDuration);
            
            while (!JoyconInput.Instance.JoyconR.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                JoyconGyro.Instance.CalcJoyconGyroInSettingView();
                yield return null;
            }
            
            yield return new WaitForSeconds(inputDuration); 
            
            GameManager.Instance.isCorrectingGyro = false;
            JoyconGyro.Instance.ResetViewBrain();
            returnButton.gameObject.SetActive(true);
            viewCorrectingText.sprite = viewCorrectingTexts[0];
            gyroText.gameObject.SetActive(true);
        }
    }
}
