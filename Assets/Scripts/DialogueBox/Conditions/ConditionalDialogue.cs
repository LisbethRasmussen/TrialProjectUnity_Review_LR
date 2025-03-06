using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDialogue : TriggerableDialogue
{
    [Header("Condition(s)")]
    [SerializeField] private List<Condition> _conditions;
    [SerializeField] private ConditionType _conditionsToBeMet;

    [Header("Dialogues")]
    [SerializeField] private TriggerableDialogue _dialogueOnTrue;
    [SerializeField] private TriggerableDialogue _dialogueOnFalse;

    public override void Trigger()
    {
        if (_conditions == null || _conditions.Count == 0)
        {
            Debug.LogWarning("No conditions set for the conditional dialogue.");
            return;
        }

        // Check that all conditions are met
        bool finalResult = false;

        if (_conditionsToBeMet == ConditionType.All)
        {
            finalResult = true;
            foreach (Condition condition in _conditions)
            {
                if (!condition.Evaluate())
                {
                    finalResult = false;
                    break;
                }
            }
        }
        else if (_conditionsToBeMet == ConditionType.Any)
        {
            finalResult = false;
            foreach (Condition condition in _conditions)
            {
                if (condition.Evaluate())
                {
                    finalResult = true;
                    break;
                }
            }
        }

        if (finalResult)
        {
            if (_dialogueOnTrue != null)
            {
                _dialogueOnTrue.Trigger();
            }
        }
        else
        {
            if (_dialogueOnFalse != null)
            {
                _dialogueOnFalse.Trigger();
            }
        }
    }

    [Serializable]
    public enum ConditionType
    {
        All,
        Any
    }
}
