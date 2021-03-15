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
    public class TrainUnit : SerializableSFSType {
    
        //public static int trainLength;
        //public ArrayList train;
        //public static TrainUnit[] stagecoach;
        //public static TrainUnit[] trainRoof;
	    //public static TrainUnit[] trainCabin;
        //public string carType;
        //public string carFloor;
        public string carTypeAsString;
        public string carFloorAsString;
        public TrainUnit above ;
        public TrainUnit below ;
        public TrainUnit left ;
        public TrainUnit right ;
        public TrainUnit beside ;
        
        // Used for stagecoach and it's adjacent car ONLY.
        public bool isMarshalHere ;
        public ArrayList banditsHere ;
        public ArrayList lootHere ;
        public ArrayList horsesHere ;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public TrainUnit() {}
        
        // private TrainUnit(string carType, string carFloor) {
        //     //this.carType = carType;
        //     //this.carFloor = carFloor;
        //     this.carTypeAsString = carType;
        //     this.carFloorAsString = carFloor;
        //     // TODO: createGraphic()
        // }
        
        // public static TrainUnit[,] createTrain(int numberOfBandits) {
        //     // Create one car for each player, +1 to account for the locomotive
        //     int trainLength = (numberOfBandits + 1);
        //     TrainUnit[,] train = new TrainUnit[2,trainLength];
        //     TrainUnit locoCabin = new TrainUnit("LOCOMOTIVE", "CABIN");
        //     TrainUnit locoRoof = new TrainUnit("LOCOMOTIVE", "ROOF");
        //     // Create locomotive
        //     locoCabin.above = locoRoof;
        //     locoCabin.isMarshalHere = true;
        //     Marshal m = Marshal.getInstance();
        //     m.setMarshalPosition(locoCabin);
        //     // TODO: put strongbox
        //     // locoRoof.below = Optional.of(locoCabin);
        //     locoRoof.below = locoCabin;
        //     // TODO: Add locomotive to array
        //     // TODO: Create rest of the cars, associate with each other, add to array
        //     // TODO: Associate locomotive to front car
        //     TrainUnit.trainLength = trainLength;
        //     TrainUnit.train = train;
        //     return train;
        // }
        
        // public static TrainUnit[] createStagecoach() {
        //     TrainUnit[] stagecoach = new TrainUnit[2];
        //     TrainUnit cabin = new TrainUnit("STAGECOACH", "CABIN");
        //     TrainUnit roof = new TrainUnit("STAGECOACH", "ROOF");
        //     stagecoach[0] = roof;
        //     stagecoach[1] = cabin;
        //     return stagecoach;
        // }
        
        // carType
        // public string getCarType() {
        //     return this.carType;
        // }
        
        // public void setCarType(string type) {
        //     this.carType = type;
        // }
        
        // carTypeAsString
        public string getCarTypeAsString() {
            return this.carTypeAsString;
        }
        
        public void setCarTypeAsString(string type) {
            this.carTypeAsString = type;
        }
        
        // carFloor
        // public string getCarFloor() {
        //     return this.carFloor;
        // }
        
        // public void setCarFloor(string floor) {
        //     this.carFloor = floor;
        // }
        
        // carFloorAsString
        public string getCarFloorAsString() {
            return this.getCarFloorAsString();
        }
        
        public void setCarFloorAsString(string floor) {
            this.carFloorAsString = floor;
        }
        
        // // trainLength
        // public static int getTrainLength() {
        //     return TrainUnit.trainLength;
        // }
        
        // public static void setTrainLength(int length) {
        //     TrainUnit.trainLength = length;
        // }
        
        // // train
        // public ArrayList getTrain() {
        //     return train;
        // }
        
        // // stagecoach
        // public static TrainUnit[] getStagecoach() {
        //     return TrainUnit.stagecoach;
        // }
        
        public TrainUnit getAbove() {
            return this.above;
        }
        
        public void setAbove(TrainUnit otherTrainUnit) {
            this.above = otherTrainUnit;
            otherTrainUnit.below = this;
        }
        
        public TrainUnit getBelow() {
            return this.below;
        }
        
        public void setBelow(TrainUnit otherTrainUnit) {
            this.below = otherTrainUnit;
            otherTrainUnit.above = this;
        }
        
        public TrainUnit getRight() {
            return this.right;
        }
        
        public void setRight(TrainUnit otherTrainUnit) {
            this.right = otherTrainUnit;
            otherTrainUnit.left = this;
        }
        
        public TrainUnit getLeft() {
            return this.left;
        }
        
        public void setLeft(TrainUnit otherTrainUnit) {
            this.left = otherTrainUnit;
            otherTrainUnit.right = this;
        }
        
        public TrainUnit getBeside() {
            return this.beside;
        }
        
        public bool isAdjacentTo(TrainUnit otherTrainUnit) {
            bool adjacentTo = false;
            if (((this.below == otherTrainUnit) 
                        && (otherTrainUnit.above == this))) {
                adjacentTo = true;
            }
            else if (((this.above == otherTrainUnit) 
                        && (otherTrainUnit.below == this))) {
                adjacentTo = true;
            }
            else if (((this.right == otherTrainUnit) 
                        && (otherTrainUnit.left == this))) {
                adjacentTo = true;
            }
            else if (((this.left == otherTrainUnit) 
                        && (otherTrainUnit.right == this))) {
                adjacentTo = true;
            }
            else if (((this.beside == otherTrainUnit) 
                        && (otherTrainUnit.beside == this))) {
                adjacentTo = true;
            }
            
            return adjacentTo;
        }
        
        public void addBandit(Bandit b) {
            System.Diagnostics.Trace.Assert(!this.banditsHere.Contains(b));
            this.banditsHere.Add(b);
        }
        
        public void removeBandit(Bandit b) {
            System.Diagnostics.Trace.Assert(banditsHere.Contains(b));
            this.banditsHere.Remove(b);
        }
        
        public bool containsBandit(Bandit b) {
            return this.banditsHere.Contains(b);
        }
        
        public int numOfBanditsHere() {
            return this.banditsHere.Count;
        }
        
        public ArrayList getBanditsHere() {
            //query gm hashtable
            return (ArrayList)banditsHere.Clone();
        }
        
        public void addLoot(Loot a) {
            System.Diagnostics.Trace.Assert(!this.lootHere.Contains(a));
            this.lootHere.Add(a);
        }
        
        public void removeLoot(Loot a) {
            System.Diagnostics.Trace.Assert(this.lootHere.Contains(a));
            this.lootHere.Remove(a);
        }
        
        bool containsLoot(Loot a) {
            return this.lootHere.Contains(a);
        }
        
        int numOfLootHere() {
            return this.lootHere.Count;
        }
        
        public bool getIsMarshalHere() {
            return this.isMarshalHere;
        }
        
        public void setIsMarshalHere(bool b) {
            this.isMarshalHere = b;
        }
        
        public void moveMarshalTo(TrainUnit dest) {
            this.isMarshalHere = false;
            dest.isMarshalHere = true;
        }
    }
}