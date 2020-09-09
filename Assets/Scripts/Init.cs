using UnityEngine;
using TMPro;

public static class Init
{
    public static TextMeshProUGUI ScoreText() 
    {
        return GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
    } 
    public static TextMeshProUGUI MovesLeftText()
    {
        return GameObject.Find("MovesText")?.GetComponent<TextMeshProUGUI>();
    }
    public static GameObject GameoverMessage()
    {
        return GameObject.Find("GameOver");
    }
    public static GameObject Menu()
    {
        return GameObject.Find("Menu");
    }
    public static GameObject ExitMenu()
    {
        return GameObject.Find("ExitMenu");
    }
    public static GameObject PauseMenu()
    {
        return GameObject.Find("PauseMenu");
    }
    public static GameObject PauseButton()
    {
        return GameObject.Find("PauseButton");
    }
    public static Game Game()
    {
        return GameObject.Find("GameField")?.GetComponent<Game>();
    }
    public static AudioSource AudioSourse() 
    {
        return  GameObject.Find("AudioManager")?.GetComponent<AudioSource>();
    }
}
