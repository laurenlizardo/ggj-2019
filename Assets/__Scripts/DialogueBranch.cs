using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBranch : MonoBehaviour
{
    private DialogueManager dialogueManager;
    
    [Header("---------------------------------------------------------------------------")]
    [Header("DO NOT ALTERNATE")]
    public int dialogueIndex;
    public Dialogue currDialogue;

    [Header("---------------------------------------------------------------------------")]
    [Header("Dialogue Branch Customization")]
    [Space(20)]

    public Dialogue[] dialogue;


    private void Start()
    {
        dialogueManager = DialogueManager.Instance;

        //Invoke("TriggerDialogue", 2);
    }

    public void TriggerDialogue()
    {
        
        dialogueIndex = 0;
        currDialogue = dialogue[dialogueIndex];

        dialogueManager.ChangeTextColor();
        dialogueManager.StartDialogue(currDialogue);
    }
    
}
