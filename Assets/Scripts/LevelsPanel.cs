using TMPro;
using UnityEngine;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] Color[] colors;
    bool opened = false;
    public int levels;
    public GameObject nameSelect;
    public TMP_InputField namee;
    public static int levelsUnlocked = 1;
    [Header("0 = easy, 1 = medium, 2 = hard")]
    public int[] difficulties;
    [SerializeField] private GameObject levelParent;
    void Start()
    {
        levelsUnlocked = PlayerPrefs.GetInt("levels");
        levels = Global.times.Length;
        if (levelsUnlocked == 0) { levelsUnlocked = 1; }
    }
    public void Open()
    {
        if (!opened)
        {
            if (PlayerPrefs.GetString("name") == null || PlayerPrefs.GetString("name") == "")
            {
                nameSelect.SetActive(true);
                return;
            }
            opened = true;
            levelsUnlocked = PlayerPrefs.GetInt("levels");
            if (levelsUnlocked == 0) { levelsUnlocked = 1; }
            for (int i = 1; i <= levels; i++)
            {
                LevelButton button = Instantiate(Resources.Load("LevelButton") as GameObject, levelParent.transform).GetComponent<LevelButton>();
                button.colors = colors;
                if (i <= levelsUnlocked) { button.unlocked = true; }
                button.ButtonUpdate();
                button.n = i;
                LevelButton.Difficulty difficulty = LevelButton.Difficulty.Easy;
                if (difficulties[i - 1] == 1) { difficulty = LevelButton.Difficulty.Medium; } else if (difficulties[i - 1] == 2) { difficulty = LevelButton.Difficulty.Hard; }
                button.difficulty = difficulty;
            }
        }
    }
    public void SetName()
    {
        if (namee.text != "")
        {
            PlayerPrefs.SetString("name", namee.text);
            PlayerPrefs.Save();
            nameSelect.SetActive(false);
            Open();
        }
    }
}
