using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue currentDialogue;

    public GameObject pilotBox;
    public GameObject prologueBox;
    public GameObject dialogueBox;
    public GameObject branchingBox;

    public Image actorAvatar;
    public Text actorName;
    public Text actorSpeech;

    public Text decisionBlurb1;
    public Text decisionBlurb2;

    public AudioSource sfxSource;

    Queue<string> sentences;
    GameObject currentDialogueBox;
    Text currentText;

    Text pilotText;
    Text prologueText;
    Text dialogueText;

    void Awake()
    {
        pilotText = pilotBox.GetComponentInChildren<Text>();
        prologueText = prologueBox.GetComponentInChildren<Text>();
        dialogueText = dialogueBox.GetComponentInChildren<Text>();

        Debug.Log(pilotText.text);
        Debug.Log(prologueText.text);
        Debug.Log(dialogueText.text);
    }

    void Start()
    {
        sentences = new Queue<string>();
        currentDialogueBox = null;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        // Set dialogue box focus and actor...
        SetCurrentFocus();
        Debug.Log("Starting conversation with " + currentDialogue.currentActor);

        sentences.Clear();

        foreach(string sentence in currentDialogue.lines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        StopAllCoroutines();

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string nextSentence = sentences.Dequeue();
        StartCoroutine(DisplaySentence(nextSentence));
    }

    void EndDialogue()
    {
        if (currentDialogue.nextDialogue != null)
        {
            currentDialogue = currentDialogue.nextDialogue;
            StartDialogue();
            return;
        }

        if (currentDialogue.branchNext)
        {
            decisionBlurb1.text = currentDialogue.branch.decisionBlurb1;
            decisionBlurb2.text = currentDialogue.branch.decisionBlurb2;
            branchingBox.SetActive(true);
        }

        Debug.Log("End of dialogue!");
    }

    void SetCurrentFocus()
    {
        if (currentDialogueBox != null)
        {
            currentDialogueBox.SetActive(false);
        }

        switch (currentDialogue.focus)
        {
            case CurrentDialogueBox.MainBox:
                currentDialogueBox = dialogueBox;
                currentText = dialogueText;
                break;
            case CurrentDialogueBox.PilotBox:
                currentDialogueBox = pilotBox;
                currentText = pilotText;
                break;
            case CurrentDialogueBox.PrologueBox:
                currentDialogueBox = prologueBox;
                currentText = prologueText;
                break;
            default:
                Debug.Log("INVALID DIALOGUE BOX");
                break;
        }

        currentDialogueBox.SetActive(true);
    }

    public void DecisionMade(Button button)
    {
        currentDialogue = button.name == "Decision 1" ? currentDialogue.branch.branch1 : currentDialogue.branch.branch2;
        branchingBox.SetActive(false);
        StartDialogue();
    }

    IEnumerator DisplaySentence(string sentence)
    {
        actorName.text = currentDialogue.currentActor;
        currentText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            currentText.text += letter;
            sfxSource.PlayOneShot(sfxSource.clip);
            yield return null;
        }
    }
}
