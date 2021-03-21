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

namespace model {
    public abstract class Card : SerializableSFSType {
      
        public string belongsToAsString;

        public Card() {}
        
        //queries each bandit's 10 cards to find object match (a bit excessive but it's the only way to get rid of reference from here)
       public Bandit getBelongsTo() {
           GameManager gm = GameManager.getInstance();
           foreach (Bandit b in gm.bandits) {
               if (b.banditNameAsString.Equals(this.belongsTo)) {
                   return b;
               }
           }
           return null;
       }

       public void setBelongsTo(string belongsTo) {
           if(belongsTo.Equals("GHOST")||belongsTo.Equals("DOC")||belongsTo.Equals("TUCO")||belongsTo.Equals("CHEYENNE")||belongsTo.Equals("BELLE")||belongsTo.Equals("DJANGO")){
               this.belongsTo = belongsTo;
           }
           else{
               Debug.Log("CARD SET TO INVALID CHARACTER");
           }
               
       }
    }
}

/* alternate method of querying cards - dont delete for now

ArrayList cards = b.deck.Clone();
               cards.AddRange(b.hand.Clone());
               cards.AddRange(b.discardPile.Clone());
               foreach(Card c in cards) {
                    if (c.Equals(this)){
                        return b;
                    }
               }*/