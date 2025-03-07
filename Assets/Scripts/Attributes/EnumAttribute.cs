using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class EnumAttribute : PropertyAttribute
{
    public string[] StringValues { get; }
    public int[] IntValues { get; }

    public EnumAttribute(params string[] values) => StringValues = values;
    public EnumAttribute(params int[] values) => IntValues = values;
}
