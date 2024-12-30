using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public GameObject WinScreen;
    public string MainMenuName;
    void Start()
    {
        if(WinScreen != null)
        {
            WinScreen.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Win());
        }
    }
    private IEnumerator Win()
    {
        WinScreen.SetActive(true);
        PlayerScript.Instance.SimilationStarted = false;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0f;
    }
    public void Play_Again()
    {
        PlayerDeath.Instance.loadLevel(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void BackToMenu()
    {
        PlayerDeath.Instance.BackToMenu();
        Time.timeScale = 1f;

    }
    public void Next_Level()
    {
        PlayerDeath.Instance.NextLevel();
        Time.timeScale = 1f;
    }
}
