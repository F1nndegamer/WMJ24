using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int n = 0;
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Easy;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image[] images;

    void Start()
    {
        StartCoroutine(ButtonIn());
        InvokeRepeating("ButtonUpdate", 0.1f, 5f);
    }
    public void ButtonUpdate()
    {
        levelText.text = n.ToString();
        foreach(Image image in images){
            image.color = Color.white;
            //Set color based on difficutly
        }
    }
    private IEnumerator ButtonIn()
    {
        while (transform.localScale.x < 1)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
