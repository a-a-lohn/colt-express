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
    public class Money : Loot, SerializableSFSType {
    
        public int value;    
        public MoneyType moneyType;
        public string moneyTypeAsString;
        
        // FOR NETWORKING
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Money() {}
        
        public Money(MoneyType mt, int value) {
            this.moneyType = mt;
            this.value = value;
        }
        
        // value
        public void setValue(int v) {
            this.value = v;
        }
        
        public int getValue() {
            return this.value;
        }
        
        // moneyType
        public void setMoneyType(MoneyType p) {
            this.moneyType = p;
        }
        
        public MoneyType getMoneyType() {
            return this.moneyType;
        }
        
        // moneyTypeAsString
        public string getMoneyTypeAsString() {
            return this.moneyTypeAsString;
        }
        
        public void setMoneyTypeAsString(string s) {
            this.moneyTypeAsString = s;
        }
    }
}
