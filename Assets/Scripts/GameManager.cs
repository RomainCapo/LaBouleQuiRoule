using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool GameFirstRun = true;
    public static bool GameIsPaused = false;
    public static bool GameIsOver = false;

    // Canvas
    public GameObject pauseMenuUI;
    public GameObject gameMenuUI;
    public GameObject mainMenuUI;
    public GameObject gameoverMenu;
    public GameObject gameWinMenu;
    public GameObject levelWinMenu;
    public GameObject levelChooseMenu;

    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI LevelWinText;

    private const int NB_LEVEL = 4;

    private AudioSource backgroundMusic;
    private int levelNb;

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.UnPause();

        levelNb = SaveGame.Load<int>("level") + 1;

        pauseMenuUI.SetActive(false);
        gameoverMenu.SetActive(false);
        gameWinMenu.SetActive(false);
        levelWinMenu.SetActive(false);
        levelChooseMenu.SetActive(false);

        LevelText.SetText("Level " + levelNb);
        
        if(GameFirstRun)
        {
            Time.timeScale = 0f;
            GameFirstRun = false;
        }
        else
        {
            mainMenuUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameIsOver)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void RestartGame()
    {
        GameIsOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();

    }

    public void StartGame()
    {
        if(!SaveGame.Exists("level"))
        {
            SaveGame.Save<int>("level", 0);
        }
        Resume();

        mainMenuUI.SetActive(false);
        gameMenuUI.SetActive(true);
    }

    public void GameOver()
    {
        backgroundMusic.Pause();
        GameIsOver = true;
        gameoverMenu.SetActive(true);
        gameMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void GameWin()
    {
        backgroundMusic.Pause();
        GameIsOver = true;
        gameWinMenu.SetActive(true);
        gameMenuUI.SetActive(false);
        Time.timeScale = 0f;
        SaveGame.Save<int>("level", 0);
    }

    public void EndLevel()
    {
        backgroundMusic.Pause();
        LevelWinText.SetText("Level " + levelNb + " done !");
        GameIsOver = true;
        levelWinMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DisplayLevelMenu()
    {
        mainMenuUI.SetActive(false);
        levelChooseMenu.SetActive(true);
    }

    public void LoadLevel(int level)
    {
        SaveGame.Save<int>("level", level);
        levelChooseMenu.SetActive(false);
        RestartGame();
    }

    public void NextLevel()
    {
        levelWinMenu.SetActive(false);
        GameIsOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        gameoverMenu.SetActive(false);
        gameWinMenu.SetActive(false);
        levelWinMenu.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        backgroundMusic.UnPause();
        pauseMenuUI.SetActive(false);
        gameMenuUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        backgroundMusic.Pause();
        pauseMenuUI.SetActive(true);
        gameMenuUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
