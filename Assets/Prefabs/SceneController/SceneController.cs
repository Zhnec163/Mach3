using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private GameObject menu;
    private GameObject exitMenu;

    private GameObject pauseMenu;
    private GameObject pauseButton;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            menu = Init.Menu();
            exitMenu = Init.ExitMenu();
            exitMenu.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            pauseMenu = Init.PauseMenu();
            pauseButton = Init.PauseButton();
            pauseMenu.SetActive(false);
        }
    }
    public void GoToGameScene() => SceneManager.LoadScene("Game");
    public void GoToScoreScene() => SceneManager.LoadScene("Score");
    public void GoToAboutTheProgramScene() => SceneManager.LoadScene("AboutTheProgram");
    public void GoToMenuScene() => SceneManager.LoadScene("Menu");
    public void ExitMenu(bool off) 
    {
        if (off)
        {
            exitMenu.SetActive(false);
            menu.SetActive(true);
        }
        else
        {
            exitMenu.SetActive(true);
            menu.SetActive(false);
        }
    }
    public void CloseApplication() 
    {
        Application.Quit();
    }
    public void PauseMenu(bool pause) 
    {
        if (pause)
        {
            pauseMenu.SetActive(true);
            pauseButton.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(false);
            pauseButton.SetActive(true);
        }
    }
    public void LinkDeveloper() 
    {
        Application.OpenURL("https://vk.com/durov");
    }
    public void Music() 
    {
        AudioManager.instance.Music();
    }
}
