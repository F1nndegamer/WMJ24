using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private CanvasGroup overlay;
    [SerializeField] private Image loadingImage;
    [SerializeField] private bool loadingSmth = false;
    public static Menu Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SFXManager.instance.PlaySFX("Click");
        }
    }
    private void Start()
    {
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
    }
    public void loadLevel(string name)
    {
        if (!loadingSmth)
        {
            loadingSmth = true;
            try
            {
                if (PlayerPrefs.GetInt("lastlevel") >= Int32.Parse(name.Substring(5)))
                {
                    PlayerPrefs.SetInt("startlevel", Int32.Parse(name.Substring(5)));
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("startlevel", 1);
                    PlayerPrefs.Save();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            StartCoroutine(LoadLevel(name));
        }
    }
    public void exit()
    {
        if (!loadingSmth)
        {
            loadingSmth = true;
            StartCoroutine(Exit());
        }
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
        Application.OpenURL("https://alimadcorp.github.io/secret/wmj24.html");
        Application.Quit();
    }
    private IEnumerator LoadLevel(string name)
    {
        float progress = 0f;
        overlay.gameObject.SetActive(true);
        overlay.alpha = 0;
        yield return new WaitForSecondsRealtime(1f);
        while (progress < 0.05f)
        {
            progress = Mathf.Lerp(progress, 0.105f, Time.unscaledDeltaTime * 5f);
            loadingImage.fillAmount = progress;
            yield return null;
        }
        int lastScene = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation loading = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        loading.allowSceneActivation = false;
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
        yield return new WaitForSecondsRealtime(0.7f);
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
        yield return new WaitForSecondsRealtime(0.5f);

        loading.allowSceneActivation = true;
        yield return new WaitUntil(() => loading.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        loadingSmth = false;
        SceneManager.UnloadSceneAsync(lastScene);
    }
}
