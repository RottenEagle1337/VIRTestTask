using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("MENUS")]
    [SerializeField] private Menu[] menus;

    private bool isPaused;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].isOpened)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpened)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            OpenMenu("Pause");
        }
        else
        {
            Time.timeScale = 1f;
            OpenMenu("Main");
        }
    }

    public void ResetSaves()
    {
        LevelManager.Instance.ResetAllKeys();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
