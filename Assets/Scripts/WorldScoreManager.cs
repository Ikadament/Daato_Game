using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WorldScoreManager : MonoBehaviour
{
    public static WorldScoreManager Instance;

    [Header("UI refs")]
    [SerializeField] private Canvas worldScoreCanva;
    [SerializeField] private GameObject scorePopupPrefab;

    [Header("Animation settings")]
    [SerializeField] private float moveYLengh;
    [SerializeField] private float moveYDuration;
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnScorePopup(int score, Vector3 worldPos)
    {
        GameObject scorePopup = Instantiate(scorePopupPrefab, worldScoreCanva.transform);
        scorePopup.transform.position = worldPos;

        TextMeshProUGUI tmp = scorePopup.GetComponent<TextMeshProUGUI>();
        tmp.text = score.ToString();

        scorePopup.transform.DOMoveY(worldPos.y + moveYLengh, moveYDuration).SetEase(Ease.OutQuad);
        tmp.DOFade(0f, fadeDuration).OnComplete(() => Destroy(scorePopup));
    }

}
