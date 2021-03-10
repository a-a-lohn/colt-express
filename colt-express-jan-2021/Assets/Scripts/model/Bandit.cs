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

    public class Bandit : SerializableSFSType {
    
        public bool getsAnotherAction;       
        public bool playedThisTurn;       
        // public Character banditName;       
        public string banditNameAsString;
        
        // FOR NETWORKING
        public TrainUnit position;   
        public Hostage hostage;       
        public string hostageAsString;
        
        // FOR NETWORKING
        public ArrayList loot ;      
        public ArrayList bullets ;        
        public ArrayList deck ;
        public ArrayList hand ;
        public ArrayList discardPile ;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Bandit() {}

        public Bandit(/*Character*/ string c) {
            //this.banditName = c;
            this.banditNameAsString = c;//.ToString();
            this.getsAnotherAction = false;
            this.playedThisTurn = false;
            this.position = null;
            this.hostage = null;
        }
        
        /*public Character getCharacter() {
            return this.banditName;
        }*/
        
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
            return this.position;
        }
        
        public void setPosition(TrainUnit newObject) {
            this.position = newObject;
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
        
        public Hostage getHostage() {
            return this.hostage;
        }
        
        public void setHostage(Hostage hostage) {
            this.hostage = hostage;
        }
        
        public string getHostageAsString() {
            return this.hostageAsString;
        }
        
        public void setHostageAsString(string hostage) {
            this.hostageAsString = hostage;
        }
        
        public void createStartingCards() {
            ActionCard acMove1 = new ActionCard(ActionType.MOVE);
            ActionCard acMove2 = new ActionCard(ActionType.MOVE);
            ActionCard acChangeFloor1 = new ActionCard(ActionType.CHANGE_FLOOR);
            ActionCard acChangeFloor2 = new ActionCard(ActionType.CHANGE_FLOOR);
            ActionCard acMarshal = new ActionCard(ActionType.MOVE_MARSHAL);
            ActionCard acPunch = new ActionCard(ActionType.PUNCH);
            ActionCard acRob1 = new ActionCard(ActionType.ROB);
            ActionCard acRob2 = new ActionCard(ActionType.ROB);
            ActionCard acShoot1 = new ActionCard(ActionType.SHOOT);
            ActionCard acShoot2 = new ActionCard(ActionType.SHOOT);
            this.deck.Add(acMove1);
            this.deck.Add(acMove2);
            this.deck.Add(acChangeFloor1);
            this.deck.Add(acChangeFloor2);
            this.deck.Add(acMarshal);
            this.deck.Add(acPunch);
            this.deck.Add(acRob1);
            this.deck.Add(acRob2);
            this.deck.Add(acShoot1);
            this.deck.Add(acShoot2);
            foreach (Card c in this.deck) {
                c.setBelongsTo(this);
            }
            
        }
        
        public void createHand() {
            //this.deck = this.deck.Shuffle(); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //Collections.shuffle(this.deck, new Random(System.currentTimeMillis()));
            for (int i = 0; (i < 6); i++) {
                Card c = (Card)this.deck[0];
                this.hand.Add(c);
                // this.deck.remove(c);
            }
            
        }
        
        public void createBulletCards() {
            BulletCard bc1 = new BulletCard();
            BulletCard bc2 = new BulletCard();
            BulletCard bc3 = new BulletCard();
            BulletCard bc4 = new BulletCard();
            BulletCard bc5 = new BulletCard();
            BulletCard bc6 = new BulletCard();
            this.bullets.Add(bc1);
            this.bullets.Add(bc2);
            this.bullets.Add(bc3);
            this.bullets.Add(bc4);
            this.bullets.Add(bc5);
            this.bullets.Add(bc6);
            foreach (BulletCard bc in this.bullets) {
                bc.setBelongsTo(this);
            }
            
        }
        
        public void createStartingPurse() {
            Money startingPurse = new Money(MoneyType.PURSE, 250); 
            this.loot.Add(startingPurse);
        }

    }

}
