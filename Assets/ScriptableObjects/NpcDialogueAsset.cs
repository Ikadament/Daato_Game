using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NpcDialogueAsset : ScriptableObject
{
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;
    public DialogueChoice[] choicesPair;

    public float typingSpeed = 0.05f;
}

[System.Serializable]

public class DialogueChoice
{
    public int dialogueIndex; //Dialogue line where choices appear
    public string[] choices; //Player response options
    public int[] nextDialogueIndexes; //where choice leads
}
