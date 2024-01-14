using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void LoadSteamHost()
    {
        SceneManager.LoadScene("SteamGame");
    }

    public void LoadLocalHost()
    {
        SceneManager.LoadScene("LocalHostGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}