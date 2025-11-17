using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilTargetFragments : MonoBehaviour
{
    private WhiteFlash whiteFlashScript;

    private void Start()
    {
        whiteFlashScript = GetComponent<WhiteFlash>();

        whiteFlashScript.Flash();
    }
}
