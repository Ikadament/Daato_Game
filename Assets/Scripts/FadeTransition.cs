using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] public float doorEntranceTransitionDuration;
    [SerializeField] public float doorExitTransitionDuration;

    private void Start()
    {
        // Optionnel : commencer invisible
        SetAlpha(0f);
    }

    public void FadeIn()  // vers visible (alpha 1)
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeOut() // vers invisible (alpha 0)
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
