using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyDoom.GeneralSystems
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        [SerializeField] public Transform playerPosition;
        [SerializeField] internal Animator powerUpEffectAnimator;
        [SerializeField] public LevelEndScreenManager levelEndScreenManager;
    
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject powerUpsContainer;
        private int enemyCount;
        private int powerUpCount;
        
        public static GameManager Instance { get { if (_instance == null) Debug.Log("No GameManager"); return _instance; } }
    
        private void Awake()
        {
            _instance = this;
            ResumeLevel();
        }
    
        void Start()
        {
            PlayerStats.Instance.OnPlayerDeath += RestartGame;
            CollectLevelStatsInfo();
        }
    
        public void RestartGame([CanBeNull] object sender, EventArgs e)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        public void powerUpEffect()
        {
            powerUpEffectAnimator.SetTrigger("Effect");
        }
    
        void CollectLevelStatsInfo()
        {
            enemyCount = enemyContainer.transform.childCount;
            powerUpCount = powerUpsContainer.transform.childCount;
        }
    
        public float EnemiesKilledPercentage()
        {
            float enemiesKilledPercentage = ((float)(enemyCount - enemyContainer.transform.childCount) / enemyCount) * 100f;
            return enemiesKilledPercentage;
        }
        
        public float PowerUpsCollectedPercentage()
        {
            float powerUpsCollectedPercentage = ((float)(powerUpCount - powerUpsContainer.transform.childCount) / powerUpCount) * 100f;
            return powerUpsCollectedPercentage;
        }

        public void StopLevel()
        {
            Time.timeScale = 0;
        }
        
        public void ResumeLevel()
        {
            Time.timeScale = 1;
        }
    }
}