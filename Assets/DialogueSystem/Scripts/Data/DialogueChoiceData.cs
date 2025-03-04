using UnityEngine;

public class DialogueChoiceData
{
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public DialogueSO NextDialogue { get; set; }

}
