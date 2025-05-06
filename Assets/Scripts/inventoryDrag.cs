using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrag : MonoBehaviour
{
    public GameObject prefab;
    private Vector3 offset;
    private bool isDragging;
    public GameObject clone;
    public static InventoryDrag instance;
    void Start()
    {
        instance = this;
    }
    void Update()
    {
        if (isDragging && PlayerScript.Instance.SimilationStarted)
        {
            OnMouseUp();
        }
    }
    private void OnMouseDown()
    {
        float screenEdgeThreshold = 50f;

        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 0));
            worldPos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            bool isTouchNearEdge = touchPos.x < screenEdgeThreshold || touchPos.x > Screen.width - screenEdgeThreshold;

            if (isTouchNearEdge && (hit == null || hit.gameObject != gameObject))
                return;
        }

        if (!PlayerScript.Instance.SimilationStarted && PlayerScript.Instance.maxMagnets > dragmanager.instance.Magnetsplaced)
        {
            dragmanager.instance.Magnetsplaced++;
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
            BlockDrag blockDrag = clone.GetComponent<BlockDrag>();
            if (blockDrag.willdestroy)
            {
                dragmanager.instance.Magnetsplaced -= 1;
                Destroy(clone);
                return;
            }
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
