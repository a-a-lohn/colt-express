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
    public abstract class Loot : SerializableSFSType {

        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Loot() {}
        
        public Loot(Bandit b) {
            this.belongsTo = b;
            this.position = default;
        }
        
        public Loot(TrainUnit pos) {
            this.position = pos;
            this.belongsTo = default;
        }
        
        // belongsTo
        public Bandit getBelongsTo() {
            return this.belongsTo;
        }
        
        public void setBelongsTo(Bandit b) {
            System.Diagnostics.Trace.Assert(b != null);
            this.position = default;
            this.belongsTo = b;
        }
        
        // position
        public TrainUnit getPosition() {
            return this.getPosition();
        }
        
        public void setPosition(TrainUnit pos) {
            System.Diagnostics.Trace.Assert(pos != null);
            this.belongsTo = default;
            this.position = pos;
        }
        
        public void drop() {
            // TODO
        }
        
        public void pickup() {
            // TODO
        }

        public Bandit getBelongsTo(){
            return new Bandit("BELLE");
        }
    }
}
