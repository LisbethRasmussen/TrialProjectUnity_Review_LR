using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public ReadOnlyAttribute()
    {
    }
}
