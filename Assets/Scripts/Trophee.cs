using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Trophee : MonoBehaviour
{
    public GameObject starEffect;
    public Transform starParent;
    public Animator starAnimator;
    private Animator tropheeAnimator;
    private WhiteFlash whiteFlashScript;
    private SpriteRenderer spriteRenderer;
    public Image tropheeHUD;

    //fonctionnement
    private bool tropheeAnimation;
    public CinemachineVirtualCamera playerCam;
    private CinemachineVirtualCamera activeCam;

    public Shooting shootingScript;

    [Header("MoveToCenter Animation Settings")]
    [SerializeField] private float tropheeMoveToCenterDuration;
    [SerializeField] private Vector3 tropheeScaleCenter;
    [SerializeField] private float tropeeScaleCenterDuration;
    [SerializeField] private float waitBeforeSlide;

    [Header("SlidesBack Animation Settings")]
    [SerializeField] private float tropheeSlidesBackValue;
    [SerializeField] private float tropheeSlidesDuration;
    [SerializeField] private Vector3 tropheeScaleSlide;
    [SerializeField] private float waitBeforeBanner;

    [Header("Achievement Banner Animation Settings")]
    private float originBannerScaleX;
    [SerializeField] private float bannerScaleUpX;
    [SerializeField] private float bannerScaleDuration;

    [Header("Achievement Banner UI")]
    //Achievement Banner
    public TropheeAsset tropheeAsset;
    public GameObject achievementCanva;
    public TextMeshProUGUI achievementHeader;
    public TextMeshProUGUI achievementTitle;
    public Transform achievementBannerPivot;
    public GameObject achievementBanner;

    [SerializeField] private float headerOffsetY;
    [SerializeField] private float headerOffsetX;
    [SerializeField] private float titleOffsetY;
    [SerializeField] private float titleOffsetX;
    [SerializeField] private float bannerDuration;

    [Header("End Animation Settings")]
    [SerializeField] private float starEffectScaleDownDuration;
    [SerializeField] private float waitBeforeSlideHUD;
    private Vector3 tropheeHudWorldPos;
    [SerializeField] private float tropheeToHudDuration;
    [SerializeField] [Range(0.1f,0.6f)] private float tropheeToHudSizeDivisor;


    void Start()
    {
        whiteFlashScript = GetComponent<WhiteFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //tropheeAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        tropheeHudWorldPos = Camera.main.ScreenToWorldPoint(tropheeHUD.rectTransform.position);
        tropheeHudWorldPos.z = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flechette") && !tropheeAnimation)
        {
            tropheeAnimation = true;
            activeCam = CameraManager.ActiveCamera;

            Destroy(collision.gameObject);

            PlayerMovement.Instance.FreezePlayer();

            whiteFlashScript.Flash();
            HitStop.Instance.HitPause();

            //tropheeAnimator.SetBool("TropheeIdle", false);

            StartCoroutine(TropheeAnimation());
        }
    }

    IEnumerator TropheeAnimation()
    {
        spriteRenderer.sortingLayerName = "TropheeUI";

        CameraManager.SwitchCamera(playerCam, 0.2f);

        yield return new WaitForSeconds(0.3f);

        TropheeToCenter();

        yield return new WaitForSeconds(waitBeforeSlide);

        TropheeSlidesBackAndShrink();

        yield return new WaitForSeconds(waitBeforeBanner);

        AchievementBannerSlideUp();

        yield return new WaitForSeconds(bannerScaleDuration);

        AchievementSetText();

        yield return new WaitForSeconds(bannerDuration);

        AchievementHideText();

        AchievementBannerSlideDown();

        yield return new WaitForSeconds(waitBeforeSlideHUD);

        TropheeSlideToHUD();

    }

    private void TropheeToCenter()
    {
        Vector3 camCenter = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width / 2f, Screen.height / 2f, 10f));

        transform.DOMove(camCenter, tropheeMoveToCenterDuration);
        transform.DOScale(tropheeScaleCenter, tropeeScaleCenterDuration).OnComplete(() => StarEffect());

    }

    private void TropheeSlidesBackAndShrink()
    {
        transform.DOMoveX(transform.position.x - tropheeSlidesBackValue, tropheeSlidesDuration);
        transform.DOScale(tropheeScaleSlide, tropheeSlidesDuration);
    }

    private void StarEffect()
    {
        starEffect.SetActive(true);
        starAnimator.SetBool("StarAnim", true);
    }

    private void AchievementBannerSlideUp()
    {
        originBannerScaleX = achievementBannerPivot.transform.localScale.x;
        achievementBannerPivot.transform.DOScaleX(bannerScaleUpX, bannerScaleDuration);
    }

    private void AchievementSetText()
    {
        Vector3 headerPos = achievementBanner.transform.position;
        headerPos.y += headerOffsetY;
        headerPos.x += headerOffsetX;

        achievementHeader.transform.position = headerPos;

        Vector3 titlePos = achievementBanner.transform.position;
        titlePos.y += titleOffsetY;
        titlePos.x += titleOffsetX;

        achievementTitle.transform.position = titlePos;

        achievementCanva.SetActive(true);
        achievementHeader.text = $"Achievement <color=#952b2b>({tropheeAsset.tropheeID})</color> unlocked!";
        achievementTitle.text = tropheeAsset.title;
    }

    private void AchievementHideText()
    {
        achievementCanva.SetActive(false);
    }

    private void AchievementBannerSlideDown()
    {
        achievementBannerPivot.transform.DOScaleX(originBannerScaleX, bannerScaleDuration).OnComplete(() => StarEffectExitAndDestroy());
    }

    private void StarEffectExitAndDestroy()
    {
        starParent.transform.DOScale(Vector3.zero, starEffectScaleDownDuration).OnComplete(() => Destroy(starEffect));
    }

    private void TropheeSlideToHUD()
    {
        //Le problème c'est que dès le départ j'aurais du faire en sorte que tropheeobject soit en hud sur un canva,
        // ça aurait permis de faire l'anim quand le joueur bouge sans problème, mais la je dois attendre car ça fait objet => canva pos en worldspace,
        // c'est n'imp.
        

        transform.DOMove(tropheeHudWorldPos, tropheeToHudDuration);
        transform.DOScale(tropheeHUD.rectTransform.localScale * tropheeToHudSizeDivisor, tropheeToHudDuration).OnComplete(() =>
        {
            tropheeAnimation = false;
            
            PlayerMovement.Instance.DefrostPlayer();

            CameraManager.SwitchCamera(activeCam, 0.2f);

            TropheeManager.Instance.AddTropheeCount();
            Destroy(gameObject);
        });
    }
}
