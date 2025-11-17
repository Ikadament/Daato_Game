using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHook : MonoBehaviour
{

    [SerializeField] private float hookDuration;
    [SerializeField] private float hookEndWaitDuration;

    private int hookLayer;
    private int usedHookLayer;

    private void Start()
    {
        hookLayer = LayerMask.NameToLayer("Hook");
        usedHookLayer = LayerMask.NameToLayer("UsedHook");
    }

    public void Hooked(GameObject targetHook)
    {
        transform.DOMove(targetHook.transform.position, hookDuration);
        StartCoroutine(OnHookEndWait(targetHook));
    }

    private IEnumerator OnHookEndWait(GameObject targetHook)
    {
        targetHook.layer = usedHookLayer;
        yield return new WaitForSeconds(hookEndWaitDuration);
        targetHook.layer = hookLayer;
    }

}
