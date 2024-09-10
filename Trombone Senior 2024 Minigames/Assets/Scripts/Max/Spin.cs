using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] Vector3 rotationPerSecond;
    void FixedUpdate()
    {
        transform.eulerAngles += rotationPerSecond * Time.fixedDeltaTime;   
    }
}
