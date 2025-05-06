using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using Dan.Models;
using TMPro;
public class Leaderboard : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> numbers;
    [SerializeField] List<TextMeshProUGUI> scores;
    [SerializeField] TextMeshProUGUI yourName;
    [SerializeField] TextMeshProUGUI yourScore;
    [SerializeField] TextMeshProUGUI yourNumber;
    [SerializeField] TextMeshProUGUI yourTitle;
    [SerializeField] List<string> nameList;
    [SerializeField] GameObject leaderboardGroup;
    [SerializeField] GameObject noNetWorkGroup;
    private string publicLeaderboardKey = "51ad3b5829793874e57f3ace420e00cec973b8409578169d43789631b477ebd6";
    //secret key 361cd9ef106b8d5102e882dc027d0e47e92e78f819fe7935269bab944970f62f0532faf420eab79868d4cd0330ba45a83b76fc288906b178cb436905fe3f7c2ee76cf6457387cd9174433443c2dac6ed27d1a9227619f25dc7299d909bc53da67d80c1d1cb3649178052cfe684646737d97ae3075879f663a328d3c2473f0496

    private void Start()
    {
        //GetLeaderboard();
        Leaderboards.FilchFlipperLeaderboard.GetEntries(new Dan.Models.LeaderboardSearchQuery(), OnLeaderboardLoaded, ErrorCallback);
        LeaderboardCreator.OnNoNetwork.AddListener(NoNewtork);
    }
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            Debug.Log($"leaderboard count {msg.Length}");
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));

    }

    public void SetLeaderboardEntry(string username, int score)
    {
        foreach (var s in nameList) if (s.Equals(username)) return;
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            Leaderboards.FilchFlipperLeaderboard.GetEntries(new Dan.Models.LeaderboardSearchQuery(), OnLeaderboardLoaded, ErrorCallback);
        }));
    }
    private void OnLeaderboardLoaded(Entry[] entries)
    {
        //foreach (Transform t in _entryDisplayParent)
        //    Destroy(t.gameObject);

        //foreach (var t in entries)
        //    CreateEntryDisplay(t);

        //ToggleLoadingPanel(false);
        nameList = new();
        int loopLength = (entries.Length < names.Count) ? entries.Length : names.Count;
        for (int i = 0; i < entries.Length; ++i)
        {
            if (i < names.Count)
            {
                nameList.Add(entries[i].Username);
                names[i].text = entries[i].Username;
                scores[i].text = entries[i].Score.ToString();
                numbers[i].text = (i + 1).ToString() + ".";
                if (entries[i].IsMine())
                {
                    names[i].color = UnityEngine.Color.yellow;
                    numbers[i].color = UnityEngine.Color.yellow;
                }
            }

            if (entries[i].IsMine())
            {
                yourTitle.text = "You";
                yourName.text = entries[i].Username;
                yourScore.text = entries[i].Score.ToString();
                yourNumber.text = (i + 1).ToString() + ".";
            }
        }
    }
    private void ErrorCallback(string error)
    {
        Debug.LogError(error);
    }

    public void NoNewtork()
    {
        Debug.Log("NO NETWORK");
        leaderboardGroup.SetActive(false);
        noNetWorkGroup.SetActive(true);
    }
}
