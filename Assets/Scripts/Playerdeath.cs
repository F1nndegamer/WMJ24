using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Screen Settings")]
    public GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI deathmsg;
    [SerializeField] private Image loadingImage;
    [SerializeField] private GameObject skip;

    [SerializeField] private CanvasGroup overlay;
    private int c_level;
    public static PlayerDeath Instance;
    private bool loadingSmth = false;
    public string MainMenuName;
    private void Start()
    {
        Instance = this;
        StartCoroutine(InTransition());
    }
    private IEnumerator InTransition()
    {
        overlay.gameObject.SetActive(true);
        for (float t = 1; t > 0; t -= Time.unscaledDeltaTime)
        {
            overlay.alpha = t;
            yield return null;
        }
        overlay.alpha = 0;
        overlay.gameObject.SetActive(false);
        c_level = Int32.Parse(SceneManager.GetActiveScene().name.Substring(5));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Die("Depolarized"));

            if (SFXManager.instance != null)
                SFXManager.instance.PlaySFX("Burn");

            Vector3 pos = collision.contacts[0].point;
            ParticleSystem system = Instantiate(Resources.Load("Explode") as GameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
            system.Play();
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            StartCoroutine(Die("Out of Border"));

            if (SFXManager.instance != null)
                SFXManager.instance.PlaySFX("Bounds");

            Vector3 pos = collision.contacts[0].point;
            ParticleSystem system = Instantiate(Resources.Load("Explode") as GameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
            system.Play();
        }
    }

    IEnumerator Die(string message)
    {
        deathScreen.SetActive(true);
        PlayerScript.Instance.SimilationStarted = false;
        deathmsg.text = message;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0f;

        deathScreen.SetActive(true);
        deathScreen.GetComponent<Animator>().SetBool("show", true);
        int currentLevel = Int32.Parse(SceneManager.GetActiveScene().name.Substring(5));
        Global.attempts[currentLevel - 1]++;
        if (Global.attempts[currentLevel - 1] > 2)
        {
            skip.SetActive(true);
        }
    }
    public void NextLevel(bool skip)
    {
        int currentLevel;
        string levelName = SceneManager.GetActiveScene().name;
        currentLevel = Int32.Parse(levelName.Substring(5));
        if (PlayerPrefs.GetInt("levels") < currentLevel + 1)
        {
            PlayerPrefs.SetInt("levels", currentLevel + 1);
            PlayerPrefs.Save();
        }
        if (currentLevel < Global.times.Length)
        {
            if (skip) { Global.times[c_level - 1] += 30000; }
            Global.times[c_level - 1] += Mathf.FloorToInt(GetComponent<PlayerScript>().timeSpentThisLevel);
            loadLevel("Level" + (currentLevel + 1).ToString());
            PlayerPrefs.SetInt("ls" + c_level.ToString(), Global.times[c_level - 1]);
            PlayerPrefs.Save();
        }
        else
        {
            if (skip) { Global.times[c_level - 1] += 30000; }
            Global.times[c_level - 1] += Mathf.FloorToInt(GetComponent<PlayerScript>().timeSpentThisLevel);
            loadLevel("Leaderboard");
            PlayerPrefs.SetInt("ls" + c_level.ToString(), Global.times[c_level - 1]);
            PlayerPrefs.Save();
        }
        if (Int32.Parse(SceneManager.GetActiveScene().name.Substring(5)) == Global.times.Length)
        {
            PlayerPrefs.SetInt("startlevel", 0);
            PlayerPrefs.Save();
        }
    }
    public void Retry()
    {
        StartCoroutine(GetComponent<PlayerScript>().moveback());
        GetComponent<PlayerScript>().velocity = Vector2.zero;
        PlayerScript.Instance.SimilationStarted = false;
        Time.timeScale = 1f;
        PlayerScript.Instance.Retry();
    }
    public void BackToMenu()
    {
        loadLevel("Menu");
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        StartCoroutine(Exit());
    }
    private IEnumerator Exit()
    {
        overlay.gameObject.SetActive(true);
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
        {
            overlay.alpha = t;
            yield return null;
        }
        overlay.alpha = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        Application.Quit();
    }
    public void loadLevel(string name) { if (!loadingSmth) { StartCoroutine(LoadLevel(name)); } }
    private IEnumerator LoadLevel(string name)
    {
        Time.timeScale = 1;
        loadingSmth = true;
        float progress = 0f;
        overlay.gameObject.SetActive(true);
        overlay.alpha = 0;
        yield return new WaitForSecondsRealtime(0.2f);
        while (progress < 0.05f)
        {
            progress = Mathf.Lerp(progress, 0.105f, Time.unscaledDeltaTime * 5f);
            loadingImage.fillAmount = progress;
            yield return null;
        }
        int lastScene = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation loading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

        while (loading.progress < 0.9f)
        {
            if (progress < loading.progress)
            {
                progress = Mathf.Lerp(progress, loading.progress, Time.unscaledDeltaTime);
            }
            loadingImage.fillAmount = progress;
            yield return null;
        }
        while (progress < 0.98f)
        {
            progress = Mathf.Lerp(progress, 1, Time.unscaledDeltaTime * 3f);
            loadingImage.fillAmount = progress;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.3f);
        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 2f)
        {
            progress = Mathf.Lerp(progress, 1, Time.unscaledDeltaTime);
            loadingImage.fillAmount = progress;
            yield return null;
        }
        progress = 1f;
        loadingImage.fillAmount = progress;
        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 2f)
        {
            overlay.alpha = t;
            yield return null;
        }
        overlay.alpha = 1;
        yield return new WaitForSecondsRealtime(0.2f);

        loading.allowSceneActivation = true;
        yield return new WaitUntil(() => loading.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        loadingSmth = false;
        SceneManager.UnloadSceneAsync(lastScene);
    }
}
