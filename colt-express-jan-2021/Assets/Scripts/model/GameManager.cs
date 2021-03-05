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

    }
}