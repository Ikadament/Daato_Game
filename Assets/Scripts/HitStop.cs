using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    [SerializeField]
    private float duration;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
    }

    public void HitPause()
    {
        StartCoroutine(HitStopIE());
    }

    private IEnumerator HitStopIE()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
