using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }
    #endregion

    private Spaceship spaceship;

    [SerializeField] int score = 0;
    [SerializeField] int bestScore = 0;
    [SerializeField] int asteroidsCount = 0;
    [SerializeField] int timeInGame = 0;


    private float timer;
    private bool doublePoints;

    public int Score
    {
        get
        {
            return score;
        }

    }
    public int BestScore
    {
        get
        {
            return bestScore;
        }

    }
    public int AsteroidsCount
    {
        get
        {
            return asteroidsCount;
        }

    }
    public int TimeInGame
    {
        get
        {
            return timeInGame;
        }
    }
    public bool DoublePoints
    {
        get
        {
            return doublePoints;
        }
        set
        {
            doublePoints = value;
        }
    }
    public void SetBestScore()
    {
        bestScore = score;
        UImanager.Instance.BestScore();
        if (bestScore > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
    }
    public void AddPointToScore(int value)
    {
        score += value;
    }
    public void AddAsteroidsCount(int value)
    {
        asteroidsCount += value;
        UImanager.Instance.ChangeAsteroidsCount(asteroidsCount);
    }

    private void Start()
    {
        spaceship = FindObjectOfType<Spaceship>();
        doublePoints = false;
        ResetScores();
        UImanager.Instance.OnLevelReset += ResetScores;
    }
    private void Update()
    {
        ScorePoints();
        ScoreTime();
    }
    private void ScorePoints()
    {
        timer += 1 * Time.deltaTime;
        if (timer >= 1)
        {
            //add points to score depending on the speed of movement
            if (DoublePoints == false)
            {
                score += 1;
                timer = 0;
                UImanager.Instance.ChangeScore(Score);
                if (score > PlayerPrefs.GetInt("BestScore", 0))
                {
                    UImanager.Instance.ShowCongratsText();
                }
            }
            else if (DoublePoints)
            {
                score += 2;
                timer = 0;
                UImanager.Instance.ChangeScore(Score);
                if (score > PlayerPrefs.GetInt("BestScore", 0))
                {
                    UImanager.Instance.ShowCongratsText();
                }
            }


        }
    }
    private void ScoreTime()
    {
        //adds 1 point every second
        timer += 1 * Time.deltaTime;
        if (timer >= 1)
        {
            timeInGame += 1;
            timer = 0;
            UImanager.Instance.ChangeTime(timeInGame);
        }
    }
    private void ResetScores()
    {
        StartCoroutine(ResetScoreWithDelay());
    }
    
    private IEnumerator ResetScoreWithDelay() 
    {
        //reset score with delay
        yield return new WaitForSeconds(0.3f);
        score = 0;
        bestScore = 0;
        asteroidsCount = 0;
        timeInGame = 0;
    }
}
