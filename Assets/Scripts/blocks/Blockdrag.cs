using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;
    public bool isColliding;
    private bool isDragging;
    public bool willdestroy;
    private void OnMouseDown()
    {
            zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if(willdestroy)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = true;
        }
        if (other.gameObject.CompareTag("destroywindow"))
        {
            willdestroy = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
        if (other.gameObject.CompareTag("destroywindow"))
        {
            willdestroy = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
