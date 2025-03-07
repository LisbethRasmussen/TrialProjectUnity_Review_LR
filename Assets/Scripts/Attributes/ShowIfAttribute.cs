using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public class ShowIfAttribute : PropertyAttribute
{
    public readonly string VariableName;
    public readonly bool DisableInsteadOfHidding;
    public readonly bool Invert;

    public ShowIfAttribute(string variableName, bool disableInsteadOfHidding = false, bool invert = false)
    {
        VariableName = variableName;
        DisableInsteadOfHidding = disableInsteadOfHidding;
        Invert = invert;
    }
}
