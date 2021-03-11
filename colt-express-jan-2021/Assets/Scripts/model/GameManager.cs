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

// STATIC FIELDS ARE NOT SERIALIZED
namespace model {
    public class GameManager : SerializableSFSType {

        public string strGameStatus; //public GameStatus status;
        public Round currentRound;
        public Bandit currentBandit;
        public ArrayList rounds;
        public static GameManager instance;
        public static Marshal marshalInstance;
        public static PlayedPile playedPileInstance; 
        public ArrayList trainUnits;
        public ArrayList bandits;

        public GameManager() { }

        public void PlayTurn() {
            if(/*currentBandit == MYBANDIT*/true) {
                if(strGameStatus == "SCHEMIN") {
                    // calculateCardsAvailable()
                    //promptChooseCardOrDrawCards(Arraylist of cards); -- Makes objects clickable (plus drawcard) (of class types from gm), can be implemented in a separate class
                }
                else if (strGameStatus == "STEALIN") {
                    if (/*playedPileInstance.Pop() == "PUNCH"*/true) {
                        calculatePunchTargets();
                    }
                }
            }
        }

        public void PlayCard(ActionCard c) { // -- Called from front-end
            //...
            // CARRY OUT END OF MOVE / READY FOR NEXT MOVE (CHANGE BANDIT TO NEXT PLAYER, ETC.)
            GameBoard.SendNewGameState();
        }

        public void DrawCards() { // -- Called from front-end
            //...
        }


        private void calculatePunchTargets() {
            //...
            //promptPunch(/*arraylist of bandits*/); -- Makes objects clickable
            // this method sets clickable and action global variables to the clickable items and to what should be called next, respectively
        }

        private void punch(Bandit b) { // -- Called from front-end
            // CARRY OUT THE PUNCH
            // CARRY OUT END OF MOVE / READY FOR NEXT MOVE (CHANGE BANDIT TO NEXT PLAYER, ETC.)
            GameBoard.SendNewGameState();
        }

    }
}