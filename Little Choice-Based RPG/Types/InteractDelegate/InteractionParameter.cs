using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.InteractDelegate
{
    /// <summary> Describes parameters and what they should be sourced from. </summary>
    public enum InteractionParameter
    {
        //Syntax: Target_Type -- The type will be used by the delegate/method. The target tells the ChoiceHandler where to retrieve that type. 
        // i.e. AllNearby_MetalCabinets_List_GameObject should return a list of gameobject that match the target description (List<GameObject>).

        //General GameObject
        Source_GameObject,
        Target_GameObject, //Target means asking the player to decide what to put in. // Issue: how to differentiate roomtarget or inventorytargets

        //General PropertyHandler
        Source_PropertyHandler,
        Target_PropertyHandler,

        //Flitered PropertyHandler
        // -- Filters apply to the target. 
        // -- i.e. Target_PropertyHandler_Filtered would mean a propertyhandler from a target gameobject in the local room filtered by having a list of properties.
        Target_PropertyHandler_Filtered
    }
}
