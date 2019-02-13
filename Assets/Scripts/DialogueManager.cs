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
    public GameObject approachBox;

    public Image actorAvatar;
    public Text actorName;
    public Text actorSpeech;

    public Image backgroundImage;

    public Sprite approach1Sprite;
    public Sprite approach2Sprite;
    public Sprite approach3Sprite;
    public Sprite isabelleArrivedSprite;
    public Sprite mechCenteredSprite;

    public Sprite oldmanNeutral;
    public Sprite oldmanAngry;
    public Sprite oldmanShocked;
    public Sprite isabelleDiscovering;
    public Sprite isabelleGlaring;
    public Sprite isabelleLaughing;
    public Sprite isabelleNeutral;

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
    Text approachText;


    void Awake()
    {
        pilotText = pilotBox.GetComponentInChildren<Text>();
        prologueText = prologueBox.GetComponentInChildren<Text>();
        dialogueText = dialogueBox.GetComponentInChildren<Text>();

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            approachText = approachBox.GetComponentInChildren<Text>();
        }

        Debug.Log(pilotText.text);
        Debug.Log(prologueText.text);
        Debug.Log(dialogueText.text);
    }

    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            backgroundImage.sprite = approach1Sprite;
        }
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
        SetAvatarAndName();

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
            case CurrentDialogueBox.ApproachBox:
                currentDialogueBox = approachBox;
                currentText = approachText;
                break;
            default:
                Debug.Log("INVALID DIALOGUE BOX");
                break;
        }

        currentDialogueBox.SetActive(true);
    }

    void SetAvatarAndName()
    {
        if (currentDialogue.currentActor == CurrentActor.IsabelleDiscovering || currentDialogue.currentActor == CurrentActor.IsabelleGlaring
            || currentDialogue.currentActor == CurrentActor.IsabelleLaughing || currentDialogue.currentActor == CurrentActor.IsabelleNeutral)
        {
            actorName.text = "Isabelle";
        }

        if (currentDialogue.currentActor == CurrentActor.OldManAngry || currentDialogue.currentActor == CurrentActor.OldManNeutral
            || currentDialogue.currentActor == CurrentActor.OldManShocked)
        {
            actorName.text = "Old Man";
        }

        switch (currentDialogue.currentActor)
        {
            case CurrentActor.IsabelleDiscovering:
                actorAvatar.sprite = isabelleDiscovering;
                break;
            case CurrentActor.IsabelleGlaring:
                actorAvatar.sprite = isabelleGlaring;
                break;
            case CurrentActor.IsabelleNeutral:
                actorAvatar.sprite = isabelleNeutral;
                break;
            case CurrentActor.IsabelleLaughing:
                actorAvatar.sprite = isabelleLaughing;
                break;
            case CurrentActor.OldManAngry:
                actorAvatar.sprite = oldmanAngry;
                break;
            case CurrentActor.OldManNeutral:
                actorAvatar.sprite = oldmanNeutral;
                break;
            case CurrentActor.OldManShocked:
                actorAvatar.sprite = oldmanShocked;
                break;
            default:
                Debug.Log("INVALID DIALOGUE BOX");
                break;
        }
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

        if (segueToCutTo == Segue.Approach2)
        {
            backgroundImage.sprite = approach2Sprite;
            currentDialogue = currentDialogue.nextDialogue;
            StartDialogue();
        }

        if (segueToCutTo == Segue.Approach3)
        {
            backgroundImage.sprite = approach3Sprite;
            currentDialogue = currentDialogue.nextDialogue;
            StartDialogue();
        }

        if (segueToCutTo == Segue.Approach4)
        {
            backgroundImage.sprite = isabelleArrivedSprite;
            currentDialogue = currentDialogue.nextDialogue;
            StartDialogue();
        }

        if (segueToCutTo == Segue.MechCentered)
        {
            backgroundImage.sprite = mechCenteredSprite;
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
        //actorName.text = currentDialogue.currentActor;
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
