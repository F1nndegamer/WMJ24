using UnityEngine;
using TMPro;
using Dan.Main;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private GameObject dataPrefab;
        [SerializeField] private Transform parent;
        public int Score;

        private void Start()
        {
            LoadEntries();
        }

        private void LoadEntries()
        {
            // Q: How do I reference my own leaderboard?
            // A: Leaderboards.<NameOfTheLeaderboard>
        
            Leaderboards.WMJ24.GetEntries(entries =>
            {
                LeaderData[] objectss = dataPrefab.GetComponentsInChildren<LeaderData>();
                foreach(LeaderData dat in objectss){
                    Destroy(dat.gameObject);
                }
                var length = entries.Length;
                for (int i = 0; i < length; i++){
                    LeaderData dat = Instantiate(Resources.Load("LeaderData") as GameObject, parent).GetComponent<LeaderData>();
                    dat.SetData(entries[i].Username, entries[i].Score.ToString(), entries[i].Rank.ToString());
                }
            });
        }
        
        public void UploadEntry()
        {
            Leaderboards.WMJ24.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
    }
}