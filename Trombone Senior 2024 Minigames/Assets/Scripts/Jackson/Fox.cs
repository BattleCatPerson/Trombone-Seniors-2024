using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] Color color;
    public Color Color => color;
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] Transform target;

    [Header("Grab")]
    [SerializeField] bool grabbed;
    public bool Grabbed => grabbed;
    [SerializeField] bool grabbable;
    public bool Grabbable => grabbable;
    [SerializeField] Rigidbody2D rb;
    void Start()
    {
        grabbable = true;
    }

    void Update()
    {
        if (grabbed || Vector3.Distance(transform.position, target.position) < 0.01f || !grabbable || FoodArea.gameOver) rb.velocity = Vector3.zero;
        else rb.velocity = (target.position - transform.position).normalized * speed;
        if (grabbable) transform.up = -rb.velocity;
    }

    public void Grab(bool b) => grabbed = b;

    public void SetTarget(Transform t) => target = t;
    public void DisableGrab() => grabbable = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HASDOUIADSHOASD");
    }
}
