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
    public class Hostage : SerializableSFSType {
    
        public HostageType hostageType;
        public string hostageTypeAsString;
        
        /// /FOR NETWORKING
        //public Option<Bandit> capturedBy;
        public Bandit capturedBy;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Hostage() {}
        
        // hostageType
        public HostageType getHostageType() {
            return this.hostageType;
        }
        
        public void setHostageType(HostageType hostage) {
            this.hostageType = hostage;
        }
        
        // hostageTypeAsString
        public string getHostageTypeAsString() {
            return this.hostageTypeAsString;
        }
        
        public void setHostageTypeAsString(string hostage) {
            this.hostageTypeAsString = hostage;
        }
        
        // capturedBy
        public Bandit getCapturedBy() {
            //return this.capturedBy.value;
            return this.capturedBy;
        }
        
        public void setCapturedBy(Bandit capturedBy) {
            //this.capturedBy = Option<Bandit>.Some(capturedBy);
            this.capturedBy = capturedBy;
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
