using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle fullTogg;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle InstantReset;
    [SerializeField] private Toggle PerformanceMode;
    public bool fullScreen = false, lightMode = false;
    [Range(0, 1)]
    public float volume = 1f;
    public CanvasGroup lightModee;
    public AudioSource lm;
    private bool retried = false;
    private Action retryAction;
    public static Settings instance;
    [SerializeField] private GameObject invertObj;

    private void Start()
    {
        if (instance == null) instance = this;
        volumeSlider.onValueChanged.AddListener(SetVol);
        Load();
        InvokeRepeating("Check", 0.1f, 3);
    }

    void Check()
    {
        fullScreen = Screen.fullScreen;
        if (fullScreen != fullTogg.isOn) { fullTogg.isOn = fullScreen; Save(); }

    }

    void Load()
    {
        Screen.fullScreen = fullTogg.isOn = fullScreen = PlayerPrefs.GetInt("full") == 1;

        InstantReset.isOn = PlayerPrefs.GetInt("reset", 0) == 1;
        PerformanceMode.isOn = PlayerPrefs.GetInt("Perf", 0) == 1;

        float vol = PlayerPrefs.GetFloat("volume", 1);
        volumeSlider.onValueChanged.RemoveListener(SetVol);
        volume = vol;
        volumeSlider.value = vol;
        SetVol(vol);
        volumeSlider.onValueChanged.AddListener(SetVol);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("full", fullTogg.isOn ? 1 : 0);
        PlayerPrefs.SetInt("reset", InstantReset.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Perf", PerformanceMode.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.Save();
        print("save called");
    }
    public void SetVolSlider(float vol)
    {
        SetVol(vol);
    }
    public void SetVol(float vol)
    {
        AudioListener.volume = vol;
        volume = vol;
    }
    public void SetFullTrigger(bool val)
    {
        SetFull(val);
    }
    public void SetFull(bool val)
    {
        Screen.fullScreen = val;
        fullScreen = val;
    }

    public void SetLight(GameObject val)
    {
        if (!val.activeSelf)
        {
            val.SetActive(true);
            StartCoroutine(llm());
        }
    }

    private void ApplyAgain()
    {
        retryAction?.Invoke();
    }


    private IEnumerator llm()
    {
        lm.Play();
        lightModee.alpha = 1;
        yield return new WaitForSecondsRealtime(2f);
        for (float a = 1; a > 0; a -= Time.deltaTime / 2f)
        {
            lightModee.alpha = a;
            yield return null;
        }
        lightModee.alpha = 0;
    }
}
