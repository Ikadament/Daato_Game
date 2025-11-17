using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmallRoomEnter : MonoBehaviour
{
    public SmallChallengeAsset smallChallengeData;
    public TextMeshProUGUI titleText;
    

    public SmallChallengeStart smallChallengeStartScript;


    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform[] spawnPoints;
    public List<GameObject> lilTargets = new List<GameObject>();

    public bool roomSetup = false;

    private void Update()
    {
        if (GameState.Instance.isInSmallRoom && !roomSetup)
        {
            WriteTitle();
            SpawnLilTargets();
            smallChallengeStartScript.ChangeChallengeLilTargetActiveOrInactive();
            Debug.Log("Room setup DONE");

            roomSetup = true;
        }
    }

    private void WriteTitle()
    {
        titleText.text = smallChallengeData.smallChallengeTitle;
    }

    public void SpawnLilTargets()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;

            GameObject newTarget = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);

            LilTarget newTargetLilTargetScript = newTarget.GetComponent<LilTarget>();
            newTargetLilTargetScript.isChallengeLilTarget = true;

            lilTargets.Add(newTarget);

            SpriteRenderer newTargetSpriteRenderer = newTarget.GetComponent<SpriteRenderer>();

            smallChallengeStartScript.lilTargetsSpriteRenderer.Add(newTargetSpriteRenderer);

        }
    }


}
