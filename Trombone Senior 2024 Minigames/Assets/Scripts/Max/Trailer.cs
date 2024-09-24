using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    public Vector3 velocity;
    private void Start()
    {
        Destroy(gameObject, 4f);
    }
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
