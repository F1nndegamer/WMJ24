using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonInteractable : MonoBehaviour
{
    public Toggle toggle;
    public Button button;

    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleChanged);
        button.interactable = toggle.isOn; // Set initial state
    }

    void OnToggleChanged(bool isOn)
    {
        button.interactable = isOn;
    }
}
