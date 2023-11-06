using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameOnWallCollision : MonoBehaviour
{
    [SerializeField] NickGameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.position.z < transform.position.z) return;
        gameManager.StopGame();
        collision.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
