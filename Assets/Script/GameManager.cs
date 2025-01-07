using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField] public Transform playerPosition;
    [SerializeField] internal Animator powerUpEffectAnimator;
    public static GameManager Instance { get { if (_instance == null) Debug.Log("No GameManager"); return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void powerUpEffect()
    {
        powerUpEffectAnimator.SetTrigger("Effect");
    }
}
