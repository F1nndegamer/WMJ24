using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Image overlay;
    [SerializeField] private Image loadingImage;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        float progress = 0f;
        yield return new WaitForSecondsRealtime(1f);
        while (progress < 0.05f)
        {
            progress = Mathf.Lerp(progress, 0.105f, Time.unscaledDeltaTime * 5f);
            loadingImage.fillAmount = progress;
            yield return null;
        }

        AsyncOperation loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
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
            overlay.color = new Color(0, 0, 0, t);
            yield return null;
        }
        overlay.color = new Color(0, 0, 0, 1);
        yield return new WaitForSecondsRealtime(0.5f);

        loading.allowSceneActivation = true;
        yield return new WaitUntil(() => loading.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        SceneManager.UnloadSceneAsync(0);
    }
}
