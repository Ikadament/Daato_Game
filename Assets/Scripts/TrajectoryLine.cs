using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Shooting shootingScript;

    [Header("Trajectory settings")]
    [SerializeField] private int resolution = 30;
    [SerializeField] private float simulationTime = 2f;

    private LineRenderer lineRenderer;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && shootingScript.canShootBasic)
        {
            lineRenderer.enabled = true;

            Vector3 startPos = player.position;

            //souris
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector3 dir = (mousePos - player.position).normalized;

            //force de tir
            float elapsed = Mathf.Max(shootingScript.mouseDownTimeElapsed,
            shootingScript.elapsedMinimum);

            elapsed *= shootingScript.elapsedMultiplicator;
            

            //vitesse de base
            float speed = shootingScript.flechetteSpeed * elapsed;
            Vector2 velocity = dir * speed;

            DrawTrajectory(player.position, velocity);
        }
        else
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
    }

    private void DrawTrajectory(Vector2 startPos, Vector2 startVelocity)
    {
        Vector2 pos = startPos;
        Vector2 vel = startVelocity;

        // Gravité réelle du Rigidbody2D
        Vector2 gravity = Physics2D.gravity * 2; //2 = gravity scale

        lineRenderer.positionCount = resolution;

        float dt = simulationTime / resolution;

        for (int i = 0; i < resolution; i++)
        {
            lineRenderer.SetPosition(i, pos);

            // appliquer gravité
            vel += gravity * dt;

            // appliquer drag (approximation d’Unity)
            vel *= 1f / (1f + 0 * dt);

            // avancer
            pos += vel * dt;
        }
    }
}
