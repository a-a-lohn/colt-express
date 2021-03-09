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
    public class Round : SerializableSFSType {
    
        public RoundType roundType;  
        // = round.toString(); -- Not sure if this will work, may have to be done assigned after round is assigned
        public string roundTypeAsString;  
        // FOR NETWORKING
        public Turn currentTurn;  
        public int turnCounter;
        
        // Tracks the current turn
        public ArrayList turns = new ArrayList();
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Round() {}
        
        public void addTurn(Turn a) {
            if (this.turns.Contains(a)) {
                return;
            }
            
            this.turns.Add(a);
        }
        
        public void addTurnsAt(int index, Turn a) {
            this.turns.Insert(index, a);
        }
        
        public void removeTurnsAt(int index) {
            int size = this.turns.Count;
            if ((index < size)) {
                this.turns.Remove(index);
            }
            
        }
        
        public Turn getTurnAt(int index) {
            if ((index < this.turns.Count)) {
                return (Turn)this.turns[index];
            }
            
            return null;
        }
        
        public void removeTurn(Turn a) {
            if (this.turns.Contains(a)) {
                this.turns.Remove(a);
            }
            
        }
        
        public bool containsTurns(Turn a) {
            bool contains = this.turns.Contains(a);
            return contains;
        }
        
        public int sizeOfTurns() {
            int size = this.turns.Count;
            return size;
        }
        
        public ArrayList getTurns() {
            return this.turns;
        }
        
        public Turn getCurrentTurn() {
            return this.currentTurn;
        }
        
        public void setCurrentTurn(Turn newObject) {
            this.currentTurn = newObject;
        }
        
        public Turn getNextTurn() {
            return null;
        }
        
        public void setNextTurn() {
            this.turnCounter++;
            this.currentTurn = (Turn)this.turns[this.turnCounter];
        }
        
        public int getTurnCounter() {
            return this.turnCounter;
        }
        
        public void setTurnCounter(int i) {
            this.turnCounter = i;
        }
    }
}