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
    
        public bool getsAnotherAction;       
        public bool playedThisTurn;       
        //public string banditName;       
        public string banditNameAsString;
        
        // FOR NETWORKING
        //public Hostage hostage;       
        public string hostageAsString;
        
        // FOR NETWORKING
        public ArrayList loot ;      
        public ArrayList bullets ;        
        public ArrayList deck ;
        public ArrayList hand ;
        public ArrayList discardPile ;
        public ActionCard toResolve;
        public int consecutiveTurnCounter;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Bandit() {}

        public Bandit(string c) {
            //this.banditName = c;
            this.banditNameAsString = c;
            this.getsAnotherAction = false;
            this.playedThisTurn = false;
            this.hostageAsString = null;
        }
        
        public string getCharacter() {
            return this.banditNameAsString;
        }
        
        public void setGetsAnotherAction(bool anotherAction) {
            this.getsAnotherAction = anotherAction;
        }
        
        public bool getGetsAnotherAction() {
            return this.getsAnotherAction;
        }
        
        public bool getPlayedThisTurn() {
            return this.playedThisTurn;
        }
        
        public void setPlayedThisTurn(bool played) {
            this.playedThisTurn = played;
        }
        
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
        
        public bool addLootAt(int index, Loot a) {
            bool contains = this.loot.Contains(a);
            if (contains) {
                return false;
            }
            
            this.loot.Insert(index, a);
            return true;
        }
        
        public void removeLootAt(int index) {
            if ((this.loot.Count > index)) {
                this.loot.Remove(index);
            }
            
        }
        
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
        
        public bool containsLoot(Loot a) {
            bool contains = this.loot.Contains(a);
            return contains;
        }
        
        public int sizeOfLoot() {
            int size = this.loot.Count;
            return size;
        }
        
        public ArrayList getLoot() {
            return this.loot;
        }
        
        public void addDeckAt(int index, Card a) {
            bool contains = this.deck.Contains(a);
            if (contains) {
                return;
            }
            
            this.deck.Insert(index, a);
        }
        
        public void removeDeckAt(int index) {
            if ((this.deck.Count > index)) {
                this.deck.Remove(index);
            }
            
        }
        
        public Card getDeckAt(int index) {
            if ((this.deck.Count > index)) {
                return (Card)this.deck[index];
            }
            
            return null;
        }
        
        public void addDeck(Card a) {
            bool contains = this.deck.Contains(a);
            if (contains) {
                return;
            }
            
            this.deck.Add(a);
        }
        
        public void removeDeck(Card a) {
            if (this.deck.Contains(a)) {
                this.deck.Remove(a);
            }
            
        }
        
        public bool containsDeck(Card a) {
            bool contains = this.deck.Contains(a);
            return contains;
        }
        
        public int sizeOfDeck() {
            int size = this.deck.Count;
            return size;
        }
        
        public ArrayList getDeck() {
            return this.deck;
        }
        
        public void addHandAt(int index, Card a) {
            bool contains = this.hand.Contains(a);
            if (contains) {
                return;
            }
            
            this.hand.Insert(index, a);
        }
        
        public void removeHandAt(int index) {
            if ((this.hand.Count > index)) {
                this.hand.Remove(index);
            }
            
        }
        
        public Card getHandAt(int index) {
            if ((this.hand.Count > index)) {
                Card a = (Card)this.hand[index];
                return a;
            }
            
            return null;
        }
        
        public void addHand(Card a) {
            bool contains = this.hand.Contains(a);
            if (contains) {
                return;
            }
            
            this.hand.Add(a);
        }
        
        public void removeHand(Card a) {
            if (this.hand.Contains(a)) {
                this.hand.Remove(a);
            }
            
        }
        
        public bool containsHand(Card a) {
            bool contains = this.hand.Contains(a);
            return contains;
        }
        
        public int sizeOfHand() {
            int size = this.hand.Count;
            return size;
        }
        
        public ArrayList getHand() {
            return this.hand;
        }
        
        public void addDiscardPileAt(int index, Card a) {
            bool contains = this.discardPile.Contains(a);
            if (contains) {
                return;
            }
            
            this.discardPile.Insert(index, a);
        }
        
        public void removeDiscardPileAt(int index) {
            if ((this.discardPile.Count > index)) {
                this.discardPile.Remove(index);
            }
            
        }
        
        public Card getDiscardPileAt(int index) {
            if ((this.discardPile.Count > index)) {
                Card associated = (Card)this.discardPile[index];
                return associated;
            }
            else {
                return null;
            }
            
        }
        
        public void addDiscardPile(Card a) {
            bool contains = this.discardPile.Contains(a);
            if (!contains) {
                this.discardPile.Add(a);
            }
            
        }
        
        public void removeDiscardPile(Card a) {
            if (this.discardPile.Contains(a)) {
                this.discardPile.Remove(a);
            }
            
        }
        
        public bool containsDiscardPile(Card a) {
            bool contains = this.discardPile.Contains(a);
            return contains;
        }
        
        public int sizeOfDiscardPile() {
            int size = this.discardPile.Count;
            return size;
        }
        
        public ArrayList getDiscardPile() {
            return this.discardPile;
        }
        
        // public Hostage getHostage() {
        //     return this.hostage;
        // }
        
        // public void setHostage(Hostage hostage) {
        //     this.hostage = hostage;
        // }
        
        public string getHostageAsString() {
            return this.hostageAsString;
        }
        
        public void setHostageAsString(string hostage) {
            this.hostageAsString = hostage;
        }

        public ActionCard getToResolve() {
        return this.toResolve;
        }
    
        public void setToResolve(ActionCard ac) {
            this.toResolve = ac;
        }

        public int getConsecutiveTurnCounter() {
		    return this.consecutiveTurnCounter;
	    }
	    public void setConsecutiveTurnCounter(int i) {
		    this.consecutiveTurnCounter = i;
	    }
        
        public void createStartingCards() {
            string[] actions= {"MOVE", "MOVE", "CHANGE_FLOOR","CHANGE_FLOOR", "MOVE_MARSHAL", "PUNCH", "ROB", "ROB", "SHOOT","SHOOT"};
            for (int i = 0; i<actions.Length; i++) {
                Card c = new ActionCard (actions[i], this.banditNameAsString);
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
            BulletCard bc1 = new BulletCard(this.banditNameAsString);
            BulletCard bc2 = new BulletCard(this.banditNameAsString);
            BulletCard bc3 = new BulletCard(this.banditNameAsString);
            BulletCard bc4 = new BulletCard(this.banditNameAsString);
            BulletCard bc5 = new BulletCard(this.banditNameAsString);
            BulletCard bc6 = new BulletCard(this.banditNameAsString);
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

        public void endOfSchemin(){
            foreach (Card c in this.hand){
                this.deck.Add(c);
            }
            foreach (Card c in this.discardPile){
                this.deck.Add(c);
            }
            this.hand.Clear();
            this.discardPile.Clear();
            //HAND FOR NEXT ROUND IS CREATED AT END OF SCHEMIN PHASE - MUST NOT BE VISIBLE DURING STEALING PHASE
            this.createHand();
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

        public void drawCards() {
            if (this.deck.Count < 3) {
                return;
            }
            for (int i = 0; i<3; i++){
                Card c = (Card) this.deck[i];
                this.hand.Add(c);
            }
            for (int i = 0; i<3; i++){
                Card c = (Card)this.deck[i];
                this.deck.Remove(c);
            }
        }




    }

}