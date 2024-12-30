using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public bool Paused;
    public Animator animator;
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

        if (animator == null)
        {
            Debug.LogError("Animator is not set in the Inspector!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Paused = false;
                StartCoroutine(HidePause());
            }
            else
            {
                Paused = true;
                StartCoroutine(ShowPause());
            }
        }
    }

    private IEnumerator HidePause()
    {
        if (animator == null || PauseScreen == null) yield break;

        animator.SetBool("hide", true);
        animator.SetBool("show", false);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for the animation to finish
        PauseScreen.SetActive(false);
        Time.timeScale = 1f; // Resume game time
    }

    private IEnumerator ShowPause()
    {
        if (animator == null || PauseScreen == null) yield break;

        PauseScreen.SetActive(true);
        animator.SetBool("hide", false);
        animator.SetBool("show", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for the animation to finish
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
