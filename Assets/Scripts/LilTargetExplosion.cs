using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilTargetExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionForce = 300f;
    [SerializeField]
    private float torqueRange = 300f;
    [SerializeField]
    private float explosionRadius = 2f;
    [SerializeField, Range(0f, 1f)]
    private float randomForceFactor = 0.3f; // +/-30% de variation

    void Start()
    {
        Vector2 explosionPos = transform.position;

        foreach (Transform child in transform)
        {
            Rigidbody2D rb2d = child.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                Vector2 direction = rb2d.position - explosionPos;
                float distance = direction.magnitude;

                if (distance <= explosionRadius)
                {
                    float baseForceMagnitude = explosionForce * (1 - distance / explosionRadius);

                    // Randomisation dans la plage [1 - randomForceFactor, 1 + randomForceFactor]
                    float randomMultiplier = Random.Range(1f - randomForceFactor, 1f + randomForceFactor);
                    float finalForceMagnitude = baseForceMagnitude * randomMultiplier;

                    Vector2 force = direction.normalized * finalForceMagnitude;

                    rb2d.AddForce(force, ForceMode2D.Impulse);

                    // Ajout d'un torque aleatoire pour la rotation
                    float randomTorque = Random.Range(-torqueRange, torqueRange); // Ajuste les valeurs selon l'effet voulu
                    rb2d.AddTorque(randomTorque, ForceMode2D.Impulse);
                }
            }
        }
    }
}
