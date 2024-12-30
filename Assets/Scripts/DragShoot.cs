using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    public LineRenderer lineRenderer; // Line Renderer to visualize the drag direction
    public float launchSpeed = 10f;  // Adjust the shooting speed

    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private bool isDragging = false;
    private bool isMoving = false;
    private Vector2 moveDirection;
    private bool dragStartedOnPlayer = false;

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Vector2 startPos = transform.position;
            if (Vector2.Distance(startPos, transform.position) <= 0.5f)
            {
                dragStartPos = startPos;
                isDragging = true;
                dragStartedOnPlayer = true;
                lineRenderer.positionCount = 2;
            }
        }

        if (isDragging && dragStartedOnPlayer && Input.GetMouseButton(0))
        {
            dragEndPos = GetWorldPoint(Input.mousePosition);
        }

        if (isDragging && dragStartedOnPlayer && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            lineRenderer.positionCount = 0;

            if (dragStartedOnPlayer)
            {
                moveDirection = (dragStartPos - dragEndPos).normalized * launchSpeed;
                isMoving = true;
            }

            dragStartedOnPlayer = false;
        }
        DrawDragLine(dragStartPos, dragEndPos);

        if (isMoving && PlayerScript.Instance.SimilationStarted)
        {
            MoveObject();
        }
    }

    private Vector2 GetWorldPoint(Vector3 screenPoint)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        return new Vector2(worldPoint.x, worldPoint.y);
    }

    private void DrawDragLine(Vector2 start, Vector2 end)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0));
            lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0));
        }
    }

    private void MoveObject()
    {
        transform.position += (Vector3)moveDirection * Time.deltaTime;
    }
}
