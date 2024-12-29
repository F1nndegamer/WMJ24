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
    public Color[] colors = { Color.white, Color.white, Color.white };

    void Start()
    {
        StartCoroutine(ButtonIn());
        InvokeRepeating("ButtonUpdate", 0.1f, 5f);
    }
    public void ButtonUpdate()
    {
        levelText.text = n.ToString();
        int i = 0;
        if (difficulty == Difficulty.Medium) { i = 1; } else if (difficulty == Difficulty.Hard) { i = 2; }
        Color imgColor = colors[i];
        foreach (Image image in images)
        {
            image.color = imgColor;
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
