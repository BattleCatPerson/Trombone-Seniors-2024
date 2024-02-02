using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float rotation;
    void Start()
    {
        
    }

    void Update()
    {
        transform.eulerAngles = Vector3.forward * rotation;

        if (Input.GetKey(KeyCode.Q)) rotation += 180 * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) rotation -= 180 * Time.deltaTime;
    }
}
