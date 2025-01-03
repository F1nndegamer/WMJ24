using System;
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
        if (WinScreen != null)
        {
            WinScreen.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Win());
            int currentLevel;
            string levelName = SceneManager.GetActiveScene().name;
            currentLevel = Int32.Parse(levelName.Substring(5));
            Debug.Log(currentLevel);
            PlayerPrefs.SetInt("levels", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
    private IEnumerator Win()
    {
        if(SFXManager.instance != null)
            SFXManager.instance.PlaySFX("Win");
        WinScreen.SetActive(true);
        WinScreen.GetComponent<Animator>().SetBool("show", true);
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
        PlayerDeath.Instance.NextLevel(false);
        Time.timeScale = 1f;
    }
}
