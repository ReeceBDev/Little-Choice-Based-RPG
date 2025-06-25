using Little_Choice_Based_RPG.Types.Interactions;

namespace Little_Choice_Based_RPG.Types.External.Services
{

    /// <summary> Lightweight data for clients, representing an interaction by an ID. This is delivered via services.
    /// Essentially a named ValueTuple.
    /// 
    /// InteractionID is an ID mapped to an interaction. This mapping is handled by the InteractionMapper class.
    /// SubjectID is the ID property which 'stores' the interaction.
    /// PresentationContext is an InteractionRole used to tell endpoints where to ideally show a role. This is discressionary and may be exclusionary. For more details see existing implementions for a given role.
    /// </summary>
    public readonly record struct InteractionServiceData(int InteractionID, uint SubjectID, InteractionRole PresentationContext);
}
