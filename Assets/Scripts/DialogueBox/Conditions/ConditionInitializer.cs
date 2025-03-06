using UnityEngine;

public class ConditionInitializer : MonoBehaviour
{
    [SerializeField] private ConditionVariableNamesSO _dialogueVariablesNamesSO;

    private void Awake()
    {
        if (_dialogueVariablesNamesSO == null)
        {
            Debug.LogError("DialogueVariablesNamesSO not assigned in the ConditionInitializer script.");
        }
        else
        {
            DialogueVariables.SetDialogueVariablesNamesSO(_dialogueVariablesNamesSO);
        }
    }
}
