using TMPro;
using UnityEngine;

public class MagnetText : MonoBehaviour
{
    public Color normal;
    public Color max;

    public TextMeshPro text;
    public static MagnetText instance;

    void Awake()
    
{
    instance =  this;
    text = GetComponentInChildren<TextMeshPro>();
}


    void Update()
    {
        int placed = dragmanager.instance.Magnetsplaced;
        int maxAllowed = PlayerScript.Instance.maxMagnets;
        if (text.text != "completed" && text.text != "completed1")

        {
        text.text = "Magnets " + placed + '/' + maxAllowed;
        text.color = placed == maxAllowed ? max : normal;
        }
    }
}
