using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFlash : MonoBehaviour
{
    public Material whiteMaterial; // ton Material blanc
    private Material defaultMaterial;
    private SpriteRenderer sr;

    public SpriteRenderer cibleDevantSr;
    private Material cibleDevantDefaultMaterial;

    [SerializeField]
    public float duration;

    [SerializeField]
    private bool isBigTarget;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultMaterial = sr.material; // garde le mat d'origine

        if(isBigTarget)
        cibleDevantDefaultMaterial = cibleDevantSr.material;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.material = whiteMaterial;

        if (isBigTarget)
        {
            cibleDevantSr.material = whiteMaterial;
        }
        
        yield return new WaitForSeconds(duration);
        sr.material = defaultMaterial;

        if(isBigTarget)
        {
            cibleDevantSr.material = cibleDevantDefaultMaterial;
        }
        
    }
}
