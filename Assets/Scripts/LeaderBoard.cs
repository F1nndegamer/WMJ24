using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private GameObject dataPrefab;
        [SerializeField] private Transform parent;
        public ScrollRect rectA;
        private int scorea;

        private void Start()
        {
            scorea = 0;
            for (int i = 0; i < Global.times.Length; i++)
            {
                scorea += Global.times[i];
            }
            UploadEntry();
        }

        private void LoadEntries()
        {
            // Q: How do I reference my own leaderboard?
            // A: Leaderboards.<NameOfTheLeaderboard>

            Leaderboards.WMJ24.GetEntries(entries =>
            {
                LeaderData[] objectss = dataPrefab.GetComponentsInChildren<LeaderData>();
                foreach (LeaderData dat in objectss)
                {
                    Destroy(dat.gameObject);
                }
                var length = entries.Length;
                for (int i = 0; i < length; i++)
                {
                    LeaderData dat = Instantiate(Resources.Load("LeaderData") as GameObject, parent).GetComponent<LeaderData>();
                    dat.SetData(entries[i].Username, PlayerScript.IntToTimeString(entries[i].Score), entries[i].Rank.ToString());
                }
            });
        }

        public void UploadEntry()
        {
            Leaderboards.WMJ24.UploadNewEntry(PlayerPrefs.GetString("name"), scorea, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
    }
}