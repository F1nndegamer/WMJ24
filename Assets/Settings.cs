using System;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle fullTogg, lightTogg;
    [SerializeField] private Slider volumeSlider;
    public bool fullScreen = false, lightMode = false;
    [Range(0, 1)]
    public float volume = 1f;

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
        Debug.Log("Check()");
        Save();
    }
    void Load()
    {
        Screen.fullScreen = fullTogg.isOn = fullScreen = PlayerPrefs.GetInt("full") == 1;
        invertObj.SetActive(lightTogg.isOn = lightMode = PlayerPrefs.GetInt("light") == 1);
        AudioListener.volume = volumeSlider.value = volume = PlayerPrefs.GetFloat("volume");
        Debug.Log("Loaded");
    }
    void Save()
    {
        PlayerPrefs.SetInt("full", fullTogg.isOn ? 1 : 0);
        PlayerPrefs.SetInt("light", lightTogg.isOn ? 1 : 0);
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
        lightMode = val;
        invertObj.SetActive(val);
    }
}
