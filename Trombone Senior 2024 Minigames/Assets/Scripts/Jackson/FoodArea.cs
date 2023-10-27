using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodArea : MonoBehaviour
{
    public static bool gameOver = false;
    [SerializeField] GameObject gameOverPanel;
    private void Start()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !gameOver && !collision.GetComponentInParent<Fox>().Grabbed)
        {
            gameOver = true;
            gameOverPanel.SetActive(true);
        }
    }
}
