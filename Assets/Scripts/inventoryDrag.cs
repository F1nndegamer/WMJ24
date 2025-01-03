using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrag : MonoBehaviour
{
    public GameObject prefab;
    private Vector3 offset;
    private bool isDragging;
    public GameObject clone;
    void Update(){
        if(isDragging && PlayerScript.Instance.SimilationStarted){
            OnMouseUp();
        }
    }
    private void OnMouseDown()
    {
        if (!PlayerScript.Instance.SimilationStarted)
        {
            clone = Instantiate(prefab, transform.position, transform.rotation);
            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0);
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        }

    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            clone.transform.position = GetMouseWorldPosition() + offset;
            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 1);
        }
    }
    private void OnMouseUp()
    {
        isDragging = false;
        if (clone != null)
        {
            InventoryDrag clonedrag = clone.GetComponent<InventoryDrag>();
            Destroy(clonedrag);
            if (SFXManager.instance != null)
                SFXManager.instance.PlaySFX("Place");
        }
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 pointofreturn = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return new Vector3(pointofreturn.x, pointofreturn.y, 0);
    }
}
