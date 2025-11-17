using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineBrain mainBrain;

    public static CinemachineVirtualCamera ActiveCamera = null;
    public static CinemachineVirtualCamera StoredActiveCamera = null;
    public static float DialogueZoomSpeed = 0.35f;

    void Start()
    {
        if (mainBrain == null)
            mainBrain = Camera.main.GetComponent<CinemachineBrain>();

        
    }

    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == ActiveCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera newCamera, float switchSpeed)
    {
        var blend = mainBrain.m_DefaultBlend;

        blend.m_Time = switchSpeed;

        mainBrain.m_DefaultBlend = blend;

        newCamera.Priority = 10;
        ActiveCamera = newCamera;

        foreach (CinemachineVirtualCamera cam in cameras)
        {
            if(cam != newCamera)
            {
                cam.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }

    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
