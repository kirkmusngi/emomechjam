﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueScript/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public CurrentDialogueBox focus;
    public CurrentActor currentActor;

    [TextArea(3, 10)]
    public string[] lines;

    public bool importantSegue;
    public Segue segueToCutTo;
    public Dialogue nextDialogue;
    public bool branchNext;
    public DialogueBranch branch;

    public bool switchToNextSceneAfterThis;
    public CurrentScene nextScene;


}

public enum CurrentDialogueBox
{
    PilotBox, ChildBox, MainBox, ApproachBox
}

public enum CurrentScene
{
    Menu, Prologue, Main
}

public enum Segue
{
    PresentMechPrologue, ToMainScene, Approach2, Approach3, Approach4, MechCentered
}

public enum CurrentActor
{
    OldManAngry, OldManShocked, OldManNeutral, IsabelleNeutral, IsabelleLaughing, IsabelleGlaring, IsabelleDiscovering
}