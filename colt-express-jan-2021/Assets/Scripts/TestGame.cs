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

// No longer need to edit GM.CS
//// BEFORE USING THIS ON NETWORKING BRANCH, SEE REQUIRED CHANGES IN COMMENTS ON TOP OF GAMEMANAGER.CS
// MAKE SURE IP IS SET TO "13.72.79.112" IN SFS.CS

public class TestGame : MonoBehaviour
{
    public static bool testing = false;
    static GameManager gm;
    public static string prompt = "";
    static int num = 0;

    public Text summary;

    // Start is called before the first frame update
    void Start()
    {
        testing = true;
        if (SFS.getSFS() == null)
        {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
            Debug.Log("SFS was null. Setting it now");
        }
        if (!SFS.IsConnected())
        {
            SFS.Connect("testgame");
            Debug.Log("was not connected. Connecting now");
        }
        SFS.setTestGame();
        Invoke("StartGame", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected())
        {
            SFS.ProcessEvents();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RunGame(num);
            GameSummary();
            Debug.Log("just called case " + num);
            num++;
            prompt = "";
        }
    }

    void StartGame()
    {
        ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.testgame", obj);
        SFS.Send(req);
        Debug.Log("sent request to start testgame");
    }

    public void ReceiveInitializedGame(BaseEvent evt)
    {
        Debug.Log("Received initialized game");
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
        gm = (GameManager)responseParams.GetClass("gm");

        // MUST REASSIGN REFERENCES
        GameManager.replaceInstance(gm);
        gm.currentBandit = (Bandit)gm.bandits[0];
        gm.currentRound = (Round)gm.rounds[gm.roundIndex];
        gm.playedPileInstance = PlayedPile.getInstance();
        foreach (Round r in gm.rounds)
        {
            r.currentTurn = (Turn)r.turns[r.turnCounter];
        }
        foreach (TrainUnit tr in gm.trainRoof)
        {
            foreach (Bandit b in gm.bandits)
            {
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if (tr.carTypeAsString == tu.carTypeAsString & tr.carFloorAsString == tu.carFloorAsString)
                {
                    gm.banditPositions[b.characterAsString] = tr;
                }
            }
        }
        foreach (TrainUnit tc in gm.trainCabin)
        {
            foreach (Bandit b in gm.bandits)
            {
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if (tc.carTypeAsString == tu.carTypeAsString & tc.carFloorAsString == tu.carFloorAsString)
                {
                    gm.banditPositions[b.characterAsString] = tc;
                }
            }
        }
        // add marshal's position
        Marshal marshal = Marshal.getInstance();
        marshal.setMarshalPosition((TrainUnit)gm.trainCabin[0]);

        GameSummary();
    }

    void GameSummary()
    {
        summary.text = "PROMPT: " + prompt + "\n";
        summary.text += "PHASE: " + gm.strGameStatus + "\n";
        summary.text += "CURRENT BANDIT: " + gm.currentBandit.characterAsString + "\n";
        summary.text += "CURRENT ROUND # and type: #" + gm.roundIndex + ", " + gm.currentRound.roundTypeAsString + "\n";
        summary.text += "CURRENT TURN # and type: #" + gm.currentRound.turnCounter + ", " + gm.currentRound.currentTurn.turnTypeAsString + "\n\n";

        foreach (Bandit b in gm.bandits)
        {
            summary.text += "BANDIT: " + b.characterAsString + "\n";
            summary.text += "CARDS: ";
            foreach (Card c in b.getHand())
            {
                try
                {
                    ActionCard ac = (ActionCard)c;
                    summary.text += ac.actionTypeAsString + ", ";
                }
                catch (Exception e)
                {
                    summary.text += "bullet card, ";
                }
            }
            summary.text += "\nDECK: ";
            foreach (Card c in b.getDeck())
            {
                try
                {
                    ActionCard ac = (ActionCard)c;
                    summary.text += ac.actionTypeAsString + ", ";
                }
                catch (Exception e)
                {
                    summary.text += "bullet card, ";
                }
            }
            summary.text += "\nLOOT: ";
            foreach (Loot l in b.getLoot())
            { 
                try
                {
                    Money m = (Money)l;
                    summary.text += m.moneyTypeAsString + ", ";
                }
                catch (Exception e)
                {
                    Whiskey w = (Whiskey)l;
                    summary.text += w.whiskeyTypeAsString + ", ";
                }
            }
            summary.text += "\nPOSITION: ";
            foreach (DictionaryEntry e in gm.banditPositions)
            {
                string bhere = (string)e.Key;
                if (b.characterAsString.Equals(bhere))
                {
                    TrainUnit t = (TrainUnit)e.Value;
                    summary.text += t.carTypeAsString + ", " + t.carFloorAsString + "\n";
                    break;
                }
            }
            summary.text += "\n";
        }
        summary.text += "\nPLAYED PILE: ";
        foreach (ActionCard c in PlayedPile.getInstance().getPlayedCards())
        {
            summary.text += c.actionTypeAsString + "(" + c.belongsToAsString + ") ";
            if (c.getFaceDown())
            {
                summary.text += " (FD)";
            }
            summary.text += ", ";
        }
        summary.text += "\n\nCARS: ";
        foreach (TrainUnit tr in gm.trainRoof)
        {
            summary.text += tr.carTypeAsString + " (" + tr.carFloorAsString + "): {";
            foreach (Bandit b in gm.bandits)
            {
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if (tr.containsBandit(b))
                {
                    summary.text += b.characterAsString + ", ";
                };
            }
            foreach (Loot l in tr.lootHere)
            {
                if (l is Money)
                {
                    summary.text += ((Money)l).moneyTypeAsString + ", ";
                }
                else if (l is Whiskey)
                {
                    summary.text += ((Whiskey)l).whiskeyTypeAsString + ", ";
                }
            }
            summary.text += "}, ";
            //TODO: iterate through its loot and indicate if stagecoach is adjacent
        }
        summary.text += "\n";
        foreach (TrainUnit tc in gm.trainCabin)
        {
            summary.text += tc.carTypeAsString + " (" + tc.carFloorAsString + "): {";
            foreach (Bandit b in gm.bandits)
            {
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if (tc.containsBandit(b))
                {
                    summary.text += b.characterAsString + ", ";
                };
            }
            // add marshal's position
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            if (tc == marshalPosition) {
                summary.text += "MARSHAL" + ", ";
            }
            foreach (Loot l in tc.lootHere)
            {
                if (l is Money)
                {
                    summary.text += ((Money)l).moneyTypeAsString + ", ";
                }
                else if (l is Whiskey)
                {
                    summary.text += ((Whiskey)l).whiskeyTypeAsString + ", ";
                }
            }
            summary.text += "}, ";
            //TODO: iterate through its loot and indicate if stagecoach is adjacent
        }
    }

