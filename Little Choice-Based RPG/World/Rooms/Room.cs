﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using Little_Choice_Based_RPG.Objects.Base;

namespace Little_Choice_Based_RPG.World.Rooms
{
    internal struct RoomDescriptor
    {
        public string generic;
        public string initial;
        public string distant;
        public string flair;
    }

    // handles rooms, each room has a scenic description, a description of what it looks like from other rooms, a list of objects that are sitting inside it, currently accessible directions to other rooms, visibility (i.e. fogginess) 0-4 clear (can view infinitely far), light fog (can view 3 far), medium fog (can view 2 far), heavy fog (can view 1 far), dark (cant view)
    public class Room
    {
        static uint currentID = 0;
        uint uniqueID = 0;
        uint directionNorth;
        uint directionEast;
        uint directionSouth;
        uint directionWest;

        string name;
        RoomDescriptor descriptors;

        bool hasPlayerVisited;
        bool isNorthTraversable = false;
        bool isEastTraversable = false;
        bool isSouthTraversable = false;
        bool isWestTraversable = false;

        List<DescriptiveObject> contents = [];


        public Room(string name, string newGenericDescriptor, string newInitialDescriptor = "", string newDistantDescriptor = "",
            uint directionNorth = 0, uint directionEast = 0, uint directionSouth = 0, uint directionWest = 0, int visibility = 3)
        {
            uniqueID = ++currentID;
            this.name = name;

            this.directionNorth = directionNorth;
            this.directionEast = directionEast;
            this.directionSouth = directionSouth;
            this.directionWest = directionWest;
            this.physicalVisibility = visibility;

            if (directionNorth > 0) isNorthTraversable = true;
            if (directionEast > 0) isEastTraversable = true;
            if (directionSouth > 0) isSouthTraversable = true;
            if (directionWest > 0) isWestTraversable = true;

            descriptors.generic = newGenericDescriptor;

            if (newInitialDescriptor == "")
                descriptors.initial = newGenericDescriptor; //if Initial is empty, copy Generic
            else
                descriptors.initial = newInitialDescriptor;

            if (newDistantDescriptor == "")
                descriptors.distant = newGenericDescriptor; //if Distant is empty, copy Generic
            else
                descriptors.distant = newDistantDescriptor;
        }

        public uint ID => uniqueID;
        public int physicalVisibility { get; private protected init; }
    }
}
