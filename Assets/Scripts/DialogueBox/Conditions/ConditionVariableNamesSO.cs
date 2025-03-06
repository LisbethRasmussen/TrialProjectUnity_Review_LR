using UnityEngine;

[CreateAssetMenu(fileName = "DialogueVariableNames", menuName = "Dialogue/Conditions Names", order = 1)]
public class ConditionVariableNamesSO : ScriptableObject
{
    [field: SerializeField] public string[] BoolVarNames { get; private set; }
    [field: SerializeField] public string[] IntVarNames { get; private set; }
    [field: SerializeField] public string[] StringVarNames { get; private set; }

    public string[] GetVarNames(Condition.ValueType valueType)
    {
        return valueType switch
        {
            Condition.ValueType.Bool => BoolVarNames,
            Condition.ValueType.Int => IntVarNames,
            Condition.ValueType.String => StringVarNames,
            _ => null,
        };
    }
}
