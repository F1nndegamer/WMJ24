using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private GameObject dataPrefab;  // Reference to the leaderboard entry prefab
        [SerializeField] private Transform parent;       // Parent transform to instantiate leaderboard entries
        [SerializeField] private ScrollRect rectA;       // ScrollRect for scrolling through entries
        private int scorea;

        private void Start()
        {
            scorea = 0;

            // Check if there's any gameplay data and calculate the total score
            for (int i = 0; i < Global.times.Length; i++)
            {

                if (Global.times[i] == 0)
                {
                    LoadEntries();
                    return;
                }
                scorea += Global.times[i];
            }
            // Upload the player's entry if scores are available
            UploadEntry();
        }

        private void LoadEntries()
        {

            // Make sure to replace 'WMJ24' with your actual leaderboard name or reference.
            Leaderboards.WMJ24.GetEntries(entries =>
            {

                // Clear existing leaderboard entries
                foreach (LeaderData dat in parent.GetComponentsInChildren<LeaderData>())
                {
                    Destroy(dat.gameObject);
                }

                // Load new entries from the leaderboard
                var length = entries.Length;

                for (int i = 0; i < length; i++)
                {
                    // Instantiate a new LeaderData item for each leaderboard entry
                    LeaderData dat = Instantiate(dataPrefab, parent).GetComponent<LeaderData>();
                    dat.SetData(entries[i].Username, PlayerScript.IntToTimeString(entries[i].Score), entries[i].Rank.ToString());
                }
            });
        }

        public void UploadEntry()
        {
            // Ensure that a name is available before uploading
            string username = PlayerPrefs.GetString("name");

            if (string.IsNullOrEmpty(username))
            {
                Debug.LogError("Player name is not set. Please set the name before uploading.");
                return;
            }

            Leaderboards.WMJ24.UploadNewEntry(username, scorea, isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();  // Reload entries once upload is successful
                }
                else
                {
                    Debug.LogError("Failed to upload leaderboard entry.");
                }
            });
        }
    }
}
