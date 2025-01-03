using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    public Vector3 offset;
    public bool isColliding;
    public bool isDragging;
    private bool willdestroy;
    public bool canMove;
    public bool SimStarted;
    public bool perm = false;
    [SerializeField] float PlaceSpeed = 0.9f;
    private Vector3 targetPos;
    public float targetScale;
    void Start()
    {
        targetPos = transform.position;
        StartCoroutine(Summoned());
        PlayerScript.input.Mouse.canceled += ctx => OnMouseUp();
    }
    private void OnMouseDown()
    {
        if (!PlayerScript.Instance.SimilationStarted)
        {
            if (Input.GetMouseButtonDown(1) && canMove)
            {
                Delete();
                Debug.Log("RMB");
            }
            if (Input.GetMouseButtonDown(0))
            {
                offset = transform.position - GetMouseWorldPosition();
                isDragging = true;
                Debug.Log("LMB");
            }
        }
    }
    private void Update()
    {
        if (PlayerScript.Instance.SimilationStarted)
        {
            canMove = false;
        }
        else
        {
            if (!perm)
            {
                canMove = true;
            }
        }
        if (isDragging && canMove)
        {
            targetPos = GetMouseWorldPosition() + offset;
            transform.position = VectorFixedLerp(transform.position, new Vector3(targetPos.x, targetPos.y, 0), PlaceSpeed);
        }
    }
    public void Delete()
    {
        StartCoroutine(delete());
    }
    private IEnumerator delete()
    {
        if (!canMove) { yield break; }
        transform.tag = "Untagged";
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * Time.unscaledDeltaTime * 2.5f;
            yield return null;
        }
        Destroy(gameObject);
        PlayerScript.Instance.UpdateMagnets();
    }
    private IEnumerator Summoned()
    {
        while (transform.localScale.x > targetScale)
        {
            transform.localScale -= Vector3.one * 2 * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one * targetScale;
    }
    float FixedLerp(float a, float b, float decay)
    {
        a = b + (a - b) * Mathf.Exp(-decay * Time.deltaTime);
        return a;
    }
    Vector3 VectorFixedLerp(Vector3 a, Vector3 b, float decay)
    {
        return new Vector3(FixedLerp(a.x, b.x, decay), FixedLerp(a.y, b.y, decay), FixedLerp(a.z, b.z, decay));
    }
    private void OnMouseUp()
    {
        isDragging = false;
        if (SFXManager.instance != null)
            SFXManager.instance.PlaySFX("Place");
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
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 pointofreturn = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return new Vector3(pointofreturn.x, pointofreturn.y, 0);
    }
}
