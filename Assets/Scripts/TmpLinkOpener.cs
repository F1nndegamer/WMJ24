using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMPLinkOpener : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, Camera.main);
        if (linkIndex != -1)
        {
            Debug.Log("click1");
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID();
            Application.OpenURL(url);
        }
    }
}
