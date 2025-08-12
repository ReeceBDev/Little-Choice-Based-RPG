namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public sealed class PlayerInventoryService
    {
        public event EventHandler<uint> InventoryAddition; //Informs an endpoint that a new item ID was added to the player's inventory.
        public event EventHandler<uint> InventoryRemoval; //Informs an endpoint that an item ID was removed from the player's inventory.
        public event EventHandler InventoryRefreshRequired; //Instructs endpoints to re-request their cache.
    }
}
