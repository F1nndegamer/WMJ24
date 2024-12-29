using Unity.VisualScripting;
using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;
    public bool isColliding;
    private bool isDragging;
    private bool willdestroy;
    public bool canMove;
    public bool SimStarted;
    [SerializeField] float PlaceSpeed = 0.9f;
    private Vector3 targetPos;
    void Start(){
        targetPos = transform.position;
    }
    private void OnMouseDown()
    {
            zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        
    }
    private void Update()
    {
        if(playerscript.Instance.SimilationStarted)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if (isDragging && canMove)
        {
            targetPos = GetMouseWorldPosition() + offset;
            transform.position = VectorFixedLerp(transform.position, targetPos, PlaceSpeed);
        }
    }
    float FixedLerp(float a, float b, float decay){
        a = b + (a - b) * Mathf.Exp(-decay * Time.deltaTime);
        return a;
    }
    Vector3 VectorFixedLerp(Vector3 a, Vector3 b, float decay){
        return new Vector3(FixedLerp(a.x, b.x, decay), FixedLerp(a.y, b.y, decay), FixedLerp(a.z, b.z, decay));
    }
    private void OnMouseUp()
    {
        isDragging = false;
        if (willdestroy)
        {
            Destroy(gameObject);
        }
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
