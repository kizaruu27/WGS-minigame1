using System;

public sealed class M1_PreserveAttribute : Attribute
{
    public bool AllMembers;
    public bool Conditional;
    public M1_PreserveAttribute(bool allMembers, bool conditional)
    {
        AllMembers = allMembers;
        Conditional = conditional;
    }
    public M1_PreserveAttribute() { }
}