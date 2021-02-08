using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private Text _scoreCount;
    [SerializeField] private Text _livesCount;
    [SerializeField] private Text _eatenGhostsCount;
    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _victory;
    

    
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = GameManager.singlton;
        _gameOver.gameObject.SetActive(false);
        _victory.gameObject.SetActive(false);
    }

    public void UpdateStats()
    {
        UpdateScore();
        UpdateLives();
    }
    
    public void UpdateScore()
    {
        int score = _gameManager.score;
        if (score < 10) _scoreCount.text = "00000" + score;
        else if (score < 100) _scoreCount.text = "0000" + score;
        else if (score < 1000) _scoreCount.text = "000" + score;
        else if (score < 10000) _scoreCount.text = "00" + score;
        else if (score < 100000) _scoreCount.text = "0" + score;
        else _scoreCount.text = score.ToString();
    }

    public void UpdateLives()
    {
        int lives = _gameManager.lives;
        _livesCount.text = "x" + lives;
    }

    public void UpdateEatenGhosts()
    {
        int eatenGhosts = _gameManager.eatenGhosts;
        _eatenGhostsCount.text = "x" + eatenGhosts;
    }

    public void EnableGameOver()
    {
        _gameOver.gameObject.SetActive(true);
    }
    
    public void EnableVictory()
    {
        _victory.gameObject.SetActive(true);
    }
    
}
