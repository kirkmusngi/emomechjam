using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueScript/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public CurrentDialogueBox focus;
    public string currentActor;

    [TextArea(3, 10)]
    public string[] lines;

    public Dialogue nextDialogue;
    public bool branchNext;
    public DialogueBranch branch;

    public bool switchToNextSceneAfterThis;
    public CurrentScene nextScene;
}

public enum CurrentDialogueBox
{
    PilotBox, PrologueBox, MainBox
}

public enum CurrentScene
{
    Menu, Prologue, Main
}
