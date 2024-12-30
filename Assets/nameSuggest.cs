using TMPro;
using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;

public class nameSuggest : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

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
        // Create a dictionary to hold the data
        var data = new Dictionary<string, object>
        {
            { "name", _name },
            { "user", n },
            { "timestamp", FieldValue.ServerTimestamp } // Add a server timestamp for reference
        };

        try
        {
            await db.Collection("names").AddAsync(data);
            Debug.Log("Name uploaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error uploading name: {e.Message}");
        }
    }
}
