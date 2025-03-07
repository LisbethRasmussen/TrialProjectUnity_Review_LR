using UnityEngine;

public class ConditionInitializer : MonoBehaviour
{
    [SerializeField] private bool _overrideAnimation;
    [ShowIf(nameof(_overrideAnimation)), Range(0, 5f)]
    [SerializeField] private float _animationTimer;
    [ShowIf(nameof(_overrideAnimation)), Range(0, 5f)]
    [SerializeField] private float _animationSmoothness;
    [ShowIf(nameof(_overrideAnimation), true, invert: true)]
    [SerializeField] private ConditionVariableNamesSO _dialogueVariablesNamesSO;
    [ShowIf(nameof(_overrideAnimation), true, invert: true)]
    [SerializeField] private ConditionVariableNamesSO _dialogueVariablesNamesS;
    [ShowIf(nameof(_overrideAnimation), true, invert: true)]
    [SerializeField] private ConditionVariableNamesSO _dialogueVariablesNames;

    [Enum("Test")]
    [SerializeField] private string test;
    [Enum("Test", "Test1")]
    [SerializeField] private string test1;
    [Enum(1, 2, 3, 4, 5, 6, 7)]
    [SerializeField] private int test2;
    [Enum("Use Controller", "Use Animator")]
    [SerializeField] private bool test3;
    [ShowIf(nameof(test3))]
    [SerializeField] private string animatorKeys;

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
