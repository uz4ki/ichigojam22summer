using System;
using General;
using JoyconLib_scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        public bool JoyconConnected { get; private set; } = false;
        
        public Joycon JoyconL { get; private set; }
        public Joycon JoyconR { get; private set; }

        [SerializeField] private Image viewL; 
        [SerializeField] private Image viewR; 

        
        public UnityEvent onTryConnection;

        [SerializeField] private Button startButton;

        private void OnEnable()
        {
            OpenSetting();
        }

        public void OpenSetting()
        {
            var joycons = JoyconManager.Instance.j;

            if ( joycons == null || joycons.Count <= 0 ) return;

            JoyconL = joycons.Find( c =>  c.isLeft );
            JoyconR = joycons.Find( c =>  !c.isLeft );

            if (JoyconL != null && JoyconR != null)
            {
                JoyconConnected = true;
            }
            else
            {
                JoyconConnected = false;
            }
            
            startButton.gameObject.SetActive(JoyconConnected);
            viewL.gameObject.SetActive(JoyconL != null);
            viewR.gameObject.SetActive(JoyconR != null);
            
            onTryConnection.Invoke();
        }
        
        public void LoadGameScene()
        {
            GameManager.Instance.StartGame();
            SceneManager.LoadScene("MainGame");
        }
        
    }
}
