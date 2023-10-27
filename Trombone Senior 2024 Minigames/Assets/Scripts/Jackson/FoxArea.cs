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
    [SerializeField] List<Fox> foxesInArea;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent<Fox>(out Fox f) && f.Color == color && !f.Grabbed && !foxesInArea.Contains(f))
        {
            f.DisableGrab();
            JacksonGameManager.score++;
            foxesInArea.Add(f);
        }
    }
}
