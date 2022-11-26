using System;
using General;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ViewMissionTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _missionText;

        private void Awake()
        {
            GameManager.Instance.onChangeLevel.AddListener(() => {
                _missionText.text = MiniGameLoader.Instance.LoadedGames.Peek().MissionMassage;
            });
        }
    }
}
