using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum DrumPhase
{
    initial = 1, wait = 2, player = 3
}

public class JeremyGameManager : MonoBehaviour
{
    [Header("Rhythm")]
    [SerializeField] int BPM;
    [SerializeField] float beatsPerSecond;
    [SerializeField] float timePerBeat;
    [SerializeField] int BPMIncrease;
    [SerializeField] int beats;
    [SerializeField] int notesPerMeasure;
    [Serializable]
    public class Range
    {
        public int start;
        public int end;
    }
    [SerializeField] Range notesPerMeasureRange;

    [Header("Time")]
    [SerializeField] float duration;
    [SerializeField] float timer;
    [SerializeField] List<float> times;
    [SerializeField] List<float> timesToIgnore;

    [Header("Inputs")]
    [SerializeField] List<GameObject> options;
    [SerializeField] List<GameObject> currentSequence;
    [SerializeField] Drumset drumSet;
    Dictionary<float, GameObject> timeObjectPairs;

    [Serializable]
    public class ObjectIconPair
    {
        public GameObject key;
        public GameObject icon;
    }
    [SerializeField] List<ObjectIconPair> objectIconPairs;
    Dictionary<float, GameObject> timeIconPairs;
    [SerializeField] List<GameObject> icons;
    [SerializeField] DrumPhase phase;

    [SerializeField] float marginOfError;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip metronome;
    [SerializeField] float metronomeTimer;

    [Header("Cursor")]
    [SerializeField] RectTransform cursor;
    [SerializeField] float xOffset;

    [Header("Player Ready Check")]
    [SerializeField] Image bar;
    [SerializeField] GameObject readyPanel;

    
    private void Start()
    {
        Debug.Assert(BPM > 0);
        notesPerMeasure = notesPerMeasureRange.start;
        GenerateMeasure();
        readyPanel.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        metronomeTimer += Time.deltaTime;
        if (metronomeTimer > timePerBeat)
        {
            source.PlayOneShot(metronome);
            metronomeTimer = 0;

            if (phase == DrumPhase.wait) bar.fillAmount += 1f / beats;
        }
        cursor.anchoredPosition = new(Mathf.Lerp(-xOffset, xOffset, timer / duration), cursor.anchoredPosition.y);

        if (timer > duration)
        {
            timer = 0;
            metronomeTimer = 0;
            source.PlayOneShot(metronome);
            cursor.anchoredPosition = new(-xOffset, cursor.anchoredPosition.y);

            if (phase != DrumPhase.player)
            {
                if (phase == DrumPhase.initial)
                {
                    cursor.gameObject.SetActive(false);
                    if (times.Count > 0 && times[times.Count - 1] == duration)
                    {
                        float f = times[times.Count - 1];
                        GameObject g = timeObjectPairs[f];
                        drumSet.PlayClip(g);

                        var clone = Instantiate(ReturnIcon(g), cursor.transform.parent);
                        clone.GetComponent<RectTransform>().anchoredPosition = cursor.anchoredPosition;
                        icons.Add(clone);
                        timeIconPairs[f] = clone;
                    }

                    bar.fillAmount = 1f/ beats;
                    readyPanel.SetActive(true);
                }
                else if (phase == DrumPhase.wait)
                {
                    cursor.gameObject.SetActive(true);
                    readyPanel.SetActive(false);
                }
                phase += 1;
            }
            else 
            {
                notesPerMeasure++;
                phase = DrumPhase.initial;
                GenerateMeasure();
            }
            return;
        }

        if (phase == DrumPhase.initial)
        {
            foreach (float f in times)
            {
                if (timer >= f && !timesToIgnore.Contains(f))
                {
                    GameObject g = timeObjectPairs[f];
                    drumSet.PlayClip(g);

                    var clone = Instantiate(ReturnIcon(g), cursor.transform.parent);

                    float x = ((f / (timePerBeat * beats)) * 2 * xOffset) - xOffset;
                    clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, cursor.anchoredPosition.y);
                    cursor.SetAsLastSibling();
                    icons.Add(clone);

                    timeIconPairs[f] = clone;

                    timesToIgnore.Add(f);
                }
            }
        }
    }

    public void GenerateMeasure()
    {
        foreach (GameObject g in icons) Destroy(g);
        icons.Clear();

        if (notesPerMeasure > notesPerMeasureRange.end)
        {
            BPM += BPMIncrease;
            notesPerMeasure = notesPerMeasureRange.start;
        }

        beatsPerSecond = (float)BPM / 60;
        timePerBeat = 1 / beatsPerSecond;

        timeObjectPairs = new();
        timeIconPairs = new();
        int count = 0;
        times.Clear();
        timeObjectPairs.Clear();
        timesToIgnore.Clear();
        timeIconPairs.Clear();

        while (count < notesPerMeasure)
        {
            float eights = Random.Range(0, beats * 2);
            float t = eights * timePerBeat / 2;
            if (!times.Contains(t))
            {
                times.Add(t);
                timeObjectPairs[t] = drumSet.ReturnRandomDrumpiece();
                count++;
            }
        }

        duration = timePerBeat * beats;
        times.Sort();

        marginOfError = timePerBeat / 2f;

    }

    public GameObject ReturnIcon(GameObject g)
    {
        foreach (ObjectIconPair o in objectIconPairs) if (o.key == g) return o.icon;
        return null;
    }

    public void AddInput(GameObject g)
    {
        if (phase != DrumPhase.player) return;
        float leastDiff = Mathf.Infinity;
        float nearestTime = -1;
        foreach (float t in times)
        {
            if (Mathf.Abs(t - timer) < leastDiff && timeObjectPairs[t] == g)
            {
                nearestTime = t;
                leastDiff = Mathf.Abs(t - timer);
            }
        }
        if (nearestTime == -1) return;

        GameObject nearest = timeObjectPairs[nearestTime];
        float diff = MathF.Abs(nearestTime - timer);

        if (nearest != g || diff > marginOfError)
        {
            Debug.Log("your life is as valuable as a summer ant");
            return;
        }

        Debug.Log("YOU DID IT");

        if (!timeIconPairs.ContainsKey(nearestTime)) return;
        GameObject icon = timeIconPairs[nearestTime];

        icons.Remove(icon);
        Destroy(icon);
        timeIconPairs.Remove(nearestTime);

        timeObjectPairs.Remove(nearestTime);
        times.Remove(nearestTime);

    }
}
