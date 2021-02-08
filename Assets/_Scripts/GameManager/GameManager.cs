using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singlton;

    [Header("Settings Game")]
    [SerializeField] private float _moveStep;
    [SerializeField] private float _baseDelayMove;
    [SerializeField] private float _frightenedDuration;
    [SerializeField] private int _startLives;
    [SerializeField] private float _deathDelay;

    [Header("Food")] 
    [SerializeField] private int _pelletBounty;
    [SerializeField] private int _ghostBounty;
    
    [Header("Links")] 
    public Pacman pacman;
    public Ghost shadow;
    
    
    private UIManager UI;
    private int _level;

    private int _score;
    private int _lives;
    private int _eatenPillets;
    private int _eatenGhosts;
    private List<Ghost> _ghosts;
    private GameTimer _timer;
    private GhostManager _ghostManager;
    private FrightenedMode _frightenedMode;



    public float moveStep => _moveStep;
    public float baseDelayMove => _baseDelayMove;
    public float timerTime => _timer.time;
    public int eatenPillets => _eatenPillets;
    public int eatenGhosts => _eatenGhosts;
    
    public int allPeletsCount { get; set; }

    public int score => _score;
    public int lives => _lives;


    private void Awake()
    {
        if (singlton == null) singlton = this;
        else
        {
            Debug.LogError("Попытка создать второй GameManager!");
        }
    }

    private void Start()
    {
        InitializeComponents();
        _timer.Run();
        _lives = _startLives;
        UI.UpdateStats();
    }

    private void InitializeComponents()
    {
        UI = FindObjectOfType<UIManager>();
        _timer = gameObject.AddComponent<GameTimer>();
        _ghostManager = new GhostManager(_ghosts);
        _frightenedMode = new FrightenedMode(_ghostManager, _timer);
    }

    public void StartFrightenedMode()
    {
        if (_frightenedMode.isActive)
        {
            StopAllCoroutines();
        }
        StartCoroutine(FrightenedMode());
    }

    public IEnumerator FrightenedMode()
    {
        _frightenedMode.Run();
        print("Корутина начата: " + Time.time);
        yield return new WaitForSeconds(_frightenedDuration);
        _frightenedMode.Stop();
        print("Корутина закончена: " + Time.time);
    }



    public void AddScore(GameObject food)
    {
        switch (food.tag)
        {
            case "Pellet":
                _score += _pelletBounty;
                _eatenPillets++;
                if (eatenPillets >= allPeletsCount)
                {
                    Win();
                }
                break;
            case "Ghost":
                _score += _ghostBounty;
                _eatenGhosts++;
                UI.UpdateEatenGhosts();
                break;
        }
        
        UI.UpdateScore();
    }

    public void AddGhost(Ghost ghost)
    {
        if(_ghosts == null)
            _ghosts = new List<Ghost>();
        
        _ghosts.Add(ghost);
    }

    public void LoseLife()
    {
        StartCoroutine(LoseLifeCourutine());
    }
    
    private IEnumerator LoseLifeCourutine()
    {
        _lives--;
        _ghostManager.StopGhosts();
        yield return new WaitForSeconds(_deathDelay);
        if (_lives < 0)
        {
            GameOver();
        }
        else
        {
            UI.UpdateLives();
            ResetGame();
        }

    }

    private void GameOver()
    {
        print("GameOver!!!");
        UI.EnableGameOver();
    }

    private void Win()
    {
        print("Win!!!");
        _ghostManager.StopGhosts();
        UI.EnableVictory();
    }

    private void ResetGame()
    {
        _timer.Reset();
        _ghostManager.ResetGhosts();
        pacman.ResetPacman();
    }

}
