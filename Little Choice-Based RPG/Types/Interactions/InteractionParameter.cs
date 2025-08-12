namespace Little_Choice_Based_RPG.Types.Interactions
{
    /// <summary> Describes parameters and what they should be sourced from. </summary>
    internal enum InteractionParameter
    {
        //Syntax: Target_Type -- The type will be used by the delegate/method. The target tells the ChoiceHandler where to retrieve that type. 
        // i.e. AllNearby_MetalCabinets_List_GameObject should return a list of gameobject that match the target description (List<GameObject>).

        //General GameObject
        Target_GameObject, //Target means asking the player to decide what to put in. // Issue: how to differentiate roomtarget or inventorytargets

        //Flitered
        // -- Filters apply to the target. 
        // -- i.e. Target_GameObject_IsRepairTool_True would mean a target GameObject in the local room, filtered by being a repair tool.
        // Filters may request multiple properties at once.
        Target_GameObject_IsRepairTool_True,
        Target_GameObject_RepairToolSize_Medium //Must be a repair tool, and must by of the right size to repair mechs and cars.
    }
}
