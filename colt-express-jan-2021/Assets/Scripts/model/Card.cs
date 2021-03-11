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
        
        Bandit getBelongsTo() {
          return this.belongsTo;
       }
        
        public bool setBelongsTo(Bandit newObject) {
            this.belongsTo = newObject;
            return true;
        }
    }
}