using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SubmitLeaderboard : MonoBehaviour
{
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] TMP_InputField inputName;
    [SerializeField] MaxCharacterController controller;
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SubmitScore()
    {
        Debug.Log($"new entry: {inputName.text} {(int)controller.Score}");
        leaderboard.SetLeaderboardEntry(inputName.text, (int) controller.Score);
    }
}
