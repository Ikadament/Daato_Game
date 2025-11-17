using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilTarget : MonoBehaviour
{

    private WhiteFlash whiteFlashScript;

    public GameObject petiteCibleFragmentsPrefab;

    public bool isChallengeLilTarget;
    public GameObject lilTargetFragments;

    


    private int targetScore;

    private void Start()
    {
        whiteFlashScript = GetComponent<WhiteFlash>();

        //taille de la cibe * multiplicateur

        float localScaleValue = transform.localScale.y;
        targetScore = Mathf.RoundToInt(localScaleValue * ScoreManager.Instance.scoreMultiplicator);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flechette"))
        {
            whiteFlashScript.Flash();
            HitStop.Instance.HitPause();

            ScoreManager.Instance.AddScore(targetScore);

            StartCoroutine(WaitAndDestroy(collision));

            Destroy(collision.gameObject);
        }
    }

    private IEnumerator WaitAndDestroy(Collision2D collision)
    {       
        yield return new WaitForSeconds(whiteFlashScript.duration);

        GameObject fragments = Instantiate(petiteCibleFragmentsPrefab, transform.position, transform.rotation);

        //Instantiate score GUI
        WorldScoreManager.Instance.SpawnScorePopup(targetScore, transform.position);

        fragments.transform.localScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2, 1); //multiplier par 2 sinon trop petit

        if (isChallengeLilTarget)
        {
            fragments.gameObject.tag = "ChallengeFragments";
        }

        Destroy(transform.gameObject);
    }
}
