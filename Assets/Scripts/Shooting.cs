using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;

public class Shooting : MonoBehaviour
{
    public static Shooting instance;

    [Header("Trajectory & Shooting")]
    public GameObject trajectoryLineTri;
    [SerializeField] private float radius = 2f;
    private Vector3 direction;
    public GameObject flechettePrefab;
    private FlechetteBehavior flechetteScript;
    public PickingUp pickingUpScript;
    public GameObject controlledDart;
    public bool dartInstantiated = false;
    public bool canShootBasic = true;
    private Animator animator;

    public float mouseDownTimeElapsed;
    [SerializeField] public float mouseDownTimeElapsedMax;
    [SerializeField] public float elapsedMultiplicator;
    [SerializeField] public float elapsedMinimum;
    [SerializeField] public float flechetteSpeed = 10;
    [SerializeField] private float baseSpeedFactor = 2f;

    private ShootingCooldown cooldownScript;

    [Header("Zoom")]

    public GameObject zoomPoint;
    [SerializeField] private float zoomOffsetFactor;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        instance = this;
    }

    private void Start()
    {
        trajectoryLineTri.SetActive(true);

        animator = GetComponent<Animator>();
        cooldownScript = GetComponent<ShootingCooldown>();
    }

    void Update()
    {
        //Rotate trajectory line with mouse position
        TrajectoryLineRotation();
        zoomPoint.transform.position = trajectoryLineTri.transform.position + direction * zoomOffsetFactor;

        if (Input.GetMouseButton(0) && cooldownScript.canShoot && canShootBasic) //&& pickingUpScript.numberOfDart > 0)
        {
            trajectoryLineTri.SetActive(false);

            CameraZoom.instance.MakeCameraZoom(zoomPoint);

            mouseDownTimeElapsed += Time.deltaTime * baseSpeedFactor;
            mouseDownTimeElapsed = Mathf.Min(mouseDownTimeElapsed, mouseDownTimeElapsedMax);

            //faire en sorte qu'il soit instancié une seule fois ici
            if (!dartInstantiated)
            {
                controlledDart = Instantiate(pickingUpScript.dartPrefab, transform.position, Quaternion.identity);
                dartInstantiated = true;
            }

            if (controlledDart != null)
            {
                flechetteScript = controlledDart.GetComponent<FlechetteBehavior>();
                flechetteScript.DartRotation(); // tourne la fleche durant update en fonction de souris si on appuie
            }

        }

        if (Input.GetMouseButtonUp(0) && dartInstantiated && cooldownScript.canShoot && canShootBasic)
        {
            trajectoryLineTri.SetActive(true);

            CameraZoom.instance.MakeCameraDezoom();
            
            flechetteScript.lunching = true;
            pickingUpScript.numberOfDart -= 1;
            dartInstantiated = false;

            animator.SetTrigger("Shoot");

            //démarre cooldown
            cooldownScript.canShoot = false;
            cooldownScript.cooldownTimer = 0f;
        }

    }

    private void TrajectoryLineRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction = (mousePos - transform.position).normalized;

        Vector3 orbitPosition = transform.position + direction * radius;
        trajectoryLineTri.transform.position = orbitPosition;

        Vector3 lookDir = (trajectoryLineTri.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        trajectoryLineTri.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    
}