    // MAKE MOCK PROMPT METHODS IN GAMEMANAGER.CS THAT ASSIGN THE PROMPT STRING IN TESTGAME.CS. SEE promptDrawCardsOrPlayCard() IN GM FOR REFERENCE
    void RunGame(int num)
    {
        switch (num)
        {
            case 0:
                // initial playturn()
                gm.playTurn();
                break;
            // R T1
            case 1:
                //BELLE PLAYS CHANGEFLOOR
                gm.playCard((ActionCard)gm.currentBandit.getHand()[2]);
                // ONLY CALL PLAYTURN() IMMEDIATELY AFTER CALLING A METHOD THAT CALLS ENDOFTURN() (such as playCard())
                gm.playTurn(); // this method would be called automatically at the BEGINNING of a turn on all clients
                break;
            case 2:
                //DOC PLAYS MOVE MARSHAL
                gm.playCard((ActionCard)gm.currentBandit.getHand()[5]); // DOC MOVES MARSHAL - case 14
                gm.playTurn();
                break;
            case 3:
                //GHOST PLAYS MOVE
                gm.playCard((ActionCard)gm.currentBandit.getHand()[3]); // Ghost moves to car3 roof - case 15
                gm.playTurn();
                break;
            // R1 T2
            case 4:
                //BELLE DRAWS 3
                gm.drawCards(3);
                gm.playTurn();
                break;
            case 5:
                //DOC DRAWS 3
                gm.drawCards(3);
                gm.playTurn();
                break;
            case 6:
                //GHOST DRAWS 3
                gm.drawCards(3);
                gm.playTurn();
                break;
            // R1 T3
            case 7:
                //BELLE PLAYS SHOOT
                gm.playCard((ActionCard)gm.currentBandit.getHand()[0]); // Belle shoot Ghost - case 16
                gm.playTurn();
                break;
            case 8:
                //DOC PLAYS PUNCH
                gm.playCard((ActionCard)gm.currentBandit.getHand()[4]); // Doc punches Belle - case 17
                gm.playTurn();
                break;
            case 9:
                //GHOST PLAYS SHOOT
                gm.playCard((ActionCard)gm.currentBandit.getHand()[0]);
                gm.playTurn();
                break;
            // R1 T4
                //BELLE PLAYS CHANGEFLOOR
            case 10:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[5]);
                gm.playTurn();
                break;
            case 11:
                //GHOST PLAYS CHANGEFLOOR
                gm.playCard((ActionCard)gm.currentBandit.getHand()[1]);
                gm.playTurn();
                break;
            case 12:
                //DOC PLAYS ROB
                gm.playCard((ActionCard)gm.currentBandit.getHand()[1]);
                gm.playTurn();
                break;
            // stealing phase R1 T1 STANDARD
            case 13:
                //BELLE CHANGE FLOOR to trainRoof[1] (roof1)
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                break;
            case 14:
                //DOC MOVE MARSHAL to trainCabin[1] (car1) doc and ghost shot by marshal, both move to roof1
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                gm.moveMarshal((TrainUnit)gm.trainCabin[1]); 
                break;
            case 15:
                //GHOST MOVE to trainRoof[3] (roof3)
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                gm.move((TrainUnit)gm.trainRoof[3]);
                break;
            // R1 T2 EVERYONE DRAWS CARDS - no stealin to resolve
            // R1 T3 TUNNEL
            case 16:
                //BELLE SHOOT Ghost
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                gm.shoot((Bandit)gm.bandits[2]);
                break;
            case 17:
                //DOC PUNCH Belle to locoRoof, Belle leaves her purse on roof1
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                Bandit Belle = (Bandit)gm.bandits[0];
                gm.punch(Belle, (Loot)Belle.getLoot()[0], (TrainUnit)gm.trainRoof[0]);
                break;
            case 18:
                //GHOST SHOOT Doc
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                Bandit Doc = (Bandit)gm.bandits[1];
                gm.shoot(Doc);
                break;
            // R1 T4 SWITCHING
            case 19:
                //BELLE CHANGEFLOOR
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                break;
            case 20:
                //GHOST CHANGEFLOOR
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                break;
            case 21:
                //DOC ROB
                gm.resolveAction((ActionCard)gm.currentBandit.toResolve);
                gm.rob((Loot)gm.currentBandit.getPosition().getLootHere()[0]);
                break;

        }
    }




    void OnApplicationQuit()
    {
        // Always disconnect before quitting
        SFS.Disconnect();
    }

}
