using model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;

using System.Collections;

public class GameBoard : MonoBehaviour
{

	/*
	Frontend team:
	-attach choosecharacter strings to characters (attach character strings from
		 DisplayRemainingCharacters() to characters in the scene 
		 so that when the characters are clicked, CharacterChoice(character) is called that passes the chosen character to the server. This
		 can be done by attaching scripts to each character game object, similar to how it will work for gameobjects on the game board)
	-Assign all gameobjects in dictionary upon update game state call
	-implement prompt method in Gamemanager (i.e. set action and clickable global variables)
	-write scripts attached to each game object that checks if it is clickable, if so, checks action and calls action on clicked item
	-assign locations on game board to each gameobject (should be in attached scripts in as a global variable that is reassigned every
	   time updategamestate() is called, checks updated gm instance for new item's position)
	-get login and other scripts --done
	*/

	//debug variables
	public static Text debugText;
	public static string debugTextString;
    public Button button;
	public Button extension;
	public Button chooseChar;
  
	
    public Bandit b;

	public static GameManager gm;

	// LIST OF ALL GAME OBJECTS HERE
    public GameObject cheyenne;
	public GameObject belle; 
	public GameObject tuco; 
	public GameObject doc; 
	public GameObject ghost; 
	public GameObject django; 
	
	public GameObject gem1; 
	public GameObject gem2; 
	public GameObject gem3; 
	public GameObject gem4;
	public GameObject gem5;

	public GameObject cardA; 
	public GameObject cardB; 
	public GameObject cardC; 
	public GameObject cardD;
	public GameObject cardE;

	// propmpt messages 
	public Text promptDrawCardsOrPlayCardMsg;
	public Text promptChooseLoot; 
	public Text promptPunchTarget; 

	// public Text promptDrawCardsOrPlayCardMsg;
	// public GameObject tuco;
	// public GameObject doc; 
	// public GameObject django; 
    
    public static Dictionary<GameObject, object> objects = new Dictionary<GameObject, object>();

	// public Dictionary<T, GameObject> objects = new Dictionary<T, GameObject>();
	// NOTE: INITIALIZE THE DICTIONARY FOR EVERY OBJECT HERE FIRST,
	// ** THE DICTIONARIES ARE INITIALIZED(CLEARED) IN Start() ** 
	// E.G. objects.Add(cheyenne, null), objects.Add(tuco, null), ...
	// This way, update game state will simply be able to overwrite the values in the dictionary
	// whenever it is called by the server


	public static ArrayList clickable = new ArrayList();
	public static string action = "";


    //private static SmartFox sfs = SFS.sfs;
   // private static string defaultHost = SFS.defaultHost;// = "127.0.0.1"; //"13.90.26.131"; // 
	//private static int defaultTcpPort = SFS.defaultTcpPort;// = 9933;			// Default TCP port
    //private static string zone = SFS.zone;// = "MergedExt"; //"ColtExpress"; //"NewZone"; //"BasicExamples";// "MyExt";

    void Start()
    {
		SFS.setGameBoard(this);

		//debugTextString = "";
        //debugText.text = "";

		//SendNewGameState();
		// ** THE DICTIONARIES ARE INITIALIZED(CLEARED) IN Start() ** 
		// Bandits
		objects.Add(cheyenne, "null");
		objects.Add(belle, "null");
		/*objects.Add(tuco, "null");
		objects.Add(doc, "null");
		objects.Add(ghost, "null");
		objects.Add(django, "null");
		// Loot
		objects.Add(gem1, "null");
		objects.Add(gem2, "null");
		objects.Add(gem3, "null");
		objects.Add(gem4, "null");
		objects.Add(gem5, "null");*/
		// Cards
		objects.Add(cardA, "null");
		objects.Add(cardB, "null");
		objects.Add(cardC, "null");
		objects.Add(cardD, "null");
		objects.Add(cardE, "null");

		EnterGameBoardScene();

    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		/*if (SFS.debugText != debugText.text) {
            debugText.text = SFS.debugText;
        }

		// for debugging
		if (SFS.moreText) {
            debugTextString += SFS.debugText;
            SFS.moreText = false;
        }
        if (debugTextString != debugText.text) {
            debugText.text = debugTextString;
        }*/
    }

	public void EnterGameBoardScene() {
		Debug.Log("entering scene");
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterGameBoardScene",obj);
        SFS.Send(req);
        trace("Sent enter scene message");
	}

	public static void SendNewGameState() {
		ISFSObject obj = SFSObject.NewInstance();
		Debug.Log("sending new game state");
		obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.newGameState",obj);
        SFS.Send(req);
        trace("sent game state");
	}

	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");
		
		// REASSIGN ALL GAME OBJECTS USING DICTIONARY
		ArrayList banditsArray = gm.bandits;
		//ArrayList banditsArray = new ArrayList();
		foreach (Bandit b in banditsArray) {
            if (b.banditNameAsString == "CHEYENNE") {
				objects[cheyenne] = b;
                trace("Cheyenne added!");
            }
			if (b.banditNameAsString == "BELLE") {
                objects[belle] = b;
                trace("Belle added!");
            }
			if (b.banditNameAsString == "TUCO") {
                objects[tuco] = b;
                trace("Tuco added!");
            }
			if (b.banditNameAsString == "DOC") {
                objects[doc] = b;
                trace("Doc added!");
            }
			if (b.banditNameAsString == "GHOST") {
                objects[ghost] = b;
                trace("Ghost added!");
            }
			if (b.banditNameAsString == "DJANGO") {
                objects[django] = b;
                trace("Django added!");
            }
		}
		Debug.Log("bandits array size: " + banditsArray.Count);
		ArrayList cards = new ArrayList();
		foreach (Bandit ba in banditsArray) {
			Debug.Log(ba.banditNameAsString +" "+ ChooseCharacter.character);
			if(ba.banditNameAsString == ChooseCharacter.character) {
				ArrayList hand = b.hand;
				Debug.Log("adding cards");
				objects[cardA] = hand[0];
				objects[cardB] = hand[1];
				objects[cardC] = hand[2];
				objects[cardD] = hand[3];
				objects[cardE] = hand[4];
			}
		}

			gm.playTurn();
    }

	/*private void ChooseCharacter() {
        ISFSObject obj = SFSObject.NewInstance();
		obj.PutUtfString("chosenCharacter", "TUCO");
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        SFS.Send(req);
        trace("chose Tuco");
    }*/

    public static void trace(string msg) {
	//	debugText.text += (debugText.text != "" ? "\n" : "") + msg;
	}


    void OnApplicationQuit() {
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}