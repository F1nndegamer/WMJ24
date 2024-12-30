using TMPro;
using UnityEngine;

public class nameSuggest : MonoBehaviour
{

    void Start()
    {

        string name = PlayerPrefs.GetString("gamename");
        if (!string.IsNullOrEmpty(name))
        {
            GetComponent<TMP_InputField>().text = name;
        }
    }

    public void Suggest()
    {
        string name = GetComponent<TMP_InputField>().text;
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString("gamename", name);
            PlayerPrefs.Save();
            GetComponent<TMP_InputField>().text = name;

            UploadName(name);
        }
    }

    private async void UploadName(string _name)
    {
        if (string.IsNullOrEmpty(_name)) return;
        string n = PlayerPrefs.GetString("name");
        
    }
}
