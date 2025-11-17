using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public NpcDialogueAsset npcDialogueAsset;
    public Transform specificDialogueBoxPos;
    public bool playerInrange;

    public CinemachineVirtualCamera zoomCam;
    [SerializeField] private Vector3 npcCamOffset = new Vector3(0f, 3f, 0f);

    void Awake()
    {
        playerInrange = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInrange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInrange = false;
        }
    }

    void Update()
    {
        if (playerInrange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.instance.dialogueIsActive)
        {
            DialogueManager.instance.DialoguePackageToNpcPos(specificDialogueBoxPos.position);
            DialogueManager.instance.StartDialogue(npcDialogueAsset);

            //cam zoom
            Vector3 npcPosTarget = gameObject.transform.position + npcCamOffset;
            npcPosTarget.z = zoomCam.transform.position.z;
            zoomCam.transform.position = npcPosTarget;

            CameraManager.StoredActiveCamera = (CinemachineVirtualCamera)CameraManager.mainBrain.ActiveVirtualCamera;
            CameraManager.SwitchCamera(zoomCam, CameraManager.DialogueZoomSpeed);
        }
        else if (playerInrange && Input.GetKeyDown(KeyCode.E) && DialogueManager.instance.dialogueIsActive)
        {
            DialogueManager.instance.NextLine(npcDialogueAsset);
        }
    }
}
