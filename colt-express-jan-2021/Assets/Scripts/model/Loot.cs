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
    
        public Option<Bandit> belongsTo;       
        public Option<TrainUnit> position;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Loot() {}
        
        public Loot(Bandit b) {
            this.belongsTo = Option<Bandit>.Some(b);
            this.position = Option<TrainUnit>.None;
        }
        
        public Loot(TrainUnit pos) {
            this.position = Option<TrainUnit>.Some(pos);
            this.belongsTo = Option<Bandit>.None;
        }
        
        // belongsTo
        public Bandit getBelongsTo() {
            return this.belongsTo.value;
        }
        
        public void setBelongsTo(Bandit b) {
            System.Diagnostics.Trace.Assert(b != null);
            this.position = Option<TrainUnit>.None;
            this.belongsTo = Option<Bandit>.Some(b);
        }
        
        // position
        public TrainUnit getPosition() {
            return this.getPosition();
        }
        
        public void setPosition(TrainUnit pos) {
            System.Diagnostics.Trace.Assert(pos != null);
            this.belongsTo = Option<Bandit>.None;
            this.position = Option<TrainUnit>.Some(pos);
        }
        
        public void drop() {
            // TODO
        }
        
        public void pickup() {
            // TODO
        }

        // Customized Optional type !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public struct Option<T>
        {
            public static Option<T> None => default;
            public static Option<T> Some(T value) => new Option<T>(value);

            public readonly bool isSome;
            public readonly T value;

            Option(T value)
            {
                this.value = value;
                isSome = this.value is { };
            }

            public bool IsSome(out T value)
            {
                value = this.value;
                return isSome;
            }
        }

    }
}
