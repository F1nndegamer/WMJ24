using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Collections;
public class PlayerScript : MonoBehaviour
{
    public float attractionSpeed = 8f;
    public float repulsionForce = 5f;
    public float interactionRange = 5f;
    public float minimumDistance = 1f;
    public float dragCoefficient = 0.05f;
    public float maxVelocity = 7f;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
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
    public Sprite play, pause;
    public Image playButton;
    public Animator winS, loseS;
    public Light2D mlight;
    public Color red, blue;
    private bool paused = false;
    private float arrow_dir = 0; private float arrow_scale = 0;
    private int lastTotalTime = 0; private int c_level = 1;
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
        if (!Global.paused)
        {
            Time.timeScale = 1f;
        }
        totalTime.text = IntToTimeString(lastTotalTime + timeSpentThisLevel);
        thisTime.text = IntToTimeString(timeSpentThisLevel);

        if (SimilationStarted && !paused)
        {
            AttractOrRepelObjects(true);
            ApplyMovement(true);
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
            ApplyMovement(false);
        }
        north.color = new Color(1, 1, 1, Mathf.Lerp(north.color.a, RedPole ? 0 : 1, 0.3f));
        float t = north.color.a;
        mlight.color = Color.Lerp(blue, red, t);
        Vector2 vel = new Vector2();
        vel.x = acceleration.x; vel.y = acceleration.y;
        float direction = Mathf.Atan2(vel.y, vel.x);
        float mag = vel.magnitude;
        Vector3 l = arrow.transform.localScale;
        arrow_scale = l.x;
        arrow_dir = arrow.transform.localRotation.eulerAngles.z;
        arrow.transform.localScale = new Vector3(Mathf.Lerp(arrow_scale, mag * sf, Time.unscaledDeltaTime * 8f), l.y, l.z);
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(arrow_dir, rf * direction + ro, Time.unscaledDeltaTime * 8f)));
    }
    void Awake()
    {
        Instance = this;
        maxVelocity = 20f;
        inputActions = new InputActions();
        input = inputActions.Default;
        input.Flip.performed += ctx => ChangePole();
        input.Play.performed += ctx => SimToggle();
        input.Fullscreen.performed += ctx => fullscreenfunction();
        input.Pause.performed += ctx => Pause.Instance.PauseToggle();
        input.Restart.performed += ctx => GetComponent<PlayerDeath>().Retry();
        Invoke("StartTime", 1.1f);
        InvokeRepeating("UpdateMagnets", 0.1f, 1f);
    }
    public void Retry()
    {
        winS.SetBool("show", false);
        loseS.SetBool("show", false);
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        SimilationStarted = false;
        paused = false;
        Time.timeScale = 1f;
    }
    public IEnumerator moveback()
    {
        while ((transform.position - startPos).magnitude > 0.1f)
        {
            transform.position = VectorFixedLerp(transform.position, startPos, 30);
            yield return null;
        }
        timeSpentThisLevel = 0;
        transform.position = startPos;
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
    void Start()
    {
        Camera.main.gameObject.transform.position -= new Vector3(0, 0, 290);
        startPos = transform.position;
    }
    public void DeleteMagnets()
    {
        attractors = GameObject.FindGameObjectsWithTag("Attractor");
        repellers = GameObject.FindGameObjectsWithTag("Repeller");
        foreach (GameObject magnet in attractors)
        {
            magnet.GetComponent<BlockDrag>()?.Delete();
        }
        foreach (GameObject magnet in repellers)
        {
            magnet.GetComponent<BlockDrag>()?.Delete();
        }
        StartCoroutine(moveback());
        velocity = Vector2.zero;
        SimilationStarted = false;
        timeSpentThisLevel = 0;
        paused = false;
        acceleration = Vector2.zero;
        timeSpentThisLevel = 0;
        velocity = Vector2.zero;
        Time.timeScale = 1f;
        Attractor = null;
        Repeller = null;
    }
    private void StartTime()
    {
        Time.timeScale = 1f;
        c_level = Int32.Parse(SceneManager.GetActiveScene().name.Substring(5));
        for (int i = 0; i < c_level; i++)
        {
            lastTotalTime += Global.times[i];
            Debug.Log(Global.times[i]);
        }
        PlayerPrefs.SetInt("lastlevel", c_level);
        PlayerPrefs.Save();
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
        paused = false;
        velocity = Vector2.zero;
        UpdateMagnets();
    }
    public void SimToggle()
    {
        if (!SimilationStarted)
        {
            SimilationStarted = true;
            velocity = Vector2.zero;
        }
        else
        {
            paused = !paused;
        }
        UpdateMagnets();
    }
    public void SimStop()
    {
        SimilationStarted = false;
        paused = false;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        UpdateMagnets();
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
    public void UpdateMagnets()
    {
        Attractor = GameObject.FindGameObjectsWithTag("Attractor");
        Repeller = GameObject.FindGameObjectsWithTag("Repeller");
        playButton.sprite = (SimilationStarted && !paused) ? pause : play;
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
                float distanceToAttractor = Vector2.Distance(transform.position, attractor.transform.position);
                if (distanceToAttractor <= interactionRange && distanceToAttractor > minimumDistance)
                {
                    Vector3 directionToAttractor = (attractor.transform.position - transform.position).normalized;
                    directionToAttractor.z = 0;
                    float forceMultiplier = Mathf.Lerp(1f, 0.5f, distanceToAttractor / interactionRange);
                    attractionDirection += directionToAttractor * forceMultiplier / Mathf.Max(distanceToAttractor, 1f);

                }
            }
        }

        if (repellers != null)
        {
            foreach (GameObject repeller in repellers)
            {
                float distanceToRepeller = Vector2.Distance(transform.position, repeller.transform.position);
                if (distanceToRepeller <= interactionRange && distanceToRepeller > minimumDistance)
                {
                    Vector3 directionToRepeller = (transform.position - repeller.transform.position).normalized;
                    directionToRepeller.z = 0;
                    float forceMultiplier = Mathf.Lerp(1f, 0.3f, distanceToRepeller / interactionRange);
                    repulsionDirection += directionToRepeller * forceMultiplier / Mathf.Max(distanceToRepeller, 1f);
                }

            }
        }
        acceleration = (attractionDirection * attractionSpeed) + (repulsionDirection * repulsionForce);
    }

    private void ApplyMovement(bool move)
    {
        velocity += acceleration * Time.deltaTime;
        velocity *= 1 - dragCoefficient * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        velocity.z = 0;
        if (move)
        {
            transform.position += velocity * Time.deltaTime;
        }
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
