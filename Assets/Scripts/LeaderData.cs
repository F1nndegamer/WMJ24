using TMPro;
using UnityEngine;

public class LeaderData : MonoBehaviour
{
    public TextMeshProUGUI namee;
    public TextMeshProUGUI score;
    public void SetData(string _name, string _score){
        namee.text = _name;
        score.text = _score;
    }
}
