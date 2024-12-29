using UnityEngine;
public class PlayerScript : MonoBehaviour
{
    public float attractionSpeed = 8f;
    public float repulsionForce = 5f;
    public float interactionRange = 5f;
    public float minimumDistance = 1f;
    public float dragCoefficient = 0.05f;
    public float maxVelocity = 7f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    public GameObject[] attractors;
    public GameObject[] repellers;
    [SerializeField] private SpriteRenderer north;
    public InputActions inputActions;
    public static InputActions.DefaultActions input;

    public bool SimilationStarted;
    public bool RedPole;
    public static PlayerScript Instance;
    private void Update()
    {
        if (SimilationStarted)
        {
            AttractOrRepelObjects();
            ApplyMovement();
        }
        north.color = new Color(1, 1, 1, Mathf.Lerp(north.color.a, RedPole ? 0 : 1, 0.3f));
    }
    void Awake(){
        inputActions = new InputActions();
        input = inputActions.Default;
        input.Flip.performed += ctx => ChangePole();
    }
    private void OnEnable() {
        input.Enable();
    }
    private void OnDisable() {
        input.Disable();
    }
    private void Start()
    {
        Instance = this;
    }
    public void SimStarted()
    {
        SimilationStarted = true;
    }
    public void SimStop()
    {
        SimilationStarted = false;
    }
    public void ChangePole()
    {
        if (RedPole)
        {
            RedPole = false;
        }
        else
        {
            RedPole = true;
        }
    }
    private void AttractOrRepelObjects()
    {
        if(RedPole)
        {
            attractors = GameObject.FindGameObjectsWithTag("Attractor");
            repellers = GameObject.FindGameObjectsWithTag("Repeller");
        }
        else
        {
            attractors = GameObject.FindGameObjectsWithTag("Repeller");
            repellers = GameObject.FindGameObjectsWithTag("Attractor");
        }
        Vector3 attractionDirection = Vector3.zero;
        Vector3 repulsionDirection = Vector3.zero;

        foreach (GameObject attractor in attractors)
        {
            BlockDrag blockDrag = attractor.GetComponent<BlockDrag>();
            if (blockDrag != null && !blockDrag.isColliding)
            {
                float distanceToAttractor = Vector3.Distance(transform.position, attractor.transform.position);
                if (distanceToAttractor <= interactionRange && distanceToAttractor > minimumDistance)
                {
                    Vector3 directionToAttractor = (attractor.transform.position - transform.position).normalized;
                    float forceMultiplier = Mathf.Lerp(1f, 0.5f, distanceToAttractor / interactionRange);
                    attractionDirection += directionToAttractor * forceMultiplier / Mathf.Max(distanceToAttractor, 1f);
                }

            }
        }

        foreach (GameObject repeller in repellers)
        {
            BlockDrag blockDrag = repeller.GetComponent<BlockDrag>();
            if (blockDrag != null && !blockDrag.isColliding)
            {
                float distanceToRepeller = Vector3.Distance(transform.position, repeller.transform.position);
                if (distanceToRepeller <= interactionRange && distanceToRepeller > minimumDistance)
                {
                    Vector3 directionToRepeller = (transform.position - repeller.transform.position).normalized;
                    float forceMultiplier = Mathf.Lerp(1f, 0.3f, distanceToRepeller / interactionRange);
                    repulsionDirection += directionToRepeller * forceMultiplier / Mathf.Max(distanceToRepeller, 1f);
                }
            }
        }

        Vector3 force = (attractionDirection * attractionSpeed) + (repulsionDirection * repulsionForce);
        ApplyForce(force);
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration = force;
    }

    private void ApplyMovement()
    {
        velocity += acceleration * Time.deltaTime;
        velocity *= 1 - dragCoefficient * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (RedPole)
        {
            attractors = GameObject.FindGameObjectsWithTag("Attractor");
            repellers = GameObject.FindGameObjectsWithTag("Repeller");
        }
        else
        {
            attractors = GameObject.FindGameObjectsWithTag("Repeller");
            repellers = GameObject.FindGameObjectsWithTag("Attractor");
        }
        Gizmos.color = Color.blue;
        foreach (GameObject attractor in attractors)
        {
            float distanceToAttractor = Vector3.Distance(transform.position, attractor.transform.position);
            if (distanceToAttractor <= interactionRange && distanceToAttractor > minimumDistance)
            {
                Gizmos.DrawLine(transform.position, attractor.transform.position);
            }
        }

        Gizmos.color = Color.red;
        foreach (GameObject repeller in repellers)
        {
            float distanceToRepeller = Vector3.Distance(transform.position, repeller.transform.position);
            if (distanceToRepeller <= interactionRange && distanceToRepeller > minimumDistance)
            {
                Gizmos.DrawLine(transform.position, repeller.transform.position);
            }
        }
    }
}
