using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //A RETIRER

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 7f;

    private bool isWalking;
    public bool ladderHitbox;

    public Rigidbody2D rb;

    //A RETIRER, JUSTE POUR TEST
    public CinemachineVirtualCamera camPlayer;
    public CinemachineVirtualCamera camLevel;

    //Jump variables
    public float jumpAmount = 5f;
    public float jumpCutMultiplier = 0.5f;
    public GroundCheck groundCheck;

    //Coyote Time Variables
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //Jump Buffer Variables
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //Freeze
    public bool freeze;

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

    private void Update()
    {

        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W) && !freeze) inputVector.y += 1;
        if (Input.GetKey(KeyCode.S) && !freeze) inputVector.y -= 1;

        if (Input.GetKey(KeyCode.A) && !freeze)
        {
            inputVector.x -= 1;
            spriteRenderer.flipX = true;
        }


        if (Input.GetKey(KeyCode.D) && !freeze)
        {
            inputVector.x += 1;
            spriteRenderer.flipX = false;
        }


        inputVector = inputVector.normalized;

        float moveY = ladderHitbox ? inputVector.y : 0;
        Vector3 moveDirection = new Vector3(inputVector.x, moveY, 0);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        //jump

        if (groundCheck.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !freeze)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0) //&& !isTalking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);

            jumpBufferCounter = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && !freeze) //&& !isTalking)
        {
            coyoteTimeCounter = 0f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderHitbox = true;
            rb.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderHitbox = false;
            rb.gravityScale = 1f;
        }
    }

    public void FreezeRigidbody()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void DefrostRigidbody()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector2.zero;
        rb.WakeUp();
    }

    public void FreezePlayer()
    {
        freeze = true;
        FreezeRigidbody();
        Shooting.instance.canShootBasic = false;
        Shooting.instance.trajectoryLineTri.SetActive(false);
    }

    public void DefrostPlayer()
    {
        freeze = false;
        DefrostRigidbody();
        Shooting.instance.canShootBasic = true;
        Shooting.instance.trajectoryLineTri.SetActive(true);
    }

}
