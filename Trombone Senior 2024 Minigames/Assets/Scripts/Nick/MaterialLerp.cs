using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    [SerializeField] List<Renderer> renderers;
    [SerializeField] Material material1;
    [SerializeField] Material material2;
    [SerializeField] float duration;
    [SerializeField] float timer;
    [SerializeField] bool started;
    void Start()
    {
        foreach (Transform t in transform)
        {
            renderers.Add(t.GetComponent<Renderer>());
        }
    }

    public void StartLerp()
    {
        started = true;
    }

    void Update()
    {
        if (started && timer <= duration)
        {
            timer += Time.deltaTime;
            foreach (Renderer r in renderers)
            {
                r.material.Lerp(material1, material2, timer / duration);
            }
        }
    }
}
