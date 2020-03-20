using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { set; get; }

    private GameManager gameManager;

    //---------------------------------
    // Groups/Arrays
    //---------------------------------
    public DialogueBranch[] dialogueBranches;
    public int branchIndex;
    public DialogueBranch currDialogueBranch;

    [Space(10)]

    public GameObject[] choiceSets;
    public int choiceSetIndex;
    public GameObject currChoiceSet;

    [Space(10)]

    [Header("------------------------------------------------")]

    [Space(10)]

    //---------------------------------
    // GameObject References
    //---------------------------------
    public Text dialogueText;
    public Text settingText;
    public GameObject fade;
    public GameObject continueButton;

    [Space(10)]

    [Header("------------------------------------------------")]

    [Space(10)]

    [TextArea(3, 5)]
    public string setting;
    public Color textColorDaughter;
    public Color textColorDad;
    public int nextSceneIndex;

    //---------------------------------
    // Private Variables
    //---------------------------------
    private Queue<string> sentences;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;

        //dialogueBranch = DialogueBranch.Instance;
        //branchIndex = 0;    // Replace this with GameManager's loadBranchIndex

        branchIndex = gameManager.loadBranchIndexForNextScene;

        currDialogueBranch = dialogueBranches[branchIndex];

        //choiceSetIndex = 0;
        choiceSetIndex = gameManager.loadChoiceIndexForNextScene;
        currChoiceSet = choiceSets[choiceSetIndex];

        sentences = new Queue<string>();

        //textColorDaughter = new Color(255, 180, 60);
        //textColorDad = new Color(8, 255, 100);

        dialogueText.text = "";
        continueButton.SetActive(false);

        settingText.text = setting;
        
        settingText.gameObject.GetComponent<Animator>().Play("Fade_InAndOut");
        PlayFadeAnimation("Fade_FromBlack");    // About 5 seconds long
        

        Invoke("TriggerDialogue", 5f);
    }

    

    private void TriggerDialogue()
    {
        fade.SetActive(false);
        settingText.gameObject.SetActive(false);

        

        currDialogueBranch.TriggerDialogue();
    }

    private void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        //animator.SetBool("IsOpen", true);

        //nameText.text = dialogue.name;

        

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        continueButton.SetActive(false);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        ChangeTextColor();

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //yield return null;

            if(SceneManager.GetActiveScene().name == "SC11_Voicemail")
                yield return new WaitForSeconds(.09f); //yield return null;//yield return new WaitForSeconds(.1f);   // Should be set to .1f
            else
                yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(1f);

        //if (dialogueBranch.currDialogueIndex == (dialogueBranch.dialogue.Length - 1))
        if (currDialogueBranch.dialogueIndex == (currDialogueBranch.dialogue.Length - 1))   // This checks if the dialogueBranch[] is the last element of the array
        {
            //choiceBox.SetActive(true);
            //currChoiceSet.SetActive(true);
            
            if (sentences.Count != 0)
            {
                continueButton.SetActive(true);
                continueButton.GetComponent<Animator>().Play("Fade_In");
            }
            else if (choiceSetIndex != choiceSets.Length - 1)
            {
                currChoiceSet.SetActive(true);
            }
            else if (choiceSetIndex == choiceSets.Length - 1)
            {
                Debug.Log("End of scene");

                fade.SetActive(true);
                PlayFadeAnimation("Fade_ToBlack");  // 3.5 seconds long

                //SceneManager.LoadScene(nextSceneIndex);

                StartCoroutine(ChangeScene(3.5f));
            }
        }
        else
        {
            continueButton.SetActive(true);
            continueButton.GetComponent<Animator>().Play("Fade_In");
        }
            

    }

    private void EndDialogue()
    {
        //animator.SetBool("IsOpen", false);
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        /*
        if (dialogueBranch.currDialogueIndex != (dialogueBranch.dialogue.Length - 1))
        {
            // Move on to the next dialogue
            dialogueBranch.currDialogueIndex++;
            StartDialogue(dialogueBranch.dialogue[dialogueBranch.currDialogueIndex]);
        }
        */

        if (currDialogueBranch.dialogueIndex != (currDialogueBranch.dialogue.Length - 1))
        {
            // Move on to the next dialogue
            currDialogueBranch.dialogueIndex++;
            StartDialogue(currDialogueBranch.dialogue[currDialogueBranch.dialogueIndex]);
        }
    }

    public void ChangeTextColor()
    {
        //switch (dialogueBranch.dialogue[dialogueBranch.currDialogueIndex].name)
        switch (currDialogueBranch.dialogue[currDialogueBranch.dialogueIndex].name)
        {
            case "Daughter":
                dialogueText.color = textColorDaughter;
                break;
            case "Dad":
                dialogueText.color = textColorDad;
                break;
        }
    }

    public void SetCurrentDialogueBranch(int branchInt) // Set in Choice A/B OnClick() event
    {
        branchIndex = branchInt;
        currDialogueBranch = dialogueBranches[branchInt];

        ChangeTextColor();
    }

    public void SetCurrentChoiceSet(int choiceInt)      // Set in Choice A/B OnClick() event
    {
        choiceSetIndex = choiceInt;
        currChoiceSet = choiceSets[choiceInt];

        // If the Choice Set index is set to the last index of the Choice Set (AKA the ChoiceSet_Empty), the Choice Set gameobject will no longer appear/end choices
    }

    private void PlayFadeAnimation(string animationClip)
    {
        fade.GetComponent<Animator>().Play(animationClip);
    }

    IEnumerator ChangeScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void SetDialogueForNextScene(int index)
    {
        gameManager.loadBranchIndexForNextScene = index;
    }

    public void SetChoiceSetForNextScene(int index)
    {
        gameManager.loadChoiceIndexForNextScene = index;
    }

    public void SetSceneIndex(int index)
    {
        nextSceneIndex = index;
    }
}