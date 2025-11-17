using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorTeleportation : MonoBehaviour
{
    private bool playerInDoorZone;
    public GameObject otherDoor;

    public FadeTransition fadeTransitionScript;

    [SerializeField] private bool isEntranceDoor;
    [SerializeField] private bool isSmallRoomDoor;
    [SerializeField] private bool isLockedDoor;

    //lockedDoor

    [SerializeField] private GameObject[] lilTargetLocks;
    public bool lockedDoorIsUnlocked = false;

    //UI
    public TextMeshProUGUI challengeText;
    public TextMeshProUGUI challengeTitleText;
    public TextMeshProUGUI challengeTimerText;
    public SmallChallengeAsset smallChallengeAsset;
    public SmallChallengeStart smallChallengeStartScript;

    void Start()
    {
        DisableChallengeTransitionUI();

        if (!isEntranceDoor)
        {
            challengeText = null;
            challengeTitleText = null;
            challengeTimerText = null;
            smallChallengeAsset = null;
            smallChallengeStartScript = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInDoorZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInDoorZone = false;
        }
    }

    private void Update()
    {
        if (playerInDoorZone && Input.GetKeyDown(KeyCode.E) && !isLockedDoor)
        {

            if (!GameState.Instance.isInSmallRoom)
            {
                GameState.Instance.EnterSmallRoom();
            }
            else
            {
                GameState.Instance.ExitSmallRoom();
            }

            StartCoroutine(Teleportation());
        }
        else if (playerInDoorZone && Input.GetKeyDown(KeyCode.E) && isLockedDoor && lockedDoorIsUnlocked)
        {
            GameState.Instance.EnterSmallRoom();
            StartCoroutine(Teleportation());
        }

        // lock check

        if (AllTargetLockDestroyed(lilTargetLocks))
        {
            lockedDoorIsUnlocked = true;
        }
    }

    private IEnumerator Teleportation()
    {
        fadeTransitionScript.FadeIn();

        if (isEntranceDoor)
        {
            EnableChallengeTransitionUI();

            GameState.Instance.ChangeDartLilTargetCollision(GameState.Instance.isInSmallRoom);
            Debug.Log("Changed Collision!");
        }

        yield return new WaitForSeconds(isEntranceDoor 
        ? fadeTransitionScript.doorEntranceTransitionDuration 
        : fadeTransitionScript.doorExitTransitionDuration);
        
        PlayerMovement.Instance.transform.position = otherDoor.transform.position;
        
        yield return new WaitForSeconds(isEntranceDoor 
        ? fadeTransitionScript.doorEntranceTransitionDuration 
        : fadeTransitionScript.doorExitTransitionDuration);

        if (!isEntranceDoor)
        {
            GameState.Instance.ChangeDartLilTargetCollision(GameState.Instance.isInSmallRoom);
            Debug.Log("Changed Collision!");

        }
            
        if (isEntranceDoor) DisableChallengeTransitionUI();

        fadeTransitionScript.FadeOut();
       
    }

    private void EnableChallengeTransitionUI()
    {
        challengeText.gameObject.SetActive(true);
        challengeTitleText.gameObject.SetActive(true);
        challengeTitleText.text = smallChallengeAsset.smallChallengeTitle;
        challengeTimerText.gameObject.SetActive(true);
        string formattedTime = string.Format("{0:00}:{1:00}", 0, smallChallengeAsset.smallChallengeTimeInSec);
        challengeTimerText.text = formattedTime;
        //challengeTimerText.text = Mathf.Ceil(smallChallengeAsset.smallChallengeTimeInSec).ToString();
    }

    private void DisableChallengeTransitionUI()
    {
        challengeText.gameObject.SetActive(false);
        challengeTitleText.gameObject.SetActive(false);
        challengeTimerText.gameObject.SetActive(false);
    }

    static bool AllTargetLockDestroyed(GameObject[] lockedTargets)
    {
        if (lockedTargets == null || lockedTargets.Length == 0)
            return true;

        foreach (var obj in lockedTargets)
        {
            if (obj != null)
                return false;
        }

        return true;
    }
    
}
