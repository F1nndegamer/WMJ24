using Unity.VisualScripting;
using UnityEngine;

public class dragmanager : MonoBehaviour
{
    public int Magnetsplaced = 0;
    public static dragmanager instance;
    void Start()
    {
        instance = this;
    }
}
