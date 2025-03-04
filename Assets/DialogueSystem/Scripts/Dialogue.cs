using UnityEngine;

public class Dialogue : MonoBehaviour
{
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
}
