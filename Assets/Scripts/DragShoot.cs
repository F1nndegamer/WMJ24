using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float launchSpeed = 10f; 
    public float arrowHeadSize = 0.2f; 

    private Vector2 dragStartPos;
    public Vector2 dragEndPos;
    private bool isDragging = false;
    public Vector2 moveDirection;
    bool hover = false;

    void Start()
    {
        dragStartPos = transform.position;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.grey;
        lineRenderer.endColor = Color.grey;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector2 startPos = transform.position;

        if (hover && Input.GetMouseButtonDown(0) && !GetComponent<PlayerScript>().SimilationStarted)
        {
            dragStartPos = startPos;
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            dragEndPos = GetWorldPoint(Input.mousePosition);
        }

        DrawDragLine(dragStartPos, dragEndPos);

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            moveDirection = (dragStartPos - dragEndPos).normalized * launchSpeed;
            Debug.Log(moveDirection);
            isDragging = false;
        }
    }
    void OnMouseDown(){
        hover = true;
    }
    void OnMouseUp(){
        hover = false;
    }
    private Vector2 GetWorldPoint(Vector3 screenPoint)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        return new Vector2(worldPoint.x, worldPoint.y);
    }
    public void Move(){
        Debug.Log("Move");
        Vector3 dir = new Vector3(moveDirection.x * launchSpeed, moveDirection.y * launchSpeed, 0);
        GetComponent<PlayerScript>().ApplyForce(dir);
        Debug.Log(dir);
    }

    private void DrawDragLine(Vector2 start, Vector2 end)
    {
        if(GetComponent<PlayerScript>().SimilationStarted) return;
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0));
            lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0));
        }
        DrawArrow(end, (start - end).normalized);
    }

    private void DrawArrow(Vector2 position, Vector2 direction)
    {
        if(GetComponent<PlayerScript>().SimilationStarted) return;
        // Create two points for the arrowhead
        Vector2 left = Quaternion.Euler(0, 0, 135) * direction * arrowHeadSize;
        Vector2 right = Quaternion.Euler(0, 0, -135) * direction * arrowHeadSize;

        // Create a new line renderer for the arrowhead
        LineRenderer arrowHead = new GameObject("ArrowHead").AddComponent<LineRenderer>();
        arrowHead.startWidth = 0.05f;
        arrowHead.endWidth = 0.05f;
        arrowHead.positionCount = 3;
        arrowHead.startColor = Color.grey;
        arrowHead.endColor = Color.grey;
        arrowHead.SetPosition(0, new Vector3(position.x, position.y, 0));
        arrowHead.SetPosition(1, new Vector3(position.x + left.x, position.y + left.y, 0));
        arrowHead.SetPosition(2, new Vector3(position.x + right.x, position.y + right.y, 0));
        Destroy(arrowHead.gameObject, 0.1f);
    }
}
