using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetHook : MonoBehaviour
{
    private WhiteFlash whiteFlashScript;
    private PlayerHook playerHookScript;

    

    private void Start()
    {
        whiteFlashScript = GetComponent<WhiteFlash>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHookScript = player.GetComponent<PlayerHook>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flechette"))
        {


            whiteFlashScript.Flash();
            HitStop.Instance.HitPause();
            Destroy(collision.gameObject);

            playerHookScript.Hooked(gameObject);
        }
    }
}
