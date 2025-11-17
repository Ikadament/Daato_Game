using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SmallChallengeStart : MonoBehaviour
{
    public bool smallChallengeStarted = false;
    public SmallRoomEnter smallRoomEnterScript;

    //UI
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI temporaryVictoryUI;
    public SmallChallengeAsset smallChallengeData;
    private float startTime;
    private float timeLeft;
    private Coroutine timerCoroutine;

    //Visuals
    public List<SpriteRenderer> lilTargetsSpriteRenderer = new List<SpriteRenderer>();

    private void Start()
    {
        startTime = smallChallengeData.smallChallengeTimeInSec;
        SetTimerText();

        
    }

    private void Update()
    {
        ChallengeOverVerification();

        LilTargetListUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            smallChallengeStarted = true;

            timerCoroutine = StartCoroutine(StartCountdown(startTime));

            // // si smallChallengeStarted false alors desac collisions et inversemement
            // Physics2D.IgnoreLayerCollision(dartLayer, lilTargetLayer, !smallChallengeStarted); 
            GameState.Instance.ChangeDartLilTargetCollision(!smallChallengeStarted);
            ChangeChallengeLilTargetActiveOrInactive();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            smallChallengeStarted = false;

            StopCoroutine(timerCoroutine);
            SetTimerText();

            timeLeft = startTime;

            temporaryVictoryUI.gameObject.SetActive(false);
            DestroyAllFragments();
            DestroyAllLilTargets();
            smallRoomEnterScript.SpawnLilTargets();

            GameState.Instance.ChangeDartLilTargetCollision(!smallChallengeStarted);
            ChangeChallengeLilTargetActiveOrInactive();
        }
    }

    IEnumerator StartCountdown(float duration)
    {
        timeLeft = duration;

        while (timeLeft >= 0)
        {
            timerText.text = Mathf.Ceil(timeLeft).ToString(); // Affiche dans la console
            yield return new WaitForSeconds(1f); // Attend 1 seconde
            timeLeft--;
        }

        Debug.Log("Minuteur fin");
    }

    private void SetTimerText()
    {
        timerText.text = startTime.ToString();
    }

    public void ChangeChallengeLilTargetActiveOrInactive()
    {

        if (lilTargetsSpriteRenderer == null || lilTargetsSpriteRenderer.Count == 0)
            return;

        float targetAlpha = smallChallengeStarted ? 1f : 0.7f;

        foreach (SpriteRenderer sr in lilTargetsSpriteRenderer)
        {
            if (sr == null) continue;

            Color color = sr.color;
            color.a = targetAlpha;
            sr.color = color;
        }

    }

    private void ChallengeOverVerification()
    {
        if (smallRoomEnterScript.roomSetup && smallRoomEnterScript.lilTargets.Count == 0)
        {
            StopCoroutine(timerCoroutine);
            temporaryVictoryUI.gameObject.SetActive(true);
        }
    }

    private void LilTargetListUpdate()
    {
        // Parcours la liste des targets et supprime celles qui ont été détruites
        for (int i = smallRoomEnterScript.lilTargets.Count - 1; i >= 0; i--)
        {
            if (smallRoomEnterScript.lilTargets[i] == null)
            {
                smallRoomEnterScript.lilTargets.RemoveAt(i);
                lilTargetsSpriteRenderer.RemoveAt(i);
            }
        }
    }

    private void DestroyAllLilTargets()
    {
        for (int i = smallRoomEnterScript.lilTargets.Count - 1; i >= 0; i--)
        {
            GameObject lilTarget = smallRoomEnterScript.lilTargets[i];
            if (lilTarget != null)
            {
                Destroy(lilTarget);
            }
            smallRoomEnterScript.lilTargets.RemoveAt(i);
        }

        lilTargetsSpriteRenderer.Clear();
    }

    private void DestroyAllFragments()
    {
        GameObject[] fragmentsGroup = GameObject.FindGameObjectsWithTag("ChallengeFragments");

        foreach (GameObject fragments in fragmentsGroup)
        {
            Destroy(fragments);
        }
    }
}
