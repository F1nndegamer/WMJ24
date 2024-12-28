using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Screen Settings")]
    public GameObject deathScreen;
    public string MainMenuName;

    void Start()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
            deathScreen.SetActive(true);
    }

    public void Retry()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuName);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
