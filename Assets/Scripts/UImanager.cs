using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UImanager : MonoBehaviour,IPointerClickHandler
{
    public Action OnLevelReset = delegate { };

    #region Singleton
    public static UImanager Instance { get; private set; }

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

    [Header("Score")]
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;
    [SerializeField] Text timeInGame;
    [SerializeField] Text asteroids;
    [SerializeField] Text newBestScore;
    [SerializeField] float congratsDelay = 2f;

    [Header("MainScene")]
    [SerializeField] Text mainBestScore;

    [Header("BoostBar")]
    [SerializeField] Slider boostSlider;
    [SerializeField] Text pressBoost;

    [Header("Prestart")]
    [SerializeField] CanvasGroup prestartMenu;

    [Header("Die")]
    [SerializeField] CanvasGroup dieMenu;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float restartTime = 1f;
    [SerializeField] Text finalScore;
    [SerializeField] Text finalAsteroids;
    [SerializeField] Text finalTotalTime;
    [SerializeField] Text upgradeScore;

    [Header("SettingsPanel")]
    [SerializeField] CanvasGroup settingsPanel;
    [SerializeField] Slider volumeSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Toggle muteToogle;
    [SerializeField] private AudioClip clickSound;

    Spaceship spaceship;

    public void ChangeScore(int value)
    {
        scoreText.text = "SCORE : " + value;
    }
    public void ChangeTime(int value)
    {
        timeInGame.text = "TIME : " + value + "s.";
    }
    public void ChangeAsteroidsCount(int value)
    {
        asteroids.text = "ASTEROIDS : " + value;
    }
    public void ActivatePrestartMenu()
    {
        OnLevelReset();
        ScenesLoader.Instance.LoadNextLevel();
        Time.timeScale = 0;
        prestartMenu.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        if (Input.anyKeyDown)
        {
            prestartMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
            spaceship = FindObjectOfType<Spaceship>();
            spaceship.OnBoost += ChangeBoostSlider;
        }
    }
    public void RestartLevel()
    {
        BestScore();
        OnLevelReset();
        ScenesLoader.Instance.LoadActiveScene();
        Time.timeScale = 1;
        upgradeScore.gameObject.SetActive(false);
        dieMenu.gameObject.SetActive(false);
    }
    public void BestScore()
    {
        bestScoreText.text = "BEST SCORE : " + PlayerPrefs.GetInt("BestScore", 0);
    }
    public void ActivateSettingPanel()
    {
        Time.timeScale = 0;
        settingsPanel.gameObject.SetActive(true);
        volumeSlider.value = AudioManager.Instance.GetMusicVolume() * volumeSlider.maxValue;
        effectSlider.value = AudioManager.Instance.GetEffectsVolume() * effectSlider.maxValue;
    }
    public void CloseSettingPanel()
    {
        Time.timeScale = 1;
        settingsPanel.gameObject.SetActive(false);
    }
    public void MusicVolumeChange()
    {
        AudioManager.Instance.SetMusicVolume(volumeSlider.value / volumeSlider.maxValue);
    }

    public void EffectVolumeChange()
    {
        AudioManager.Instance.SetEffectsVolume(effectSlider.value/effectSlider.maxValue);
    }

    public void MuteAll()
    {
        var volumeState = muteToogle.isOn;
        AudioListener.pause = volumeState;
    }
    public void ShowCongratsText()
    {
        StartCoroutine(ShowCongrats(congratsDelay));
    }
    public void ActivateLoseGamePanel()
    {
        StartCoroutine(EnableLosePanel(waitTime));
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //AudioManager.Instance.PlayEffect(clickSound);
    }

    public void OnClick()
    {
        AudioManager.Instance.PlayEffect(clickSound);
    }

    private void Start()
    {
        SetStartBestScore();
        BestScore();
    }
    private void ChangeBoostSlider()
    {
        //change boost slider value
        boostSlider.value = spaceship.BoostFilling;
        if (boostSlider.value == boostSlider.maxValue)
        {
            pressBoost.enabled = true;
        }
        else
        {
            pressBoost.enabled = false;
        }
    }

    private IEnumerator EnableLosePanel(float delay)
    {
        //open lose panel and set all final score
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        dieMenu.gameObject.SetActive(true);
        finalScore.text = "FINAL SCORE : " + LevelManager.Instance.Score;
        finalAsteroids.text = "ASTEROIDS : " + LevelManager.Instance.AsteroidsCount;
        finalTotalTime.text = "TOTAL TIME : " + LevelManager.Instance.TimeInGame;
        if (LevelManager.Instance.Score > PlayerPrefs.GetInt("BestScore", 0))
        {
            upgradeScore.gameObject.SetActive(true);
        }
    }
    private IEnumerator ShowCongrats(float delay)
    {
        //pop - up inscription when a new record is reached
        newBestScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        newBestScore.gameObject.SetActive(false);
    }
    private void SetStartBestScore()
    {
        mainBestScore.text = "BEST SCORE : " + PlayerPrefs.GetInt("BestScore", 0);
    }

    
}
