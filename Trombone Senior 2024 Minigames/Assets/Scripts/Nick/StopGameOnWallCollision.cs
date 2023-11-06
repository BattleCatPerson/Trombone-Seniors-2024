using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameOnWallCollision : MonoBehaviour
{
    [SerializeField] NickGameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        gameManager.StopGame();
    }
}
