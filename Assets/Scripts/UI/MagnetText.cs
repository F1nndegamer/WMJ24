using TMPro;
using UnityEngine;

public class MagnetText : MonoBehaviour
{
    public Color normal;
    public Color max;

    private TextMeshPro text;

    void Awake()
{
    text = GetComponentInChildren<TextMeshPro>();
}


    void Update()
    {
        int placed = dragmanager.instance.Magnetsplaced;
        int maxAllowed = PlayerScript.Instance.maxMagnets;

        text.text = "Magnets " + placed + '/' + maxAllowed;
        text.color = placed == maxAllowed ? max : normal;
    }
}
