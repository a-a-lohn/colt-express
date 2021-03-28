using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;
using Random = System.Random;

//The following code is executed right after creating the SmartFox object:
// using System.Reflection;
//        DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
namespace model {

    public class Bandit : SerializableSFSType {
    
        public string characterAsString;
        public string hostageAsString;
        public ArrayList loot;      
        public ArrayList bullets;        
        public ArrayList deck;
        public ArrayList hand;
        public ActionCard toResolve;
        public int consecutiveTurnCounter;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Bandit() {}

        public Bandit(string c) {
            this.characterAsString = c;
            this.hostageAsString = null;
            this.loot = new ArrayList();
            this.bullets = new ArrayList();
            this.deck = new ArrayList();
            this.hand = new ArrayList();
            this.toResolve = null;
            this.consecutiveTurnCounter = 0;
        }
        
        /**
        *  --GETTERS AND SETTERS--
        */

        //character
        public string getCharacter() {
            return this.characterAsString;
        }
        
        //position
        public TrainUnit getPosition() {
            GameManager gm = GameManager.getInstance();
            TrainUnit pos = (TrainUnit)gm.banditPositions[this];
            return (TrainUnit)pos;
        }
        public void setPosition(TrainUnit newObject) {
            //change hashmap of bandit-position
            GameManager gm = GameManager.getInstance();
            gm.banditPositions[this] = newObject;
        }
        
        //loot
        public Loot getLootAt(int index) {
            if ((this.loot.Count > index)) {
                Loot a = (Loot)this.loot[index];
                return a;
            }
            return null;
        }
        public void addLoot(Loot a) {
            bool contains = this.loot.Contains(a);
            if (contains) {
                return;
            }
            this.loot.Add(a);
        }
        public void removeLoot(Loot a) {
            if (this.loot.Contains(a)) {
                this.loot.Remove(a);
            }
        }
        public ArrayList getLoot() {
            return this.loot;
        }
        
        //deck
        public void addToDeckAt(int index, Card a) {
            bool contains = this.deck.Contains(a);
            if (contains) {
                return;
            }
            this.deck.Insert(index, a);
        }
        public void removeFromDeckAt(int index) {
            if ((this.deck.Count > index)) {
                this.deck.Remove(index);
            }
        }
        public Card getFromDeckAt(int index) {
            if ((this.deck.Count > index)) {
                return (Card)this.deck[index];
            }
            return null;
        }
        public void addToDeck(Card a) {
            this.deck.Add(a);
        }
        public void removeFromDeck(Card a) {
            this.deck.Remove(a);
        }
        public int sizeOfDeck() {
            int size = this.deck.Count;
            return size;
        }
        public ArrayList getDeck() {
            return this.deck;
        }
        
        //hand
        public void addToHand(Card a) {
            bool contains = this.hand.Contains(a);
            if (contains) {
                return;
            }
            this.hand.Add(a);
        }
        public void removeFromHand(Card a) {
            if (this.hand.Contains(a)) {
                this.hand.Remove(a);
            }
        }
        public int sizeOfHand() {
            int size = this.hand.Count;
            return size;
        }
        public ArrayList getHand() {
            return this.hand;
        }
        
        //bullets
        public void addBullet(BulletCard b){
            this.bullets.Add(b);
        }
        public BulletCard popBullet(){
            BulletCard popped = (BulletCard) this.bullets[this.bullets.Count-1];
            this.bullets.Remove(popped);
            return popped;
        }
        public int getSizeOfBullets(){
            return this.bullets.Count;
        }
        
        //hostage
        public string getHostageAsString() {
            return this.hostageAsString;
        }
        
        public void setHostageAsString(string hostage) {
            this.hostageAsString = hostage;
        }

        //toResolve
        public ActionCard getToResolve() {
        return this.toResolve;
        }
        public void setToResolve(ActionCard ac) {
            this.toResolve = ac;
        }

        //consecutiveTurnCounter
        public int getConsecutiveTurnCounter() {
		    return this.consecutiveTurnCounter;
	    }
	    public void setConsecutiveTurnCounter(int i) {
		    this.consecutiveTurnCounter = i;
	    }
        

        //initializing methods
        public void createStartingCards() {
            string[] actions= {"MOVE", "MOVE", "CHANGE_FLOOR","CHANGE_FLOOR", "MOVE_MARSHAL", "PUNCH", "ROB", "ROB", "SHOOT","SHOOT"};
            for (int i = 0; i<actions.Length; i++) {
                Card c = new ActionCard (actions[i], this.characterAsString);
                this.deck.Add(c);
            }
        }
        
        public void createHand() {
            this.shuffle();
            for (int i = 0; (i < 6); i++) {
                Card c = (Card)this.deck[0];
                this.hand.Add(c);
                this.deck.Remove(c);
            }
            
        }
        
        public void createBulletCards() {
            BulletCard bc1 = new BulletCard(this.characterAsString);
            BulletCard bc2 = new BulletCard(this.characterAsString);
            BulletCard bc3 = new BulletCard(this.characterAsString);
            BulletCard bc4 = new BulletCard(this.characterAsString);
            BulletCard bc5 = new BulletCard(this.characterAsString);
            BulletCard bc6 = new BulletCard(this.characterAsString);
            this.bullets.Add(bc1);
            this.bullets.Add(bc2);
            this.bullets.Add(bc3);
            this.bullets.Add(bc4);
            this.bullets.Add(bc5);
            this.bullets.Add(bc6);
        }
        
        public void createStartingPurse() {
            Money startingPurse = new Money("PURSE", 250); 
            this.loot.Add(startingPurse);
        }

        //TODO: combine this game manager
        public void clearHand(){
            foreach (Card c in this.hand){
                this.deck.Add(c);
            }
            this.hand.Clear();
        }

     // FIX THIS
        public void shuffle() {
            System.Random RandomGen = new System.Random(DateTime.Now.Millisecond);
            ArrayList ScrambledList = new ArrayList();
            Int32 Index;
            while (this.deck.Count > 0)
            {
                Index = RandomGen.Next(this.deck.Count);
                ScrambledList.Add(this.deck[Index]);
                this.deck.RemoveAt(Index);
            }
            this.deck = ScrambledList;
        } 

        public static ArrayList shuffle(ArrayList c) {
            c = (ArrayList) c.Clone();
            Random RandomGen = new Random(DateTime.Now.Millisecond);
            ArrayList ScrambledList = new ArrayList();
            Int32 Index;
            while (c.Count > 0)
            {
                Index = RandomGen.Next(c.Count);
                ScrambledList.Add(c[Index]);
                c.RemoveAt(Index);
            }
            return ScrambledList;
        }

        //added these to clean up the gamemanager methods, there's some logic that can be handled here 

        public void drawCards(int cardsToDraw) {
            for (int i = this.sizeOfDeck()-1; i > this.sizeOfDeck()-cardsToDraw-1; i--) {
                Card toAdd = this.getFromDeckAt(i);
                this.removeFromDeckAt(i);
                this.addToHand(toAdd);
            }
        }




    }

}