using UnityEngine;

public class LoadingCube : MonoBehaviour
{
    private enum Axis { X, Y, Z }
    [SerializeField] private Axis axis;
    [SerializeField] private float max, min, speed, rotspeed, offset, mag;
    void Start()
    {
        switch(axis){
            case Axis.X:
                transform.localScale = new Vector3(max, min, min);
                break;
            case Axis.Y:
                transform.localScale = new Vector3(min, max, min);
                break;
            case Axis.Z:
                transform.localScale = new Vector3(min, min, max);
                break;
        }
    }

    void Update()
    {
        Vector3 newRot = transform.rotation.eulerAngles;
        switch(axis){
            case Axis.X:
                newRot.x += Time.fixedDeltaTime * rotspeed;
                break;
            case Axis.Y:
                newRot.y += Time.fixedDeltaTime * rotspeed;
                break;
            case Axis.Z:
                newRot.z += Time.fixedDeltaTime * rotspeed;
                break;
        }
        transform.rotation = Quaternion.Euler(newRot);
        Vector3 pos = transform.position;
        switch(axis){
            case Axis.X:
                pos.x = Mathf.Sin(Time.time * speed + offset) * mag;
                break;
            case Axis.Y:
                pos.y = Mathf.Sin(Time.time * speed + offset) * mag;
                break;
            case Axis.Z:
                pos.z = Mathf.Sin(Time.time * speed + offset) * mag;
                break;
        }
        transform.position = pos;
    }
}
