using System.Collections;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject clipCircle;
    [SerializeField] private float maxScale, speedfac;
    public void Open()
    {
        StartCoroutine(open());
    }
    public void Close()
    {
        StartCoroutine(close());
    }
    private IEnumerator open(){
        gameObject.SetActive(true);
        while(clipCircle.transform.localScale.x < maxScale){
            clipCircle.transform.localScale += Vector3.one * speedfac * Time.unscaledDeltaTime;
            yield return null;
        }
    }
    private IEnumerator close(){
        while(clipCircle.transform.localScale.x > 0){
            clipCircle.transform.localScale -= Vector3.one * speedfac * Time.unscaledDeltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
