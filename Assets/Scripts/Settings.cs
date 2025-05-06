using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle fullTogg;
    [SerializeField] private Slider volumeSlider;
    public bool fullScreen = false, lightMode = false;
    [Range(0, 1)]
    public float volume = 1f;
    public CanvasGroup lightModee; public AudioSource lm;

    [SerializeField] private GameObject invertObj;
    private void Start()
    {
        Load();
        InvokeRepeating("Check", 0.1f, 3);
    }
    void Update()
    {

    }
    void Check()
    {
        fullScreen = Screen.fullScreen;
        if (fullScreen != fullTogg.isOn) fullTogg.isOn = fullScreen;
        Save();
    }
    void Load()
    {
        Screen.fullScreen = fullTogg.isOn = fullScreen = PlayerPrefs.GetInt("full") == 1;
        AudioListener.volume = volumeSlider.value = volume = PlayerPrefs.GetFloat("volume", 1);
    }
    void Save()
    {
        PlayerPrefs.SetInt("full", fullTogg.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
    public void SetVol(float vol)
    {
        AudioListener.volume = vol;
        volume = vol;
    }
    public void SetFull(bool val)
    {
        Screen.fullScreen = val;
        fullScreen = val;
    }
    public void SetLight(bool val)
    {
        StartCoroutine(llm());
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
