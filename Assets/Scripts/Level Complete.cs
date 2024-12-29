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
        if (collision.gameObject.CompareTag("Player"));
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuName);
        Time.timeScale = 1f;

    }
    public void Next_Level()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        string numberPart = System.Text.RegularExpressions.Regex.Match(sceneName, @"\d+").Value;

        if (!string.IsNullOrEmpty(numberPart))
        {
            int sceneNumber = int.Parse(numberPart);
            int nextScene = sceneNumber + 1;
            Menu.Instance.loadLevel("Level" + nextScene);
        }
        else
        {
            Debug.Log("no number found in the scene name");
        }
    }
}
