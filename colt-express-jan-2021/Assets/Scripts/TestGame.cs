using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;
using model;

public class TestGame : MonoBehaviour
{
    static GameManager gm;
    public static string prompt = "";
    static int num = 0;

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
            //TODO: DEBUG LOG A RESPONSE AFFIRMING CONNECTION
        }
        Invoke("StartGame", 3);
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
            num++;
            prompt = "";
		}
    }

    void StartGame() {
        ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.testGame",obj);
        SFS.Send(req);
        Debug.Log("sent request to start testgame");
    }

    public static void ReceiveInitializedGame(BaseEvent evt) {
		Debug.Log("Received initialized game");
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");
		GameManager.replaceInstance(gm);
        gm.currentBandit = (Bandit)gm.bandits[0];
        // TODO: MUST REASSIGN BANDIT POSITION ARRAY
        GameSummary();
    }

    static void GameSummary() {
        Debug.Log("prompt: " + prompt);
        foreach (Bandit b in gm.bandits){
            Debug.Log("BANDIT: " + b.characterAsString);
            Debug.Log("CARDS:");
            foreach(Card c in b.getHand()){
                try {
                    ActionCard ac = (ActionCard)c;
                    //Debug.Log(ac.action type);
                } catch(Exception e) {
                    Debug.Log("bullet card");
                }
            }
            Debug.Log("LOOT:");
            foreach(Card c in b.getHand()){
                try {
                    ActionCard ac = (ActionCard)c;
                    //Debug.Log(ac.action type);
                } catch(Exception e) {
                    Debug.Log("bullet card");
                }
            }
            Debug.Log("POSITION:");
            Debug.Log("PLAYED PILE:");
        }
    }

    void RunGame(int num){
        switch (num) {
            case 0:
                gm.playTurn();
                break;
            case 1:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[0]);
                break;
            case 2:
                gm.playTurn();
                break;
            case 3:
                gm.playCard((ActionCard)gm.currentBandit.getHand()[0]);
                break;
        }
    }

    


    void OnApplicationQuit() {
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}
