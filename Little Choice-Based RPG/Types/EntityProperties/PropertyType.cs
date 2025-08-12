namespace Little_Choice_Based_RPG.Types.EntityProperties
{
    /// <summary> 
    /// Defines all possible types that may be contained within a property. Properties only contain one PropertyType each. 
    /// PropertyTypes must have value semantics, and not act like references! Reference types should instead be put in a PropertyExtension. 
    /// </summary>
    internal enum PropertyType
    {
        Boolean,
        String,
        UInt32,
        Decimal,
    }
}
