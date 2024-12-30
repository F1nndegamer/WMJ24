using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public bool Paused;
    public string MainMenuName;

    void Start()
    {
        if (PauseScreen != null)
        {
            PauseScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseScreen is not set in the Inspector!");
        }
    }

    private void Update()
    {
                PauseScreen.transform.position = new Vector3(14, 33, 0);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Paused = false;
                PauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Paused = true;
                PauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void Play_Again()
    {
        if (PlayerDeath.Instance != null)
        {
            PlayerDeath.Instance.loadLevel(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f; // Ensure time resumes
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
