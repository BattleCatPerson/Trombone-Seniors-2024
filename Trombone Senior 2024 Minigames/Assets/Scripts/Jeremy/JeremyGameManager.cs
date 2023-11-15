using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public enum DrumPhase
{
    initial = 1, wait = 2, player = 3
}

public class JeremyGameManager : MonoBehaviour
{
    [Header("Rhythm")]
    [SerializeField] int BPM;
    [SerializeField] int BPMInitial;
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

    [Header("Score and Health")]
    [SerializeField] int score;
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    [SerializeField] TextMeshProUGUI bpmText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;

    [SerializeField] Image healthBar;

    [SerializeField] int scorePerHit;
    [SerializeField] float healthLostPerMiss;

    [SerializeField] Animator bpmUpAnimation;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject newHighScoreText;

    public bool stop;

    [SerializeField] AudioSource music;
    [SerializeField] bool gameStarted;
    [SerializeField] GameObject gameStartedPanel;

    private void Start()
    {
        gameStarted = false;
        notesPerMeasure = notesPerMeasureRange.start;
        readyPanel.SetActive(false);
        maxHealth = health;
        gameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false);
        if (!PlayerPrefs.HasKey("Jeremy High Score")) PlayerPrefs.SetInt("Jeremy High Score", 0);
        gameStartedPanel.SetActive(true);
        cursor.gameObject.SetActive(false);

        scoreText.text = $"{score}";
        finalScoreText.text = $"Final Score: {score}";
        bpmText.text = $"{BPM} BPM";
    }
    public void StartGame()
    {
        Debug.Assert(BPM > 0);
        GenerateMeasure();
        source.PlayOneShot(metronome);
        if (!music.isPlaying) music.Play();
        BPMInitial = BPM;
        gameStarted = true;
        gameStartedPanel.SetActive(false);
        cursor.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (stop || !gameStarted) return;
        if (health <= 0)
        {
            stop = true;
            gameOverPanel.SetActive(true);
            NewHighScore();
            music.Stop();
            return;
        }
        timer += Time.fixedDeltaTime;
        metronomeTimer += Time.fixedDeltaTime;
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
                    drumSet.playable = true;
                    timesToIgnore.Clear();
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
                    break;
                }
            }
        }

        if (phase == DrumPhase.player)
        {
            float timeToRemove = -1;
            foreach (float f in times)
            {
                if (timer >= f + marginOfError)
                {
                    health -= healthLostPerMiss;
                    timeToRemove = f;
                    break;
                }
            }
            times.Remove(timeToRemove);
        }

        scoreText.text = $"{score}";
        finalScoreText.text = $"Final Score: {score}";
        bpmText.text = $"{BPM} BPM";
        healthBar.fillAmount = health / maxHealth;
    }

    public void GenerateMeasure()
    {
        drumSet.playable = false;
        foreach (GameObject g in icons) Destroy(g);
        icons.Clear();

        if (notesPerMeasure > notesPerMeasureRange.end)
        {
            BPM += BPMIncrease;
            notesPerMeasure = notesPerMeasureRange.start;

            bpmUpAnimation.SetTrigger("BpmUp");

            music.pitch += (float) BPMIncrease / BPMInitial;
            music.Stop();
            music.Play();
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
        if (nearestTime == -1)
        {
            health -= healthLostPerMiss;
            return;
        }

        GameObject nearest = timeObjectPairs[nearestTime];
        float diff = MathF.Abs(nearestTime - timer);

        if (nearest != g || diff > marginOfError)
        {
            Debug.Log("your life is as valuable as a summer ant");
            health -= healthLostPerMiss;
            return;
        }

        Debug.Log("YOU DID IT");
        score += scorePerHit;

        if (!timeIconPairs.ContainsKey(nearestTime)) return;
        GameObject icon = timeIconPairs[nearestTime];

        icons.Remove(icon);
        Destroy(icon);
        timeIconPairs.Remove(nearestTime);

        timeObjectPairs.Remove(nearestTime);
        times.Remove(nearestTime);

    }

    public void NewHighScore()
    {
        if (score > PlayerPrefs.GetInt("Jeremy High Score"))
        {
            PlayerPrefs.SetInt("Jeremy High Score", score);
            newHighScoreText.SetActive(true);
        }
    }
}
