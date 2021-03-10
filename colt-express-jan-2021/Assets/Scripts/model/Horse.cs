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
    public class Horse : SerializableSFSType {
    
        public TrainUnit adjacentTo;
        //public Option<Bandit> riddenBy;
        public Bandit riddenBy;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Horse(){}       
        // adjacentTo
        public TrainUnit getAdjacentTo() {
            return this.adjacentTo;
        }
        
        public void setAdjacentTo(TrainUnit adjacentTo) {
            this.adjacentTo = adjacentTo;
        }     
        // riddenBy
        public Bandit getRiddenBy() {
            //return this.riddenBy.value;
            return this.riddenBy;
        }      
        public void setRiddenBy(Bandit b) {
            //this.riddenBy = Option<Bandit>.Some(b);
            this.riddenBy = b;
        }

        // Customized Optional type 

        // public struct Option<T>
        // {
        //     public static Option<T> None => default;
        //     public static Option<T> Some(T value) => new Option<T>(value);

        //     public readonly bool isSome;
        //     public readonly T value;

        //     Option(T value)
        //     {
        //         this.value = value;
        //         isSome = this.value is { };
        //     }

        //     public bool IsSome(out T value)
        //     {
        //         value = this.value;
        //         return isSome;
        //     }
        // }


    }

}
