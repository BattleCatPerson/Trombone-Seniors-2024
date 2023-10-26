using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    gray, red
}

public class FoxArea : MonoBehaviour
{
    [SerializeField] Color color;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fox>(out Fox f) && f.Color == color)
        {
            f.DisableGrab();
            JacksonGameManager.score++;
        }
    }
}
