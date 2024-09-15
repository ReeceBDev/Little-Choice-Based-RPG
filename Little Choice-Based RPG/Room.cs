using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Little_Choice_Based_RPG
{
    internal struct RoomDescriptor
        {
        public string generic;
        public string initial;
        public string distant;
        public string flair;
    }

    // handles rooms, each room has a scenic description, a description of what it looks like from other rooms, a list of objects that are sitting inside it, currently accessible directions to other rooms, visibility (i.e. fogginess) 0-4 clear (can view infinitely far), light fog (can view 3 far), medium fog (can view 2 far), heavy fog (can view 1 far), dark (cant view)
    internal class Room
    {
        static uint currentID = 0;
        uint uniqueID = 0;
        uint directionNorth;
        uint directionEast;
        uint directionSouth;
        uint directionWest;

        int physicalVisibility;

        string name;
        RoomDescriptor descriptors;
        
        bool hasPlayerVisited;
        bool isNorthTraversable = false;
        bool isEastTraversable = false;
        bool isSouthTraversable = false;
        bool isWestTraversable = false;

        List<GenericObject> contents;
        public Room(string name, string newGenericDescriptor, string newInitialDescriptor, string newDistantDescriptor,
            uint directionNorth = 0, uint directionEast = 0, uint directionSouth = 0, uint directionWest = 0, int visibility = 3)
        {
            this.uniqueID = ++currentID;
            this.name = name;

            this.descriptors.generic = newGenericDescriptor;
            this.descriptors.initial = newInitialDescriptor;
            this.descriptors.distant = newDistantDescriptor;

            this.directionNorth = directionNorth;
            this.directionEast = directionEast;
            this.directionSouth = directionSouth;
            this.directionWest = directionWest;
            this.physicalVisibility = visibility;

            if (directionNorth > 0) this.isNorthTraversable = true;
            if (directionEast > 0) this.isEastTraversable = true;
            if (directionSouth > 0) this.isSouthTraversable = true;
            if (directionWest > 0) this.isWestTraversable = true;
        }
    }
}
