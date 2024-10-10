using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using TMPro;
public class Leaderboard : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> scores;
    private string publicLeaderboardKey = "51ad3b5829793874e57f3ace420e00cec973b8409578169d43789631b477ebd6";
    //secret key 361cd9ef106b8d5102e882dc027d0e47e92e78f819fe7935269bab944970f62f0532faf420eab79868d4cd0330ba45a83b76fc288906b178cb436905fe3f7c2ee76cf6457387cd9174433443c2dac6ed27d1a9227619f25dc7299d909bc53da67d80c1d1cb3649178052cfe684646737d97ae3075879f663a328d3c2473f0496

    private void Start()
    {
        GetLeaderboard();
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
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }
}
