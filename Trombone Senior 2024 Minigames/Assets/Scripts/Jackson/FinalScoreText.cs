using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.text = $"Final Score: {JacksonGameManager.score}";
    }
}
