using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    public float attractionSpeed = 8f;
    public float repulsionForce = 5f;
    public float interactionRange = 5f;
    public float minimumDistance = 1f;
    public float dragCoefficient = 0.05f;
    public float maxVelocity = 7f;
    public Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    public GameObject[] attractors;
    public GameObject[] repellers;
    [SerializeField] private SpriteRenderer north;
    public InputActions inputActions;
    public static InputActions.DefaultActions input;

    public bool SimilationStarted;
    public bool RedPole;
    public static PlayerScript Instance;
    private float elapsedTime = 0f;
    public int timeSpentThisLevel = 0;
    public TextMeshProUGUI totalTime, thisTime;
    public Vector3 startPos;
    private GameObject[] Attractor, Repeller;
    [SerializeField] GameObject arrow;
    public float sf, rf, ro; //sf == scaling factor, rf == rotation factor

    public static string IntToTimeString(int timeInMilliseconds)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(timeInMilliseconds);
        return timeSpan.ToString(@"hh\:mm\:ss\.fff");
    }
    public static int TimeStringToInt(string timeString)
    {
        if (System.TimeSpan.TryParseExact(timeString, @"hh\:mm\:ss\.fff", null, out System.TimeSpan timeSpan))
        {
            return (int)timeSpan.TotalMilliseconds;
        }
        throw new System.FormatException("Invalid time format. Expected format: hh:mm:ss.fff");
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("startlevel") == 1)
        {
            totalTime.text = IntToTimeString(Global.time + timeSpentThisLevel);
            thisTime.text = IntToTimeString(timeSpentThisLevel);
        }
        else
        {
            totalTime.gameObject.SetActive(false);
            thisTime.gameObject.SetActive(false);
        }
        if (SimilationStarted)
        {
            AttractOrRepelObjects(true);
            ApplyMovement();
            elapsedTime += Time.deltaTime * 1000f;
            if (elapsedTime >= 1f)
            {
                timeSpentThisLevel += Mathf.FloorToInt(elapsedTime);
                elapsedTime %= 1f;
            }
        }
        else
        {
            AttractOrRepelObjects(false);
        }
        north.color = new Color(1, 1, 1, Mathf.Lerp(north.color.a, RedPole ? 0 : 1, 0.3f));
        Vector2 vel = velocity;
        float direction = Mathf.Atan2(vel.y, vel.x);
        float mag = vel.magnitude;
        Vector3 l = arrow.transform.localScale;
        arrow.transform.localScale = new Vector3(mag * sf, l.y, l.z);
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rf * direction + ro));
    }
    void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        input = inputActions.Default;
        input.Flip.performed += ctx => ChangePole();
        input.Play.performed += ctx => SimStarted();
        input.Fullscreen.performed += ctx => fullscreenfunction();
        input.Pause.performed += ctx => Pause.Instance.PauseToggle();
        input.Restart.performed += ctx => GetComponent<PlayerDeath>().Retry();
        PlayerPrefs.SetInt("lastlevel", Int32.Parse(SceneManager.GetActiveScene().name.Substring(5)));
        PlayerPrefs.Save();
        if (Int32.Parse(SceneManager.GetActiveScene().name.Substring(5)) == 8)
        {
            PlayerPrefs.SetInt("startlevel", 0);
            PlayerPrefs.Save();
        }
        Invoke("StartTime", 0.6f);
        InvokeRepeating("UpdateMagnets", 0.1f, 1f);
    }
    void Start()
    {
        startPos = transform.position;
    }
    public void DeleteMagnets()
    {
        attractors = GameObject.FindGameObjectsWithTag("Attractor");
        repellers = GameObject.FindGameObjectsWithTag("Repeller");
        foreach (GameObject magnet in attractors)
        {
            Destroy(magnet);
        }
        foreach (GameObject magnet in repellers)
        {
            Destroy(magnet);
        }
        transform.position = startPos;
        velocity = Vector2.zero;
        SimilationStarted = false;
        Time.timeScale = 1f;
        Attractor = null;
        Repeller = null;
    }
    private void StartTime()
    {
        Time.timeScale = 1f;
    }
    public void fullscreenfunction()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
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
    private void UpdateMagnets()
    {
        Attractor = GameObject.FindGameObjectsWithTag("Attractor");
        Repeller = GameObject.FindGameObjectsWithTag("Repeller");
    }
    private void AttractOrRepelObjects(bool move)
    {
        if (RedPole)
        {
            attractors = Attractor;
            repellers = Repeller;
        }
        else
        {
            attractors = Repeller;
            repellers = Attractor;
        }
        Vector3 attractionDirection = Vector3.zero;
        Vector3 repulsionDirection = Vector3.zero;
        if (attractors != null)
        {
            foreach (GameObject attractor in attractors)
            {
                BlockDrag blockDrag = attractor.GetComponent<BlockDrag>();
                if (blockDrag != null)
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
        }

        if (repellers != null)
        {
            foreach (GameObject repeller in repellers)
            {
                BlockDrag blockDrag = repeller.GetComponent<BlockDrag>();
                if (blockDrag != null)
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
        }
        Vector3 force = (attractionDirection * attractionSpeed) + (repulsionDirection * repulsionForce);
        if (move)
        {
            ApplyForce(force);
        }
    }

    public void ApplyForce(Vector3 force)
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
