using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    internal void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
