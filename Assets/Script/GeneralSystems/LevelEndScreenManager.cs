using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyDoom.GeneralSystems
{
    public class LevelEndScreenManager : MonoBehaviour
    {
        [SerializeField] GameObject levelEndScreen;
        [SerializeField] TextMeshProUGUI killCounterText;
        [SerializeField] TextMeshProUGUI powerUpCounterText;

        public void ShowLevelEndScreen()
        {
            killCounterText.text = GameManager.Instance.EnemiesKilledPercentage().ToString("F0") + "%";
            powerUpCounterText.text = GameManager.Instance.PowerUpsCollectedPercentage().ToString("F0") + "%";
        
            levelEndScreen.SetActive(true);
            GameManager.Instance.StopLevel();
        }
    }
}

