using Little_Choice_Based_RPG.Types.Archive;

namespace Little_Choice_Based_RPG.Types
{
    /// <summary> A possible state or existence of a GameObject. Note: An ID without an EntityProperty will just be checked for being present. </summary>
    internal record struct EntityState(uint EntityReferenceID, List<EntityProperty>? RequiredProperties);
}
