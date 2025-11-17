using UnityEngine;

public class ZoneCible : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private GameObject scorePos;

    public WhiteFlash whiteFlashCibleCorrect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flechette"))
        {
            ScoreManager.Instance.AddScore(points);
            WorldScoreManager.Instance.SpawnScorePopup(points, scorePos.transform.position);
            whiteFlashCibleCorrect.Flash();
        }
    }
}
