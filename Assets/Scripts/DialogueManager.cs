using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Dialogue currentDialogue;
    public LevelChanger levelChanger;

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

    public GameObject mechPrologueImage;

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        // Set dialogue box focus and actor...
        SetCurrentFocus();
        //Debug.Log("Starting conversation with " + currentDialogue.currentActor);

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
        if (currentDialogue.importantSegue)
        {
            PlaySegue(currentDialogue.segueToCutTo);
            return;
        }

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

        if (currentDialogue.switchToNextSceneAfterThis)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
            case CurrentDialogueBox.ChildBox:
                currentDialogueBox = prologueBox;
                currentText = prologueText;
                break;
            default:
                Debug.Log("INVALID DIALOGUE BOX");
                break;
        }

        currentDialogueBox.SetActive(true);
    }

    void PlaySegue(Segue segueToCutTo)
    {
        if (segueToCutTo == Segue.PresentMechPrologue)
        {
            Debug.Log("Mech Segue!");
            mechPrologueImage.SetActive(true);
            pilotBox.GetComponent<RectTransform>().Translate(200, 0, 0);
            prologueBox.GetComponent<RectTransform>().Translate(200, 0, 0);
            currentDialogue = currentDialogue.nextDialogue;
            StartDialogue();
        }

        if (segueToCutTo == Segue.ToMainScene)
        {
            levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
        int soundDelay = 0;
        foreach(char letter in sentence.ToCharArray())
        {
            currentText.text += letter;
            soundDelay++;
            if (soundDelay % 3 == 0) {
                sfxSource.PlayOneShot(sfxSource.clip);
                soundDelay = 0;
            }

            yield return null;
        }
    }
}
