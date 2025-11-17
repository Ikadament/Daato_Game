using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBoxObject;
    [SerializeField] private RectTransform dialogueBoxRect;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public bool dialogueIsActive;
    public bool isTyping;

    //offsets 

    int dialogueIndex;

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

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsActive = false;
        //dialoguePackage.SetActive(false);
    }

    public void DialoguePackageToNpcPos(Vector2 dialogueBoxPos)
    {

        dialogueBoxRect.position = dialogueBoxPos;
        dialogueBoxObject.SetActive(true);
    }

    public void StartDialogue(NpcDialogueAsset dialogueAsset)
    {
        dialogueIsActive = true;
        dialogueIndex = 0;

        //pause controller normalement mais là freeze
        PlayerMovement.Instance.FreezePlayer();

        StartCoroutine(TypeLine(dialogueAsset));
    }

    IEnumerator TypeLine(NpcDialogueAsset dialogueAsset)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogueAsset.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueAsset.typingSpeed);
        }

        isTyping = false;

    }

    public void NextLine(NpcDialogueAsset dialogueAsset)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueAsset.dialogueLines[dialogueIndex];
            isTyping = false;
        }
        else if (dialogueIndex + 1 < dialogueAsset.dialogueLines.Length)
        {
            dialogueIndex++;
            StartCoroutine(TypeLine(dialogueAsset));
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogueIsActive = false;
        dialogueBoxObject.SetActive(false);
        dialogueText.text = "";

        CameraManager.SwitchCamera(CameraManager.StoredActiveCamera, CameraManager.DialogueZoomSpeed);

        PlayerMovement.Instance.DefrostPlayer();
    }

}
