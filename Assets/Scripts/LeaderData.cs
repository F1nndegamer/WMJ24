using TMPro;
using UnityEngine;

public class LeaderData : MonoBehaviour
{
    public TextMeshProUGUI namee;
    public TextMeshProUGUI score;
    public TextMeshProUGUI rank;
    public void SetData(string _name, string _score, string _rank){
        namee.text = _name;
        score.text = _score;
        rank.text = _rank;
    }
}
