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

namespace model {
    public abstract class Card : SerializableSFSType {
      
        public Bandit belongsTo;

        public Card() {}
        
       public Bandit getBelongsTo() {
           return this.belongsTo;
       }
        
<<<<<<< HEAD
       public bool setBelongsTo(Bandit newObject) {
=======
        public bool setBelongsTo(Bandit newObject) {
>>>>>>> a85e692a05bc960bb78a343e8db21c645f4fb5fe
            this.belongsTo = newObject;
            return true;
        }
    }
}