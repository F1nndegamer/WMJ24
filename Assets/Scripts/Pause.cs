using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public bool Paused;
    public string MainMenuName;
    public static Pause Instance;
    void Awake()
    {
        Instance = this;
    }
    public void PauseToggle()
    {
        if (Paused)
        {
            Paused = false;
            PauseScreen.GetComponent<Animator>().SetBool("show", false);
            Time.timeScale = 1f;
        }
        else
        {
            Paused = true;
            PauseScreen.GetComponent<Animator>().SetBool("show", true);
            Time.timeScale = 0f;
        }
    }

    public void Play_Again()
    {
        if (PlayerDeath.Instance != null)
        {
            PlayerDeath.Instance.loadLevel(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("PlayerDeath.Instance is not set!");
        }
    }

    public void BackToMenu()
    {
        if (PlayerDeath.Instance != null)
        {
            PlayerDeath.Instance.BackToMenu();
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("PlayerDeath.Instance is not set!");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
