using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingCooldown : MonoBehaviour
{
    private Shooting shootingScript;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownTime = 2f;
    public float cooldownTimer = 0f;
    public bool canShoot = true;

    [Header("UI")]
    public Image cooldownBar;
    public Transform barUiAnchor;

    void Start()
    {
        shootingScript = GetComponent<Shooting>();

        if (cooldownBar != null)
        {
            cooldownBar.fillAmount = 1f;
            cooldownBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleCooldown();
    }

    private void HandleCooldown()
    {
        if (!canShoot)
        {
            cooldownTimer += Time.deltaTime;
            float progress = cooldownTimer / cooldownTime;

            if (cooldownBar != null)
                cooldownBar.fillAmount = progress;
                cooldownBar.gameObject.SetActive(true);

            if (cooldownTimer >= cooldownTime)
            {
                canShoot = true;
                if (cooldownBar != null)
                    cooldownBar.fillAmount = 1f;
                    cooldownBar.gameObject.SetActive(false);
            }
        }

        if (cooldownBar != null && barUiAnchor != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(barUiAnchor.position);
            cooldownBar.transform.position = screenPos;
        }
    }
}
