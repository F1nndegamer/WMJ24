using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrag : MonoBehaviour
{
    public GameObject prefab;
    private Vector3 offset;
    private float zCoordinate;
    private bool isDragging;
    public GameObject clone;
    private void OnMouseDown()
    {
        clone = Instantiate(prefab, transform.position, transform.rotation); 
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 1);
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;

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
        InventoryDrag clonedrag = clone.GetComponent<InventoryDrag>();
        Destroy(clonedrag);
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
