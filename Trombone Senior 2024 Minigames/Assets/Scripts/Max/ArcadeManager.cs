using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ArcadeManager : MonoBehaviour
{
    [SerializeField] Image gameImage;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] AnimatorSetTrigger panelAnimator;
    [SerializeField] RestartAndQuit sceneLoader;

    public void OpenPanel(ArcadeButton button)
    {
        var info = button.info;
        gameImage.sprite = info.image;
        title.text = info.title;
        description.text = info.description;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt(info.highScoreKey).ToString();
        sceneLoader.ChangeScene(info.scene);

        panelAnimator.SetTrigger();
    }

    public void ClosePanel()
    {
        panelAnimator.SetTrigger();
    }
}
