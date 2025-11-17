using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private Animator scoreAnimator;

    private int score = 0;
    private bool firstUpdate = true;
    [SerializeField] public float scoreMultiplicator;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        scoreAnimator = scoreText.GetComponent<Animator>();

        UpdateScoreUI();
        firstUpdate = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = score.ToString("D7");

        if (!firstUpdate)
        {
            scoreAnimator.SetTrigger("ScoreUpdate");
        }
        
    }

}
