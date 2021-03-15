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

	public GameObject canvas;
  
	
    public Bandit b;

	public static GameManager gm;

	// LIST OF ALL GAME OBJECTS HERE
    public GameObject cheyenne;
	public GameObject belle; 
	public GameObject tuco; 
	public GameObject doc; 
	public GameObject ghost; 
	public GameObject django; 
	public GameObject marshal;
	
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
	public GameObject cardF;
	public GameObject cardG;
	public GameObject bulletCard;

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


    public Text announcement;

	/* For all the action cards */
	public Text drawnCard1; 
	public Text drawnCard2;
	public Text drawnCard3; 
	public Text drawnCard4;
	public Text drawnCard5;
	public Text drawnCard6;
	public Text drawnCard7;


	public GameObject playerE; 


	private String[] logMessages = {
		"Angry Marshal Round! 2 standard turns, 1 tunnel turn, and 1 switching turn",
		"Ghost chose to draw cards",
		"Cheyenne chose to draw cards",
		"Django chose to draw cards",
		"Ghost played a MOVE card",
		"Cheyenne played a CHANGEFLOOR card",
		"Django chose to draw cards",
		"Ghost played an action card which is hidden because its a tunnel turn",
		"Cheyenne played an action card which is hidden because its a tunnel turn",
		"Django played an action card which is hidden because its a tunnel turn",
		"Django played a SHOOT card",
		"Cheyenne chose to draw cards",
		"Ghost chose to draw cards",
		"Ghost moved to the adjacent car",
		"Cheyenne moved to the top of the car",
		"Ghost chooses one gem to add to his loot",
		"Cheyenne moved the Marshal",
		"Django choose to punch Ghost, who drops his loot",
		"Django shoots Ghost",
		// Speeding up round starts
		"Cheyenne played a MOVE card",
		"Cheyenne chose to draw cards",
		"Django played a CHANGEFLOOR card",
		"Django chose to draw cards",
		"Ghost chose to draw cards",
		"Ghost played a CHANGEFLOOR card",
		"Cheyenne moves to the adjacent train car",
		"Django is moved to the top of the car",
		"Ghost is moved to the top of the car",
		"Game has ended. Django is the winner"
	};

    //private static SmartFox sfs = SFS.sfs;
    // private static string defaultHost = SFS.defaultHost;// = "127.0.0.1"; //"13.90.26.131"; // 
    //private static int defaultTcpPort = SFS.defaultTcpPort;// = 9933;			// Default TCP port
    //private static string zone = SFS.zone;// = "MergedExt"; //"ColtExpress"; //"NewZone"; //"BasicExamples";// "MyExt";
    private List<float> cartZeroTop = new List<float>() {840.5F,878.4F,-364.9F};
    private List<float> cartZeroBtm = new List<float>() {786.1F, 813.5F, -364.9F};

    private List<float> cartOneTop = new List<float>() {1025.7F, 889.4F, -364.9F};
    private List<float> cartOneBtm = new List<float>() {1027.9F, 806.4F, -364.9F};

    private List<float> cartTwoTop = new List<float>() {1265.4F, 894.7F, -364.9F};
    private List<float> cartTwoBtm = new List<float>() {1279.8F, 817.7F, -364.9F};

    private List<float> cartLocoTop = new List<float>() {1410.5F, 893.4F, -364.9F};
    private List<float> cartLocoBtm = new List<float>() {1390.0F, 824.9F, -364.9F};

    private List<float> iconPosition = new List<float>() {1285.9F, 1121.9F, -364.9F};

	public void drawCards(){
		// draws 3 cards randomly and put in the hand
		drawnCard1.text = "MOVE";
		drawnCard2.text = "ROB";
		drawnCard3.text = "MARSHAL"; 
		drawnCard4.text = "CHANGE FLOOR";
		drawnCard5.text = "SHOOT"; 
		drawnCard6.text = "PUNCH"; 
		return;
	}

    void Start(){
		announcement.text = "";
		SFS.setGameBoard();
		drawnCard1.text = "MOVE";
		drawnCard2.text = "ROB";
		// drawnCard3.text = "MARSHAL"; 
		drawnCard4.text = "CHANGE FLOOR";
		drawnCard5.text = "SHOOT"; 
		// drawnCard6.text = "PUNCH"; 

		// announcement.text = "The current round is an Angry Marshal Round and the current turn is a Tunnel Turn!";
		// drawnCard1.text="MOVE";
		// SFS.setGameBoard(this);
		Debug.Log(SFS.step);
		announcement.text = logMessages[SFS.step];
		//debugTextString = "";
                //debugText.text = "";
		gem2.SetActive(false);
	

		//SendNewGameState();
		// ** THE DICTIONARIES ARE INITIALIZED(CLEARED) IN Start() ** 
		// Bandits
		// objects.Add(cheyenne, "null");
		// objects.Add(belle, "null");
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
		// objects.Add(cardA, "null");
		// objects.Add(cardB, "null");
		// objects.Add(cardC, "null");
		// objects.Add(cardD, "null");
		// objects.Add(cardE, "null");

		//EnterGameBoardScene();


    }

	// replace the last three cards with new cards 
	public void drawThreeCards(){
		// Debug.Log("DRAWS 3 CARDS");
		// remove card 5,6,7 (CardE, CardF, CardG) 
		// so that card 8,9,10 are visible
		Destroy(cardE);
		Destroy(cardF);
		Destroy(cardG);
	}

	// public void drawCards(){
	// 	// draws 3 cards randomly and put in the hand
	// 	drawnCard1.text = "MOVE";
	// 	drawnCard2.text = "ROB";
	// 	drawnCard3.text = "MARSHAL"; 
	// 	drawnCard4.text = "CHANGE FLOOR";
	// 	drawnCard5.text = "SHOOT"; 
	// 	drawnCard6.text = "PUNCH"; 
	// 	return;
	// }
	// draw3cards: 3 cards appear on click


	void MouseDown() {
		drawThreeCards();
		SFS.step += 1;
		Debug.Log("Clicked on step " + SFS.step);

		int step = SFS.step;
		ISFSObject obj = SFSObject.NewInstance();
		obj.PutInt("step", step);
		ExtensionRequest req = new ExtensionRequest("gm.nextAction",obj);
		SFS.Send(req);
		
		executeHardCoded(step);
	}

	public void executeHardCoded(int step) {
		Debug.Log("executing hardcoded");
		announcement.text += "\n";
		announcement.text += logMessages[step];
		switch(step) {
			case 1:
				//round/turn info 
				drawThreeCards();
				break;
			case 2:
				//ghost draws 
				// logmessage[2]
				break;
			case 3: 
				// chey draws
				break; 
			case 4: 
				// dja draws 
				break;
			case 5: 
				// ghost plays MOVE card
				break;
			case 6:
				// chey plays CHANGE FLOOR card
				break;
			case 7:
				// dja draws 
				break;
			case 8:
				// Ghost plays a hidden action card
				break;
			case 9:
				// chey plays a hidden action card
				break;
			case 10:
				// dja plays a hidden action card
				break;
			case 11:
				// Dja plays SHOOT // SHOT Ghost
				// TODO: CLICK ON GHOST'S PROFILE PIC AT THE TOP 
				shoot();  
				break;
			case 12:
				// chey draws 
				break;
			case 13:
				// ghost draws 
			        // ghost.transform.position = new Vector3 (cartOneBtm[0], cartOneBtm[1], cartOneBtm[2]);
                    		// ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
				break;
			case 14:
				// ghost move 
				ghost.transform.position = new Vector3 (cartOneBtm[0], cartOneBtm[1], cartOneBtm[2]);
                    		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
			        // cheyenne.transform.position = new Vector3 (cartZeroTop[0], cartZeroTop[1], cartZeroTop[2]);
                    		// cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;
				break;
			case 15:
				// chey moves 
				cheyenne.transform.position = new Vector3 (cartZeroTop[0], cartZeroTop[1], cartZeroTop[2]);
                    		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;
			        // Destroy(gem3);
				break;
			case 16:
				// ghost chooses gem3
				// TODO: GHOST CLICK ON GEM 3 
				Destroy(gem3);
				break;
			case 17:
				// chey moves the marshal 
				marshal.transform.position = new Vector3 (cartTwoBtm[0], cartTwoBtm[1], cartTwoBtm[2]);
                		marshal.transform.position += marshal.transform.forward * Time.deltaTime * 5f;
				break;
			case 18:
				// Dja PUNCH ghost, ghost drop gem2 
				punch(); 
				break;
			case 19:
			   	// dj shots ghost 
			   	shoot();
				break;
			case 20:
				// chey plays MOVE  
				break;
			case 21:	
				// chey draws
				break;
			case 22:
				// Dja plays CHANGE FLOOR  
				break;
			case 23:
				// Dja draws 
				break;
			case 24:
				// ghost draws card
				break;
			case 25:
				// ghost played a CHANGE FLOOR card	
				break;
			case 26:
				// chey moves
			        cheyenne.transform.position = new Vector3 (cartOneTop[0], cartOneTop[1], cartOneTop[2]);
                    		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;	
				break;
			case 27:
				// dj moves
			        django.transform.position = new Vector3 (cartOneTop[0], cartOneTop[1], cartOneTop[2]);
                    		django.transform.position += django.transform.forward * Time.deltaTime * 10f;	
				break;
			case 28:
				// ghost moves
			        ghost.transform.position = new Vector3 (cartOneBtm[0], cartOneBtm[1], cartOneBtm[2]);
                    		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
				break;
			case 29: 
				// announce winner 
				break; 
		}
    }

	public void rob(){
		gem3.SetActive(false);
		Destroy(gem3);
	}

	public void punch(){
		// Django punches Ghost, Ghost is punched back to last train car and with 
		// his initial purse is left in the second last train car. 
		// move ghost to the last train car
		// check if the obj being clicked on is the loot/bandit that we want to move 
		Debug.Log("GHOST IS PUNCHED");
        float posX = cartZeroBtm[0]; 
        float posY = cartZeroBtm[1]; 
        float posZ = cartZeroBtm[2]; 
        ghost.transform.position = new Vector3 (posX, posY, posZ);
        ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f; // can be any float number
		gem2.SetActive(true);
		// shoot();  
	}

	public void shoot(){
		// Django punches Ghost, Ghost is punched back to last train car and with 
		// his initial purse is left in the second last train car. 
		// move ghost to the last train car
		// check if the obj being clicked on is the loot/bandit that we want to move 
		// Debug.Log("GHOST IS SHOT");
        // float posX = cartZeroBtm[0]; 
        // float posY = cartZeroBtm[1]; 
        // float posZ = cartZeroBtm[2]; 
        // ghost.transform.position = new Vector3 (posX, posY, posZ);
        // ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f; // can be any float number
		// gem2.SetActive(true);
		Debug.Log("GHOST IS SHOT");
		bulletCard.transform.position = new Vector3 (iconPosition[0], iconPosition[1], iconPosition[2]);
        bulletCard.transform.position += bulletCard.transform.forward * Time.deltaTime * 2f;
	}

	// public void OnMouseDown(){
	// 	MouseDown();
	// 	Debug.Log("Clicked");
	// }


    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		if (Input.GetMouseButtonDown(0)){
			MouseDown();
			Debug.Log("Clicked");
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
		ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}
