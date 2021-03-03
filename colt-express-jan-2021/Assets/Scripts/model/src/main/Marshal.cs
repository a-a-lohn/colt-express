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
    public class Marshal : SerializableSFSType
    {

        public TrainUnit marshalPosition;
        // STATIC FIELDS ARE NOT SERIALIZED
        public static Marshal instance;
        
        public Marshal() {
            
        }
        
        TrainUnit getMarshalPosition() {
            return this.marshalPosition;
        }
        
        bool setMarshalPosition(TrainUnit newObject) {
            this.marshalPosition = newObject;
            return true;
        }
    }
}
