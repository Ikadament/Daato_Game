using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlechetteBehavior : MonoBehaviour
{
    [Header("References")]
    public GameObject flechetteEmptyPrefab;
    private PickingUp pickingUpScript;
    private Shooting shootingScript;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    private Vector3 direction;
    private Vector2 lastNormal;

    [Header("Dart settings")]
    [SerializeField] private float detectionDistance = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector3 offset;

    [Header("Outils")]
    public bool stuck = false;
    public bool lunching = false;
    private bool lunched = false;
    private Vector3 stuckOffset;
    private GameObject player;
    private Transform stuckTarget = null;
    private Quaternion lastRotation;



    void OnDrawGizmos()
    {
        if (rb == null) return;

        // Position actuelle
        Vector3 pos = transform.position;

        // 1) Dessiner la direction de la vitesse
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + (Vector3)rb.velocity.normalized * 2f);
            Gizmos.DrawSphere(pos + (Vector3)rb.velocity.normalized * 2f, 0.05f);
        }

        // 2) Dessiner la normale de la dernière collision
        if (lastNormal != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + (Vector3)lastNormal * 2f);
            Gizmos.DrawSphere(pos + (Vector3)lastNormal * 2f, 0.05f);
        }
    }

    //recoil 
    // [SerializeField]
    // private float recoilDistance = 0.3f;
    // private bool anticipate = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pickingUpScript = player.GetComponent<PickingUp>();
        shootingScript = player.GetComponent<Shooting>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;
    }

    private void Update()
    {
        if (rb.simulated && rb.velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (!lunching)
        {
            // if (!anticipate)
            // {
            //     Recoil();
            // }

            //porter la flechette
            transform.position = player.transform.position + offset;
        }

    }

    // private void LateUpdate()
    // {
    //     if (stuck && stuckTarget != null)
    //     {
    //         transform.position = stuckTarget.position + stuckOffset;
    //     }

    // }

    private void FixedUpdate()
    {
        lastRotation = transform.rotation;

        if (lunching)
        {
            if (!lunched)
            {
                FlyingMovement();
            }

            lunched = true;
        }
    }
    public void DartRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void FlyingMovement()
    {
        //transform.position += transform.right * flechetteSpeed * Time.fixedDeltaTime;
        rb.simulated = true;

        //si elapse = 0.1 alors il passe au minimum 0.2, on lui rajoute X5 pour faire 1 donc ça fait speed = 10 x 1, toujours 10
        //si elapse = 0.5 alors rajoute X5 pour faire 2.5, donc speed = 10 x 2.5, donc 25

        float elapsed = Mathf.Max(shootingScript.mouseDownTimeElapsed, shootingScript.elapsedMinimum);
        elapsed *= shootingScript.elapsedMultiplicator;

        rb.AddForce(transform.right * shootingScript.flechetteSpeed * elapsed, ForceMode2D.Impulse);

        shootingScript.mouseDownTimeElapsed = 0f;
    }

    // private void Recoil()
    // {
    //     gameObject.transform.localPosition -= Vector3.down * recoilDistance;
    //     anticipate = true;
    // }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            lastNormal = collision.contacts[0].normal;

            Vector3 currentPosition = transform.position;

            rb.simulated = false;

            Instantiate(flechetteEmptyPrefab, currentPosition, lastRotation);

            Destroy(gameObject);
        }
    }
}
