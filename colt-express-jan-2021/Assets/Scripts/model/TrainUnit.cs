using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;


//The following code is executed right after creating the SmartFox object:
// using System.Reflection;
//        DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
namespace model {
    public class TrainUnit : SerializableSFSType
    {

        public string carTypeAsString; //public CarType carType;

        public TrainUnit above;
        public TrainUnit below;
        public TrainUnit left;
        public TrainUnit right;
        public TrainUnit beside;

        public bool isMarshalHere;
        public ArrayList banditsHere;
        public ArrayList lootHere;
        public ArrayList horsesHere;

        public TrainUnit() { }

    }
}