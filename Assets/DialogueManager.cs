using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue currentDialogue;

    public GameObject dialogueBox;
    public GameObject branchingBox;

    public Image actorAvatar;
    public Text actorName;
    public Text actorSpeech;

    public Text decisionBlurb1;
    public Text decisionBlurb2;

    Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
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

        // TO-DO implement branching
        if (currentDialogue.branchNext)
        {
            decisionBlurb1.text = currentDialogue.branch.decisionBlurb1;
            decisionBlurb2.text = currentDialogue.branch.decisionBlurb2;
            branchingBox.SetActive(true);
        }

        Debug.Log("End of dialogue!");
    }

    public void DecisionMade(Button button)
    {
        currentDialogue = button.name == "Decision 1" ? currentDialogue.branch.branch1 : currentDialogue.branch.branch2;
        branchingBox.SetActive(false);
        StartDialogue();
        //Debug.Log(button.name);
    }

    IEnumerator DisplaySentence(string sentence)
    {
        actorName.text = currentDialogue.currentActor;
        actorSpeech.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            actorSpeech.text += letter;
            yield return null;
        }
    }
}
