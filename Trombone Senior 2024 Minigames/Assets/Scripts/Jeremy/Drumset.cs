using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;

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

    [SerializeField] JeremyGameManager gameManager;
    public bool playable;
    private void Awake()
    {
    }
    private void Update()
    {
        if (!playable) return;
        var activeTouches = Touch.activeTouches;
        foreach (Touch touch in activeTouches)
        {
            if (touch.phase != TouchPhase.Began) continue;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.screenPosition), out hit))
            {
                GameObject g = hit.collider.gameObject;
                if (GetClip(g) != null)
                {
                    PlayClip(g);
                    gameManager.AddInput(g);
                }
            }
        }
    }
    public AudioClip GetClip(GameObject g)
    {
        foreach (KeyPair k in keys) if (g == k.key) return k.clip;
        return null;
    }

    public void PlayClip(GameObject g) => audioSource.PlayOneShot(GetClip(g));

    public GameObject ReturnRandomDrumpiece()
    {
        int index = Random.Range(0, keys.Count);
        return keys[index].key;
    }
}
