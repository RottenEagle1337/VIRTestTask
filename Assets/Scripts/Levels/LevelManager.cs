using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private bool[] levelsPassed;

    private Level[] levels;

    private bool levelPassed;
    private int prevSceneIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        prevSceneIndex = 0;

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            levels = GameObject.FindGameObjectWithTag("Levels").GetComponentsInChildren<Level>();
            levelsPassed = new bool[levels.Length - 1];

            for (int i = 1; i < levels.Length; i++)
            {
                if (!PlayerPrefs.HasKey("level_" + i.ToString()))
                {
                    PlayerPrefs.SetInt("level_" + i.ToString(), 0);
                    levelsPassed[i - 1] = false;
                }
                else if (PlayerPrefs.GetInt("level_" + i.ToString()) == 0)
                {
                    levelsPassed[i - 1] = false;
                }
                else
                {
                    levelsPassed[i - 1] = true;
                }
            }

            UpdateLevels();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (prevSceneIndex != 0 && levelPassed)
        {
            levelsPassed[prevSceneIndex - 1] = levelPassed;
            PlayerPrefs.SetInt("level_" + prevSceneIndex.ToString(), 1);
        }

        if (scene.buildIndex == 0)
        {
            levels = GameObject.FindGameObjectWithTag("Levels").GetComponentsInChildren<Level>();
            UpdateLevels();
        }


        prevSceneIndex = scene.buildIndex;
    }

    private void UpdateLevels()
    {
        for (int i = 1; i < levels.Length; i++)
        {
            if (levelsPassed[i - 1])
            {
                levels[i].ShowStar();
            }
        }
    }

    public void PassedCurrentLevel(bool passedCurrentLevel)
    {
        levelPassed = passedCurrentLevel;
    }

    public void ResetAllKeys()
    {
        PlayerPrefs.DeleteAll();

        for(int i = 0; i < levelsPassed.Length; i++)
        {
            levelsPassed[i] = false;
            levels[i + 1].HideStar();
        }
    }
}
