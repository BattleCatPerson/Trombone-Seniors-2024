using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    public void PlayGameOver() => audioManager.PlayGameOver(true);
    public void StopGameOver() => audioManager.PlayGameOver(false);
}
