using UnityEngine;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] Color[] colors;
    bool opened = false;
    public int levels = 8;
    [Header("0 = easy, 1 = medium, 2 = hard")]
    public int[] difficulties;
    [SerializeField] private GameObject levelParent;
    public void Open()
    {
        opened = true;
        for(int i = 1; i <= levels; i++){
            LevelButton button = Instantiate(Resources.Load("LevelButton") as GameObject, levelParent.transform).GetComponent<LevelButton>();
            button.colors = colors;
            button.ButtonUpdate();
            button.n = i;
            LevelButton.Difficulty difficulty = LevelButton.Difficulty.Easy;
            if(difficulties[i - 1] == 1){ difficulty = LevelButton.Difficulty.Medium; } else if(difficulties[i - 1] == 2){ difficulty = LevelButton.Difficulty.Hard; }
            button.difficulty = difficulty;
        }   
    }
    void Update()
    {
        
    }
}
