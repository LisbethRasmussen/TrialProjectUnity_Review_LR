using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region Dialogue Selection Variables (Serialized by Custom Editor)
    // Dialogue Scriptable Object
    [SerializeField] private DialogueContainerSO dialogueContainer;
    [SerializeField] private DialogueGroupSO dialogueGroup;
    [SerializeField] private DialogueSO dialogue;

    // Filters
    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialogueOnly;

    // Indexes
    [SerializeField] private int selectedDialogueGroupIndex;
    [SerializeField] private int selectedDialogueIndex;
    #endregion

    private bool firstCall = true;
    private DialogueSO currentDialogue;

    private int lastDialogueIndex;
    private List<DialogueSO> dialoguesHistory;

    #region Initialization Methods
    private void Start()
    {
        ResetToFirstDialogue();
    }
    #endregion

    #region Getters

    public bool IsStartOfDialogue() => firstCall;

    public bool IsEndOfDialogue() => currentDialogue.Choices.Count == 0;

    public bool IsChoiceAvailable() => currentDialogue.Choices.Count > 1;

    public List<string> GetCurrentChoices()
    {
        if (IsChoiceAvailable())
        {
            Debug.LogWarning("Tried to get the choices but there are no choices available!");
            return null;
        }

        List<string> choices = new();
        foreach (var choice in currentDialogue.Choices)
        {
            choices.Add(choice.Text);
        }

        return choices;
    }

    #endregion

    #region Dialogue Control Methods

    /// <summary>
    /// Moves to the next dialogue and returns it.<br/>
    /// <strong>If this is the first time you're iterating on this dialogue, it will return the first dialogue.</strong> You don't have to call GetCurrent() for the first one.<br/>
    /// </summary>
    /// <param name="choiceNumber">Move with this choice index. This parameter is ignored if the current dialogue is single-choice.</param>
    /// <returns>Returns the dialogue if there is one. Returns null if you reached the end.</returns>
    public DialogueSO GetNext(int choice = 0)
    {
        if (firstCall)
        {
            // First call, return the current dialogue without moving, otherwise it would be skipped.
            firstCall = false;
            return currentDialogue;
        }

        MoveNext(choice);
        return currentDialogue;
    }

    /// <summary>
    /// Returns the current dialogue and move to the next.<br/>
    /// This returns the current one instead of GetNext() that returns the next.<br/>
    /// I don't recommend using this <strong>especially if you have multi-choice dialogue nodes</strong>. You'll be forced to select a choice before even displaying the dialogue.<br/>
    /// </summary>
    public DialogueSO GetCurrentNext(int choice = 0)
    {
        firstCall = false;
        DialogueSO current = currentDialogue;
        MoveNext(choice);
        return current;
    }

    /// <summary>
    /// Returns the current dialogue without advancing.<br/>
    /// </summary>
    public DialogueSO GetCurrent()
    {
        return currentDialogue;
    }

    /// <summary>
    /// Returns the previous dialogue by moving back and returning it.<br/>
    /// This method is useful for implementing a back button in your dialogue UI.<br/>
    /// It can be used as much as you want, but it will return null if you're at the first dialogue.<br/>
    /// </summary>
    /// <returns></returns>
    public DialogueSO GetBack()
    {
        // If there are no dialogues in the history, return null
        if (lastDialogueIndex < 0)
        {
            return null;
        }

        MoveBack();

        return currentDialogue;
    }
    #endregion

    #region Moving Helper Methods
    /// <summary>
    /// Moves the dialogue selection to the first dialogue. This will guarantees that the next GetNext() call returns the first dialogue.<br/>
    /// The first dialogue corresponds to the selected dialogue in the inspector.<br/>
    /// You don't need to call this method if this is the first time you're iterating through the dialogue.<br/>
    /// </summary>
    public void ResetToFirstDialogue()
    {
        firstCall = true;
        lastDialogueIndex = -1;
        currentDialogue = dialogue;
        dialoguesHistory = new List<DialogueSO>();
    }

    /// <summary>
    /// Moves to the next dialogue.<br/>
    /// </summary>
    /// <param name="choiceNumber">If you're currently at a multi-choice node, provide the selected option.</param>
    public void MoveNext(int choice = 0)
    {
        if (IsEndOfDialogue())
        {
            Debug.LogWarning("Tried to get the next dialogue but you reached the end!");
            return;
        }

        if (choice < 0 || choice >= currentDialogue.Choices.Count)
        {
            Debug.LogError($"Choice index was invalid! You choose the choice {choice} and there are {currentDialogue.Choices.Count}." +
                (choice < 0 ? "\nAnd no, negative indexes don't count..." : ""));
            return;
        }

        dialoguesHistory.Add(currentDialogue);
        lastDialogueIndex++;

        currentDialogue = currentDialogue.Choices[choice].NextDialogue;
    }

    /// <summary>
    /// Moves back to the previous dialogue. The history is written when the dialogue progresses.<br/>
    /// </summary>
    public void MoveBack()
    {
        if (lastDialogueIndex < 0)
        {
            Debug.LogWarning("There are no dialogues in the history to move back to.");
            return;
        }

        currentDialogue = dialoguesHistory[lastDialogueIndex];
        lastDialogueIndex--;
    }
    #endregion
}
