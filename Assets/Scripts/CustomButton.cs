using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TransferButtonEvents : MonoBehaviour, IPointerDownHandler
{
    private Button button;
    private Button.ButtonClickedEvent onClickEvents;
    [SerializeField] float delay;
    //This is a script which will call the operation the frame the button gets clicked down instead of waiting for a release
    private void Start()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.Log("Mr Button does not exist");
            return;
        }
        onClickEvents = button.onClick;
        button.onClick = new Button.ButtonClickedEvent();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(InvokeIt());
    }
    public IEnumerator InvokeIt(){
        yield return new WaitForSecondsRealtime(delay);
        if (onClickEvents != null)
        {
            onClickEvents.Invoke();
        }
    }
}
