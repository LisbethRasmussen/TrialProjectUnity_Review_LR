using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Condition))]
public class ConditionInspector : Editor
{
    // Get all the properties of the Condition script
    private SerializedProperty _dialogueVariablesNamesSO;

    private SerializedProperty _conditionValueType;

    private SerializedProperty _boolKey;
    private SerializedProperty _boolComparisonType;
    private SerializedProperty _boolValue;

    private SerializedProperty _intKey;
    private SerializedProperty _intComparisonType;
    private SerializedProperty _intValue;

    private SerializedProperty _stringKey;
    private SerializedProperty _stringValue;

    private void OnEnable()
    {
        _dialogueVariablesNamesSO = serializedObject.FindProperty("_dialogueVariablesNamesSO");

        _conditionValueType = serializedObject.FindProperty("_conditionValueType");

        _boolKey = serializedObject.FindProperty("_boolKey");
        _boolComparisonType = serializedObject.FindProperty("_boolComparisonType");
        _boolValue = serializedObject.FindProperty("_boolValue");

        _intKey = serializedObject.FindProperty("_intKey");
        _intComparisonType = serializedObject.FindProperty("_intComparisionType");
        _intValue = serializedObject.FindProperty("_intValue");

        _stringKey = serializedObject.FindProperty("_stringKey");
        _stringValue = serializedObject.FindProperty("_stringValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialogue Variables Names", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_dialogueVariablesNamesSO);

        if (_dialogueVariablesNamesSO == null || _dialogueVariablesNamesSO.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Assign the DialogueVariableNamesSO scriptable object to get the variable names." +
                "To create one, Right Click -> Dialogue -> Condition Names.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Condition Type", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_conditionValueType);

        EditorGUILayout.Space(16);

        switch ((Condition.ValueType)_conditionValueType.enumValueIndex)
        {
            case Condition.ValueType.Bool:
                DrawDropdownForPropertyAndType(_boolKey, Condition.ValueType.Bool);
                EditorGUILayout.PropertyField(_boolComparisonType);
                EditorGUILayout.PropertyField(_boolValue);
                break;
            case Condition.ValueType.Int:
                // Display the three properties aside each
                DrawDropdownForPropertyAndType(_intKey, Condition.ValueType.Int);
                EditorGUILayout.PropertyField(_intComparisonType);
                EditorGUILayout.PropertyField(_intValue);
                break;
            case Condition.ValueType.String:
                DrawDropdownForPropertyAndType(_stringKey, Condition.ValueType.String);
                EditorGUILayout.PropertyField(_stringValue);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDropdownForPropertyAndType(SerializedProperty property, Condition.ValueType type)
    {
        // Get the names of the variables based on the type
        ConditionVariableNamesSO dialogueVariableNames = _dialogueVariablesNamesSO.objectReferenceValue as ConditionVariableNamesSO;
        string[] variableNames = dialogueVariableNames.GetVarNames(type);

        // If the variable names are not found, show a warning
        if (variableNames == null || variableNames.Length == 0)
        {
            EditorGUILayout.HelpBox($"No names were found for this type! Create one in the {dialogueVariableNames.name} object.", MessageType.Warning);
            return;
        }

        int selected = Array.IndexOf(variableNames, property.stringValue);
        selected = EditorGUILayout.Popup("Variable Name", selected, variableNames);
        selected = Mathf.Clamp(selected, 0, variableNames.Length - 1);

        property.stringValue = variableNames[selected];
    }
}
