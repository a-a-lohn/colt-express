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
    public class BulletCard : Card, SerializableSFSType {
    
        public bool fired;
        public string owner; // added 
        public int size; // added by annie

        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public BulletCard() {}

        public BulletCard(string belongsToAsString) {
            base.belongsToAsString = belongsToAsString;
            this.fired = false;
        }

        //fired
        public bool getFired(){
            return this.fired;
        }

        public void setFired(bool b){
            this.fired = b;
        }

        // added by annie
        public int getSize(){
            return this.size; 
        }
    }
}
