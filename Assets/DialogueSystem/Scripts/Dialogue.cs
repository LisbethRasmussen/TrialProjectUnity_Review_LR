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

    #region Initialization Methods
    private void Start()
    {
        currentDialogue = dialogue;
    }
    #endregion

    #region Dialogue Control Methods

    /// <summary>
    /// Moves to the next dialogue and returns it.<br/>
    /// <strong>If this is the first time you're iterating on this dialogue, it will return the first dialogue.</strong> You don't have to call GetCurrent() for the first one.<br/>
    /// </summary>
    /// <param name="choiceNumber">Move with this choice index. This parameter is ignored if the current dialogue is single-choice.</param>
    public DialogueSO GetNext(int choice = 0)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns the current dialogue and move to the next.<br/>
    /// This returns the current one instead of GetNext() that returns the next.<br/>
    /// I don't recommend using this <strong>especially if you have multi-choice dialogue nodes</strong>. You'll be forced to select a choice before even displaying the dialogue.<br/>
    /// </summary>
    public DialogueSO GetCurrentNext(int choice = 0)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns the current dialogue without advancing.<br/>
    /// </summary>
    public DialogueSO GetCurrent()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns the previous dialogue by moving back and returning it.<br/>
    /// This method is useful for implementing a back button in your dialogue UI.<br/>
    /// It can be used as much as you want, but it will return null if you're at the first dialogue.<br/>
    /// </summary>
    /// <returns></returns>
    public DialogueSO GetBack()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Moves the dialogue selection to the first dialogue. This will guarantees that the next GetNext() call returns the first dialogue.<br/>
    /// The first dialogue corresponds to the selected dialogue in the inspector.<br/>
    /// You don't need to call this method if this is the first time you're iterating through the dialogue.<br/>
    /// </summary>
    public void MoveToFirstDialogue()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Moves to the next dialogue.<br/>
    /// </summary>
    /// <param name="choiceNumber">If you're currently at a multi-choice node, provide the selected option.</param>
    public void MoveNext(int choice = 0)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
