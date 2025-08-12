namespace Little_Choice_Based_RPG.External.Types
{
    /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
    public enum InteractionRole
    {
        Explore, //Default. Should show most of the time, such as when exploring or when opening an inspect context-menu.
        Inspect, //Should be applied upon inspecting an object and opening its context-menu.
        Navigation, //Should be applied to choices relating to moving around, such as Direction.
        System, //Should apply to anything relating to the meta system, for example, exiting the game, navigating to the options menu, and so on.

        //Unique
        SystemInventoryEntry //Only used by the inventory screen, to different inventory elements. Therefore, it doesn't need to be considered, ever.
    }

}
