using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum ScoreType
{
    score, highScore
}

public enum Minigame
{
    Jackson, Jeremy, Max, Bhavandeep, Nick
}

public class HighScoreText : MonoBehaviour
{
    [SerializeField] Minigame minigame;
    [SerializeField] TextMeshProUGUI text;

    void Update()
    {
        text.text = "High: ";
        switch (minigame)
        {
            case Minigame.Jackson:
                text.text += $"{PlayerPrefs.GetInt("Jackson High Score")}";
                break;
            case Minigame.Jeremy:
                text.text += $"{PlayerPrefs.GetInt("Jeremy High Score")}";
                break;
            case Minigame.Max:
                text.text += $"{PlayerPrefs.GetInt("Max High Score")}";
                break;
            case Minigame.Bhavandeep:
                text.text += $"{PlayerPrefs.GetInt("Bhavandeep High Score")}";
                break;
            case Minigame.Nick:
                text.text += $"{PlayerPrefs.GetInt("Nick High Score")}";
                break;
        }
    }
}
