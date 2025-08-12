namespace Little_Choice_Based_RPG.External.Types.Services
{

    /// <summary> Lightweight data for clients, representing an interaction by an ID. This is delivered via services.
    /// Essentially a named ValueTuple.
    /// 
    /// InteractionID is an ID belonging to a single instance of an interaction. 
    /// InteractionTitle is the string displayed to the user, representing the interaction.
    /// PresentationContext is an InteractionRole used to tell endpoints where to ideally show a role. This is discressionary and may be exclusionary. For more details see existing implementions for a given role.
    /// IsAdded is a boolean used to tell endpoints if an interaction is to be added or removed. True is add, false is remove.
    /// </summary>
    public readonly record struct InteractionServiceData(ulong InteractionID, string InteractionTitle, string PresentationContext, bool IsAdded);
}
