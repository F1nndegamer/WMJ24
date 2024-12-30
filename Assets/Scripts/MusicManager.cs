using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private void Update()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
