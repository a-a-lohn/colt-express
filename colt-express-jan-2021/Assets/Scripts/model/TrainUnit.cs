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

        public string strCarType; //public CarType carType;
        public TrainUnit otherfloor;
        public Marshal marshalHere;
        public ArrayList banditPositions;
        public ArrayList lootInCabin;
        public ArrayList adjacent;
        public ArrayList horses;

        public TrainUnit() { }

    }
}