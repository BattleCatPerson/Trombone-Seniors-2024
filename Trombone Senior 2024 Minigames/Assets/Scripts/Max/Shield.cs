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
    }
}
