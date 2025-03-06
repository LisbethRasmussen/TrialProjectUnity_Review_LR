using System;
using UnityEngine;

[Serializable]
public class Condition : MonoBehaviour
{
    [SerializeField] private ConditionVariableNamesSO _dialogueVariablesNamesSO;

    [SerializeField] private ValueType _conditionValueType;
    [Space]
    [SerializeField] private string _boolKey;
    [SerializeField] private BoolComparisonType _boolComparisonType;
    [SerializeField] private bool _boolValue;
    [Space]
    [SerializeField] private string _intKey;
    [SerializeField] private IntComparisonType _intComparisionType;
    [SerializeField] private int _intValue;
    [Space]
    [SerializeField] private string _stringKey;
    [SerializeField] private string _stringValue;

    public bool Evaluate()
    {
        switch (_conditionValueType)
        {
            case ValueType.Bool:
                bool? value = DialogueVariables.GetBool(_boolKey);

                if (value != null)
                {
                    // Swtich return according to the bool comparison
                    return _boolComparisonType switch
                    {
                        BoolComparisonType.Is => (bool)value == _boolValue,
                        BoolComparisonType.And => (bool)value && _boolValue,
                        BoolComparisonType.Or => (bool)value || _boolValue,
                        BoolComparisonType.Xor => (bool)value ^ _boolValue,
                        _ => false
                    };
                }

                Debug.LogError($"Bool variable with key '{_boolKey}' not found.");
                break;

            case ValueType.Int:
                int? intValue = DialogueVariables.GetInt(_intKey);

                if (intValue != null)
                {
                    // Swtich return according to the int comparison
                    return _intComparisionType switch
                    {
                        IntComparisonType.Equal => intValue == _intValue,
                        IntComparisonType.NotEqual => intValue != _intValue,
                        IntComparisonType.Greater => intValue > _intValue,
                        IntComparisonType.GreaterOrEqual => intValue >= _intValue,
                        IntComparisonType.Less => intValue < _intValue,
                        IntComparisonType.LessOrEqual => intValue <= _intValue,
                        _ => false
                    };
                }

                Debug.LogError($"Int variable with key '{_intKey}' not found.");
                break;

            case ValueType.String:
                string stringValue = DialogueVariables.GetString(_stringKey);

                if (stringValue != null)
                {
                    return stringValue == _stringValue;
                }

                Debug.LogError($"String variable with key '{_stringKey}' not found.");
                break;
        }

        return false;
    }

    public enum ValueType
    {
        Bool,
        Int,
        String
    }


    public enum BoolComparisonType
    {
        Is,
        And,
        Or,
        Xor
    }

    public enum IntComparisonType
    {
        Equal,
        NotEqual,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual
    }
}
