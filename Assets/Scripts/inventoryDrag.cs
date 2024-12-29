using System.Collections;
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
        if(!PlayerScript.Instance.SimilationStarted)
            {
            clone = Instantiate(prefab, transform.position, transform.rotation);
            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 1);
            clone.GetComponent<BlockDrag>().offset = transform.position - GetMouseWorldPosition();
            clone.GetComponent<BlockDrag>().isDragging = true;
            StartCoroutine(onDragged());
        }

    }
    private IEnumerator onDragged(){
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.clear;
        while(renderer.color.a < 1){
            renderer.color = new Color(1, 1, 1, renderer.color.a + Time.deltaTime * 2);
            yield return null;
        }
        renderer.color = Color.white;
    }
    private void OnMouseUp()
    {
        if (clone != null)
        {
            InventoryDrag clonedrag = clone.GetComponent<InventoryDrag>();
            Destroy(clonedrag);
        }
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
