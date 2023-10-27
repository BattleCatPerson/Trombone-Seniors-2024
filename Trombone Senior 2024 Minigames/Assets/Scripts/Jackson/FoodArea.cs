using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodArea : MonoBehaviour
{
    public static bool gameOver = false;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] JacksonGameManager gameManager;
    [SerializeField] GameObject newHighScoreText;
    private void Start()
    {
        gameOver = false;
        newHighScoreText.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !gameOver && !collision.GetComponentInParent<Fox>().Grabbed)
        {
            gameOver = true;
            gameOverPanel.SetActive(true);
            if (gameManager.NewHighScore()) newHighScoreText.SetActive(true);
        }
    }
}
