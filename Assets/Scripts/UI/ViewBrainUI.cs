using System;
using General;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ViewBrainUI : MonoBehaviour
    {
        public TextMeshProUGUI brainLevelText;
        public Transform levelMaxView;
        private void Start()
        {
            GameManager.Instance.onChangeLevel.AddListener(() =>
            {
                brainLevelText.text = "脳Lv.0"; 
                levelMaxView.gameObject.SetActive(false);
            });
            
            MiniGameLoader.Instance.OnGetHint.AddListener((int level, bool isMax) =>
            {
                brainLevelText.text = $"脳Lv.{level}"; 
                levelMaxView.gameObject.SetActive(isMax);
            });
        }
        
        
    }
}
