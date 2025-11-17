using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoneTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera sceneCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.SwitchCamera(sceneCam, 0.4f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.SwitchCamera(playerCam, 0.4f);
        }
    }
}
