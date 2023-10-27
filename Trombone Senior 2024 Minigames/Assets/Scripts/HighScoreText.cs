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
        text.text = "High Score: ";
        switch (minigame)
        {
            case Minigame.Jackson:
                text.text += $"{PlayerPrefs.GetInt("Jackson High Score")}";
                break;

                //implement these later
                //case Minigame.Jeremy:
                //    break;
                //case Minigame.Max:
                //    break;
                //case Minigame.Bhavandeep:
                //    break;
                //case Minigame.Nick:
                //    break;
        }
    }
}
