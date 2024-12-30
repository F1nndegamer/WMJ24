using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensures the GameObject persists across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Prevent duplicate MusicManager instances
        }
    }
}
