using System;
using System.Collections;
using JoyconLib_scripts;
using UnityEngine;

namespace General
{
    public class JoyconGyro : SingletonMonoBehaviour<JoyconGyro>
    {
        public float loopValue;
        public float nowAngle;
        [SerializeField] private Transform brain;
        [SerializeField] private Transform viewBrain;
        private Quaternion prevVec;

        private Joycon joyconL;

        protected override void Awake()
        {
            if (!CheckInstance())
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            var joycons = JoyconManager.Instance.j;

            if ( joycons == null || joycons.Count <= 0 ) return;

            joyconL = joycons.Find( c =>  c.isLeft );
        }

        private void Update()
        {
            if (GameManager.Instance.IsPlayingGame) CalcJoyconGyroInGame();
            if (GameManager.Instance.isCorrectingGyro) CalcJoyconGyroInSetting();
        }
        
        public void ResetViewBrain()
        {
            viewBrain.rotation = Quaternion.identity;
        }
        
        private void CalcJoyconGyroInSetting()
        {
            nowAngle += joyconL.GetGyro().y * Time.deltaTime;
        }

        public void CalcJoyconGyroInSettingView()
        {
            if (nowAngle > loopValue)
            {
                nowAngle -= loopValue;
                RumbleOnGetNormalHint();
            }
            if (nowAngle < - loopValue)
            {
                nowAngle += loopValue;
                RumbleOnGetNormalHint();
            }
            
            brain.rotation = Quaternion.Euler(0, (nowAngle * 360f) / loopValue, 0);
        }

        private void CalcJoyconGyroInGame()
        {
            nowAngle += joyconL.GetGyro().y * Time.deltaTime;
            if (nowAngle > loopValue)
            {
                nowAngle -= loopValue;
                MiniGameLoader.Instance.GetHints();
            }
            if (nowAngle < - loopValue)
            {
                nowAngle += loopValue;
                MiniGameLoader.Instance.GetHints();
            }
        
        
            brain.rotation = Quaternion.Euler(0, (nowAngle * 360f) / loopValue, 0);
        }

        public void RumbleOnGetNormalHint()
        {
            joyconL.SetRumble (160, 320, 0.6f, 200);
        }
    
        public IEnumerator RumbleOnGetMaxHint()
        {
            joyconL.SetRumble (160, 320, 0.6f, 100);
            yield return new WaitForSeconds(0.2f);
            joyconL.SetRumble (160, 320, 0.6f, 100);
            yield return new WaitForSeconds(0.2f);
            joyconL.SetRumble(160, 320, 0.6f, 100);
        }
    }
}
