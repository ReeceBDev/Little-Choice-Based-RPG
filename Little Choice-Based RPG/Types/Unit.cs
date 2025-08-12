namespace Little_Choice_Based_RPG.Types
{
    /// <summary> An empty placeholder value, represented implicitly from this type itself having been declared. Inspired by F#'s Unit. Essentially a void which can be used anywhere. 
    /// Returns itself, therefore its value can only be used in an explicit Unit type declaration (prohibiting accidentally using it with whatever would be a its value's type). </summary>
    internal readonly record struct Unit
    {
        public static readonly Unit Value = new();
    }
}
