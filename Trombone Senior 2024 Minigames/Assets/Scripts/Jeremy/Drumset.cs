using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEditor.Experimental.GraphView.GraphView;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Drumset : MonoBehaviour
{
    [Serializable]
    public class KeyPair
    {
        public GameObject key;
        public AudioClip clip;
    }

    [SerializeField] GameObject cymbal;
    [SerializeField] GameObject bassDrum;
    [SerializeField] GameObject groundDrumRight;
    [SerializeField] GameObject groundDrumLeft;
    [SerializeField] GameObject hiHat;
    [SerializeField] GameObject rightSmallDrum;
    [SerializeField] GameObject leftSmallDrum;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<KeyPair> keys;
    private void Update()
    {
        var activeTouches = Touch.activeTouches;
        foreach (Touch touch in activeTouches)
        {
            if (touch.phase != TouchPhase.Began) continue;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.screenPosition), out hit))
            {
                GameObject g = hit.collider.gameObject;
                audioSource.PlayOneShot(GetClip(g));
            }

        }

    }
    public AudioClip GetClip(GameObject g)
    {
        foreach (KeyPair k in keys) if (g == k.key) return k.clip;
        return null;
    }
}
