using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;

    [SerializeField]
    private float offset = 0.34f;

    public LayerMask layers;


    //overlap box collision

    public Vector2 point;
    public Vector2 size;
    private Vector2 substractBoxVector = new Vector2(0.05f, 0.3f);

    public Vector2 gizmoPoint;
    public Vector2 gizmoSize;

    [SerializeField]
    private float sizeX = 0.72f;
    [SerializeField]
    private float sizeY = 0.35f;


    private void Start()
    {

    }

    private void Update()
    {
        point = transform.position + Vector3.down * offset;
        size = new Vector2(sizeX, sizeY) - substractBoxVector;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(point, size, 0, layers);
    }

    private void OnDrawGizmos()
    {
        //if (!Application.isPlaying) return; // Ne dessine que quand le jeu tourne

        gizmoPoint = (Vector2)transform.position + Vector2.down * offset;
        gizmoSize = new Vector2(sizeX, sizeY) - new Vector2(0.1f, 0.3f);

        Gizmos.color = isGrounded ? UnityEngine.Color.green : UnityEngine.Color.red;
        Gizmos.DrawWireCube(gizmoPoint, gizmoSize);
    }
}
