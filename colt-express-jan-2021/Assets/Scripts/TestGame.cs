using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;
using model;


// BEFORE USING THIS ON NETWORKING BRANCH, SEE REQUIRED CHANGES IN COMMENTS ON TOP OF GAMEMANAGER.CS
// MAKE SURE IP IS SET TO "13.72.79.112" IN SFS.CS
// MAKE SURE BUILD SETTINGS HAS TESTGAME SCENE


public class TestGame : MonoBehaviour
{
    static GameManager gm;
    public static string prompt = "";
    static int num = 0;

    public Text summary;

    // Start is called before the first frame update
    void Start()
    {
        if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
            Debug.Log("SFS was null. Setting it now");
        }
        if (!SFS.IsConnected()) {
            SFS.Connect("testgame");
            Debug.Log("was not connected. Connecting now");
        }
        SFS.setTestGame();
        Invoke("StartGame", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

        if (Input.GetMouseButtonDown(0)){
			RunGame(num);
            GameSummary();
            Debug.Log("just called case " + num);
            num++;
            prompt = "";
		}
    }

    void StartGame() {
        ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.testgame",obj);
        SFS.Send(req);
        Debug.Log("sent request to start testgame");
    }

    public void ReceiveInitializedGame(BaseEvent evt) {
		Debug.Log("Received initialized game");
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");
        
        // MUST REASSIGN REFERENCES
		GameManager.replaceInstance(gm);
        gm.currentBandit = (Bandit)gm.bandits[0];
        gm.currentRound = (Round)gm.rounds[gm.roundIndex];
        foreach(Round r in gm.rounds) {
            r.currentTurn = (Turn)r.turns[r.turnCounter];
        }
        foreach(TrainUnit tr in gm.trainRoof){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tr.carTypeAsString == tu.carTypeAsString & tr.carFloorAsString == tu.carFloorAsString) {
                gm.banditPositions[b.characterAsString] = tr;
                }
            }
        }
        foreach(TrainUnit tc in gm.trainCabin){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tc.carTypeAsString == tu.carTypeAsString & tc.carFloorAsString == tu.carFloorAsString) {
                gm.banditPositions[b.characterAsString] = tc;
                }
            }
        }

        GameSummary();
    }

    void GameSummary() {
        summary.text = "PROMPT: " + prompt + "\n";
        summary.text += "CURRENT BANDIT: " + gm.currentBandit.characterAsString + "\n";
        summary.text += "CURRENT ROUND # and type: #" + gm.roundIndex + ", " + gm.currentRound.roundTypeAsString + "\n";
        summary.text += "CURRENT TURN # and type: #" + gm.currentRound.turnCounter + ", " + gm.currentRound.currentTurn.turnTypeAsString + "\n";

        foreach (Bandit b in gm.bandits){
            summary.text += "BANDIT: " + b.characterAsString + "\n";
            summary.text += "CARDS: ";
            foreach(Card c in b.getHand()){
                try {
                    ActionCard ac = (ActionCard)c;
                    summary.text += ac.actionTypeAsString + ", ";
                } catch(Exception e) {
                    summary.text += "bullet card, ";
                }
            }
            summary.text += "\nLOOT: ";
            foreach(Loot l in b.getLoot()){
                summary.text += "l, ";
            }
            summary.text += "\nPOSITION: ";
            foreach(DictionaryEntry e in gm.banditPositions) {
                string bhere = (string) e.Key;
                if(b.characterAsString.Equals(bhere)) {
                    TrainUnit t = (TrainUnit) e.Value;
                    summary.text += t.carTypeAsString + ", " + t.carFloorAsString + "\n";
                    break;
                }
            }
            summary.text += "\n";
        }
        summary.text += "\nPLAYED PILE: ";
        foreach(ActionCard c in PlayedPile.getInstance().getPlayedCards()){
                summary.text += c.actionTypeAsString;
                if(c.getFaceDown()){
                    summary.text += " (FD)";
                }
                summary.text += ", ";
        }
        summary.text += "\n\nCARS: ";
        foreach(TrainUnit tr in gm.trainRoof){
                summary.text += tr.carTypeAsString + " (" + tr.carFloorAsString + "): {";
                 foreach (Bandit b in gm.bandits){
                    TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                    if(tr.containsBandit(b)){
                        summary.text += b.characterAsString + ", ";
                    };
                 }
                 summary.text += "}, ";
                //TODO: iterate through its loot and indicate if stagecoach is adjacent
        }
        summary.text += "\n";
        foreach(TrainUnit tc in gm.trainCabin){
                summary.text += tc.carTypeAsString + " (" + tc.carFloorAsString + "): {";
                foreach (Bandit b in gm.bandits){
                    TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                    if(tc.containsBandit(b)){
                        summary.text += b.characterAsString + ", ";
                    };
                }
                summary.text += "}, ";
                //TODO: iterate through its loot and indicate if stagecoach is adjacent
        }
    }

    // MAKE MOCK PROMPT METHODS IN GAMEMANAGER.CS THAT ASSIGN THE PROMPT STRING IN TESTGAME.CS. SEE promptDrawCardsOrPlayCard() IN GM FOR REFERENCE
    void RunGame(int num){
        switch (num) {
            case 0:
                // initial playturn()
                gm.playTurn();
                break;
            case 1:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[0]);
                // ONLY CALL PLAYTURN() IMMEDIATELY AFTER CALLING A METHOD THAT CALLS ENDOFTURN() (such as playCard())
                gm.playTurn();
                break;
            case 2:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[1]);
                gm.playTurn();
                break;
            case 3:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[2]);
                gm.playTurn();
                break;
            // ADD MORE CASES!
        }
    }

    


    void OnApplicationQuit() {
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}
