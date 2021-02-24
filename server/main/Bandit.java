package main;

import java.util.ArrayList;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public class Bandit implements SerializableSFSType {
    
    protected boolean getsAnotherAction;
    protected boolean playedThisTurn;
    protected Character banditName;
    protected TrainUnit position;
    protected Hostage hostage;
    protected ArrayList<Loot> loot = new ArrayList<Loot>();
    protected ArrayList<ActionCard> hand = new ArrayList<ActionCard>();;
    protected ArrayList<ActionCard> discardPile = new ArrayList<ActionCard>();;
    
    public Bandit() { }
    
    public void setGetsAnotherAction(boolean anotherAction) {
        /* TODO: No message view defined */
        this.getsAnotherAction = anotherAction;
    }

    public boolean getGetsAnotherAction() {
        /* TODO: No message view defined */
        return this.getsAnotherAction;
    }

    public boolean getPlayedThisTurn() {
        /* TODO: No message view defined */
        return this.playedThisTurn;
    }

    public void setPlayedThisTurn(boolean played) {
        /* TODO: No message view defined */
        this.playedThisTurn = played;
    }

    public TrainUnit getPosition() {
        return this.position;
    }

    public void setPosition(TrainUnit newObject) {
        this.position = newObject;
    }

    public void addLoot(Loot a) {
        boolean contains = this.loot.contains(a);
        if (contains) {
            return ;
        }
        this.loot.add(a);
    }

    public void removeLoot(Loot a) {
        if (this.loot.contains(a)) {
            this.loot.remove(a);
        }
    }

    public Loot getLootAt(int index) {
        Loot associated = loot.get(index);
        return associated;
    }


    public boolean containsLoot(Loot a) {
        boolean contains = loot.contains(a);
        return contains;
    }

    public int sizeOfLoot() {
        int size = loot.size();
        return size;
    }

    public ArrayList<Loot> getLoot() {
        return this.loot;
    }

    public void addToHand(ActionCard a) {
        boolean contains = this.hand.contains(a);
        if (contains) {
            return;
        }
        this.hand.add(a);
    }

    public void removeFromHand(ActionCard a) {
        if (this.hand.contains(a)){
            this.hand.remove(a);
        }
    }

    public boolean handContains(ActionCard a) {
        if (this.hand.contains(a)) {
            return true;
        }
        return false;
    }

    public int sizeOfHand() {
        int size = hand.size();
        return size;
    }

    public ArrayList<ActionCard> getHand() {
        return this.hand;
    }

    public void addToDiscardPile(ActionCard a) {
        boolean contains = this.discardPile.contains(a);
        if (contains) {
            return;
        }
        this.discardPile.add(a);
    }

    public void removeFromDiscardPile(ActionCard a) {
        if (this.discardPile.contains(a)) {
            this.discardPile.remove(a);
        }
    }

    public boolean discardPileContains(ActionCard a) {
        if (this.discardPile.contains(a)) {
            return true;
        }
        return false;;
    }

    public int sizeOfDiscardPile() {
        int size = discardPile.size();
        return size;
    }

    public ArrayList<ActionCard> getDiscardPile() {
        return this.discardPile;
    }

    public Hostage getHostage() {
        return this.hostage;
    }

    public void setHostage(Hostage newObject) {
        this.hostage = newObject;
    }
}
