using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom instance;

    [Header("References")]
    public CinemachineVirtualCamera vcam;
    public Transform player;

    [Header("Settings")]
    [SerializeField] private float zoomValue = 4f;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float yOffset = 2f;

    //internal
    private float baseCamSize;
    private Coroutine currentCoroutine = null;
    private float sizeVel = 0f;
    private Vector3 moveVel = Vector3.zero;

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

    void Start()
    {
        baseCamSize = vcam.m_Lens.OrthographicSize;

        vcam.Follow = player;
    }

    public void MakeCameraZoom(GameObject zoomPoint)
    {
        // Stopper la coroutine en cours pour eviter les conflits
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(SmoothZoom(zoomPoint.transform));
    }

    private IEnumerator SmoothZoom(Transform zoomPoint)
    {
        vcam.Follow = zoomPoint;

        Vector3 target = new Vector3(zoomPoint.position.x, zoomPoint.position.y, vcam.transform.position.z);

        while (Mathf.Abs(vcam.m_Lens.OrthographicSize - zoomValue) > 0.01f || (zoomPoint.position - target).sqrMagnitude > 0.0001f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, zoomValue, ref sizeVel, smoothTime);

            zoomPoint.position = Vector3.SmoothDamp(zoomPoint.position, target, ref moveVel, smoothTime);

            zoomPoint.position = new Vector3(zoomPoint.position.x, zoomPoint.position.y, vcam.transform.position.z);

            yield return null;
        }

        vcam.m_Lens.OrthographicSize = zoomValue;
        zoomPoint.position = target;

        currentCoroutine = null;
    } 
    
    public void MakeCameraDezoom()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(SmoothDezoom());
    }

    private IEnumerator SmoothDezoom()
    {
        vcam.Follow = player;

        while (Mathf.Abs(vcam.m_Lens.OrthographicSize - baseCamSize) > 0.01f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, baseCamSize, ref sizeVel, smoothTime);

            yield return null;
        }

        vcam.m_Lens.OrthographicSize = baseCamSize;

        currentCoroutine = null;
    }
}
