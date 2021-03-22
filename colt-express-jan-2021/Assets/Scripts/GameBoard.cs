using model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
	
	-Assign all gameobjects in dictionary upon update game state call --loots
	
	-implement prompt method in Gamemanager (i.e. set action and clickable global variables) --done // makeallclickable() and update()
	
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

	public Text Round;
	//public GameObject exit;
	public Text exitText;
  
	
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
	public GameObject gem6;

	public GameObject ghoLoot;

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
    
    public static Dictionary<GameObject, object> objects = new Dictionary<GameObject, object>();
	// public static Dictionary<GameObject, object> bulletCards = new Dictionary<GameObject, object>();

	// public Dictionary<T, GameObject> objects = new Dictionary<T, GameObject>();
	// NOTE: INITIALIZE THE DICTIONARY FOR EVERY OBJECT HERE FIRST,
	// ** THE DICTIONARIES ARE INITIALIZED(CLEARED) IN Start() ** 
	// E.G. objects.Add(cheyenne, null), objects.Add(tuco, null), ...
	// This way, update game state will simply be able to overwrite the values in the dictionary
	// whenever it is called by the server

	// public static ArrayList clickable = new ArrayList();
	// public static ArrayList clickable = new ArrayList();
	public static string action = ""; // i.e. PUNCH, SHOOT etc. 

    public Text announcement;

	public Text cardAText; 
	public Text cardBText;
	public Text cardCText; 
	public Text cardDText;
	public Text cardEText;
	public Text cardFText;

	public Text cardNewABext;
	public Text cardNewCText;

	public GameObject CardNewA; 
	public GameObject CardNewB; 
	public GameObject CardNewC; 
	public GameObject CardNewD; 
	public GameObject CardNewE; 
	public GameObject CardNewF; 

	public GameObject playerE;

	public GameObject BelleBulletCard1; 
	public GameObject BelleBulletCard2;
	public GameObject BelleBulletCard3;
	public GameObject BelleBulletCard4;
	public GameObject BelleBulletCard5;
	public GameObject BelleBulletCard6;      


	public Text clickableGOsText;
	public Text currentRound; 
	public Text currentBandit; 


	
	// a list of clickable items
	private List<GameObject> clickableGOs; 

	private String[] logMessages = {
		"Angry Marshal Round! 1 Standard turns, 1 Tunnel turn, and 1 Switching turn\nIt is now Ghost's turn to play a card or draw 3 cards.\n", //0
		"Standard Turn: Ghost played a MOVE card\nIt is now Cheyenne's turn to play a card or draw 3 cards.\n",
		"Standard Turn: Cheyenne played a CHANGEFLOOR card\nIt is now Django's turn to play a card or draw 3 cards.\n",
		"Standard Turn: Django chose to draw cards\nNext turn!\nIt is now Ghost's turn to play a card or draw 3 cards.\n",
		"Tunnel Turn: Ghost played an action card which is hidden\nIt is now Cheyenne's turn to play a card or draw 3 cards.\n", //4
		"Tunnel Turn: Cheyenne played an action card which is hidden\nIt is now Django's turn to play a card or draw 3 cards.\n",

		"Tunnel Turn: Django played an action card which is hidden\nSwitching Turn Player Order: Ghost, Django, Cheyenne\nIt is now Ghost's turn to play a card or draw 3 cards.\n",
		"Switching Turn: Ghost chose to draw cards\nIt is now Django's turn to play a card or draw 3 cards.\n",
		"Switching Turn: Django played a SHOOT card\nIt is now Cheyenne's turn to play a card or draw 3 cards.\n",
		"Switching Turn: Cheyenne chose to draw cards\nTime for Stealin!\nThe cards will now be resolved starting with Ghost.\n",//9

		"Stealin, Resolving Move: Ghost moved to the adjacent car\nCheyenne's card will now be resolved\n",
		"Stealin, Resolving ChangeFloor: Cheyenne moved to the top of the car\nTime for Ghost to choose to pick one loot\n",
		"Stealin, Resolving Rob: Ghost chooses one gem to add to his loot\nCheyenne's card will now be resolved\n",
		"Stealin, Resolving MoveMarshal: Cheyenne moved the Marshal\nDjango's card will now be resolved.\nDjango must punch Ghost.Time for Django to choose which loot to force Ghost to drop\n", 
		"Stealin, Resolving Punch: Django chose the loot\nTime for Django to choose where to punch Ghost to\n",
		"Punch: Django chooses to punch Ghost to the last train car\nTime for Django to choose who to shoot\n",
		"Stealin, Resolving Shoot: Django shoots Ghost\nNew Round, SpeedingUp! 1 SpeedingUp turn. New Player Order: Cheyenne, Django, Ghost\nIt is now Cheyenne's turn to play a card or draw 3 cards.\n",
		"SpeedingUp Turn 1 (Cheyenne): Cheyenne played a MOVE card\nIt is now Cheyenne's turn to play a card or draw 3 cards.\n", 
		"SpeedingUp Turn 2 (Cheyenne): Cheyenne chose to draw cards\nIt is now Django's turn to play a card or draw 3 cards.\n",
		"SpeedingUp Turn 1 (Django): Django played a CHANGEFLOOR card\nIt is now Django's turn to play a card or draw 3 cards.\n",//19
		"SpeedingUp Turn 2 (Django): Django chose to draw cards\nIt is now Ghost's turn to play a card or draw 3 cards.\n",
		"SpeedingUp Turn 1 (Ghost): Ghost chose to draw cards\nIt is now Ghost's turn to play a card or draw 3 cards.\n",
		"SpeedingUp Turn 2 (Ghost): Ghost played a CHANGEFLOOR card\nStealin Time!\nCheyenne's card will now be resolved\n",
		"Stealin, Resolving Move: Cheyenne moves to the adjacent train car\nDjango's card will now be resolved\n", //24
		"Stealin, Resolving ChangeFloor: Django is moved to the top of the car\nGhost's card will now be resolved\n",
		"Stealin, Resolving ChangeFloor: Ghost is moved to the top of the car\nCalculating Scores\n",
		"Results: Game has ended. WINNER: Django $1,250 (Gunslinger); Ghost $500; Cheyenne $250\n", //27
		""
		}; // 

    private List<float> cartZeroTop = new List<float>() {840.5F,878.4F,-364.9F};
    private List<float> cartZeroBtm = new List<float>() {786.1F, 813.5F, -364.9F};

    private List<float> cartOneTop = new List<float>() {1025.7F, 889.4F, -364.9F};
    private List<float> cartOneBtm = new List<float>() {1027.9F, 806.4F, -364.9F};

    private List<float> cartTwoTop = new List<float>() {1265.4F, 894.7F, -364.9F};
    private List<float> cartTwoBtm = new List<float>() {1279.8F, 817.7F, -364.9F};

    private List<float> cartLocoTop = new List<float>() {1410.5F, 893.4F, -364.9F};
    private List<float> cartLocoBtm = new List<float>() {1390.0F, 824.9F, -364.9F};

    private List<float> iconPosition = new List<float>() {1285.9F, 1121.9F, -364.9F};
	private List<float> gemPosition = new List<float>() {1224.1F, 1077.2F, -364.9F};

    void Start(){
		makeAllClickable();
		Debug.Log(gm.currentRound.roundTypeAsString);
		currentRound.text = gm.currentRound.roundTypeAsString; 
		currentBandit.text = gm.currentBandit.banditNameAsString; 

        gem4.SetActive(false);
		initCards();
		// set extra cards to false 
		CardNewA.SetActive(false);
		CardNewB.SetActive(false);
		CardNewC.SetActive(false);
		CardNewD.SetActive(false);
		CardNewE.SetActive(false);
		CardNewF.SetActive(false);
		announcement.text = "";
		// Round.text = "ROUND 1:\n-Standard turn\n-Tunnel turn\n-Switching turn";

		
		SFS.setGameBoard();
		announcement.text = logMessages[SFS.step];
		gem2.SetActive(false);

		//EnterGameBoardScene();
		exitText.text = ""; 
		clickableGOsText.text = "";
		// init clickables should be called on update
		initClickables();
		exitText.text =""; 
    }

	// makeAllClickable makes all clickable objects clickable 
	public void makeAllClickable(){
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach(GameObject go in allObjects){
			go.SetActive(true);
		}
	}

	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");
		
		// REASSIGN ALL GAME OBJECTS USING DICTIONARY
		ArrayList banditsArray = gm.bandits;
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

		ArrayList lootArray = gm.loots;
		foreach (Loot l in lootArray) {
            if (l.belongsTo.banditNameAsString == "CHEYENNE") {
				objects[gem1] = l;
                trace("Gem 1 added!");
            }
            if (l.belongsTo.banditNameAsString == "BELLE") {
				objects[gem2] = l;
                trace("Gem 2 added!");
            }
            if (l.belongsTo.banditNameAsString == "TUCO") {
				objects[gem3] = l;
                trace("Gem 3 added!");
            }
            if (l.belongsTo.banditNameAsString == "DOC") {
				objects[gem4] = l;
                trace("Gem 4 added!");
            }
            if (l.belongsTo.banditNameAsString == "GHOST") {
				objects[gem5] = l;
                trace("Gem 5 added!");
            }
			if (l.belongsTo.banditNameAsString == "DJANGO") {
				objects[gem6] = l;
                trace("Gem 6 added!");
            }
			// check if a loot belongs to TrainUnit and assign it 
		}

		ArrayList bulletCards = gm.neutralBulletCard;
		foreach(BulletCard bc in bulletCards){
			// get the owner of the bullet card 
			string owner = bc.getOwner(); 
			if(owner == "BELLE"){
				objects[BelleBulletCard1] = bc; 
				// @TODO: Do we distinguish the difference between number of bullets? 

			}

			if(owner == "CHEYENNE"){

			}

			if(owner == "DOC"){
				
			}

			if(owner == "DJANGO"){
				
			}

			if(owner == "GHOST"){
				
			}

			if(owner == "TUCO"){
				
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

		// assign currRound and currPlayer 
		currentRound.text += gm.currentRound.roundTypeAsString; 
		currentBandit.text += gm.currentBandit.banditNameAsString; 
    }

	public void LeaveRoom() {
        SFS.LeaveRoom();
    }

    public void initClickables(){
        // Debug.Log("initClickables CALLS YOU TO WORK HARDER, GM!!!");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject go in allObjects){
            // clickableGOsText.text += go.name;
            // https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
            if(go.activeSelf == true){
                clickableGOsText.text += go.name;
            }
        }
        clickableGOsText.text += "==== NOW GHOST IS SET TO NONACTIVE ===";
        ghost.SetActive(false);
        foreach(GameObject go in allObjects){
            if(go.activeSelf == true){
                clickableGOsText.text += go.name;
            }
        }
    }

	public void initCards(){
		// draws 6 cards randomly and put in the hand
		cardAText.text = "MOVE";
		cardBText.text = "ROB";
		cardCText.text = "MARSHAL"; 
		cardDText.text = "CHANGE FLOOR";
		cardEText.text = "SHOOT"; 
		cardFText.text = "PUNCH"; 
		return;
	}

	public void playCard(GameObject selectedCard){
		// draws 3 cards randomly and put in the hand
		Destroy(selectedCard);
	}

	public void MouseDown() {
		SFS.step += 1;
		int step = SFS.step;
		ISFSObject obj = SFSObject.NewInstance();
		obj.PutInt("step", step);
		ExtensionRequest req = new ExtensionRequest("gm.nextAction",obj);
		SFS.Send(req);
		//executeHardCoded(step);
		if (SFS.step == 27){
			LeaveRoom();
		}
	}

	public void drawCards(/*string currentChar*/){
		CardNewA.SetActive(true);
		CardNewB.SetActive(true);
		CardNewC.SetActive(true);
	}

	public void drawCardsSecond(/*string currentChar*/){
		CardNewD.SetActive(true);
		CardNewE.SetActive(true);
		CardNewF.SetActive(true);
	}

	public void executeHardCoded(int step) {
		announcement.text += "\n";

		if(step % 3 == 0){
			announcement.text = ""; 
		}
		announcement.text += logMessages[SFS.step];

		switch(step) {
			case 0:
				//round,turn info
				//"Angry Marshal Round! 1 Standard turns, 1 Tunnel turn, and 1 Switching turn",
				//Its yyy's turn to play a card or draw 3 cards.
				break;
			case 1: 
				if(ChooseCharacter.character == "GHOST"){
					playCard(cardA);
				}
				break;
			case 2:
				//"Standard Turn: Cheyenne played a CHANGEFLOOR card",
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardD);
				}
				break;
			case 3:
				//Standard Turn: Django chose to draw cards",
				// drawCards("DJANGO", step);
				// DRAW CARDS 
				if(ChooseCharacter.character == "DJANGO"){
					drawCards(); 
				}
				break;
			case 4:
				//"Tunnel Turn: Ghost played an action card which is hidden",
				if(ChooseCharacter.character == "GHOST"){
					playCard(cardB);
				}
				break;
			case 5:
				//"Tunnel Turn: Cheyenne played an action card which is hidden",
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardC);
				}
				break;
			case 6:
				//"Tunnel Turn: Django played an action card which is hidden",
				if(ChooseCharacter.character == "DJANGO"){
					playCard(cardF);
				}
				break;
			case 7:
				//"Switching Turn: ",
				if(ChooseCharacter.character == "GHOST"){
					drawCards();
				}
				//"Switching Turn: Ghost chose to draw cards",
				break;
			case 8:
				if(ChooseCharacter.character == "DJANGO"){
					playCard(cardE);
				}
				break;
			case 9:
				if(ChooseCharacter.character == "CHEYENNE"){
					drawCards();
				}
				//"Switching Turn: Cheyenne chose to draw cards",
				break;
			case 10:
				//"Stealin, Resolving Move: Ghost moved to the adjacent car",
				ghost.transform.position = new Vector3 (cartOneBtm[0] - 1F, cartOneBtm[1], cartOneBtm[2]);
                		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
				break;
			case 11:
				//"Stealin, Resolving ChangeFloor: Cheyenne moved to the top of the car",
				cheyenne.transform.position = new Vector3 (cartZeroTop[0] + 5F, cartZeroTop[1], cartZeroTop[2]);
                		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;
			        // Destroy(gem3);
				break;
			case 12:
				//"Stealin, Resolving Rob: Ghost chooses one gem to add to his loot"
				Destroy(gem3);
				gem4.SetActive(true);
				break;
			case 13:
				//"Stealin, Resolving MoveMarshal: Cheyenne moved the Marshal",
				marshal.transform.position = new Vector3 (cartTwoBtm[0], cartTwoBtm[1], cartTwoBtm[2]);
                		marshal.transform.position += marshal.transform.forward * Time.deltaTime * 5f;
				break;
			case 14:
				// "Stealin, Resolving Punch: Django must punch Ghost,Time for Django to choose which loot to force Ghost to drop"
				gem2.SetActive(true); //purse appears
				Destroy(ghoLoot);
				break;
			case 15:
				//"Stealin, Resolving Punch: Django chose the loot.\nTime for Django to choose where to punch Ghost to\n",//15
				//"Punch: Django chooses to punch Ghost to the last train car",
				punch(); //moves ghost
				break;
			case 16:
			//"Punch: Django chooses to punch Ghost to the last train car\nTime for Django to choose who to shoot\n",
			// "Stealin, Resolving Shoot: Django shoots Ghost",// "New Round, SpeedingUp! 1 SpeedingUp turn",
			   	shoot();
				Round.text = "ROUND 2:\n-SpeedingUp turn";
				if(ChooseCharacter.character == "DJANGO"){
					Destroy(cardA);
				}
				else if(ChooseCharacter.character == "CHEYENNE"){
					Destroy(CardNewC);
				} else if(ChooseCharacter.character == "GHOST") {
					Destroy(cardE);
				}
				break;
			case 17:	
				// "SpeedingUp Turn 1 (Cheyenne): Cheyenne played a MOVE card",  
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardA);
				}
				break;
			case 18:
				// "SpeedingUp Turn 2 (Cheyenne): Cheyenne chose to draw cards",
				if(ChooseCharacter.character == "CHEYENNE"){
					drawCardsSecond();
				}
				break;
			case 19:
				if(ChooseCharacter.character == "DJANGO"){
							playCard(CardNewB);
				}
				// "SpeedingUp Turn 1 (Django): Django played a CHANGEFLOOR card", 
				break;
			case 20:
				if(ChooseCharacter.character == "DJANGO"){
					drawCardsSecond();
				}
				// "SpeedingUp Turn 2 (Django): Django chose to draw cards",
				break;
			case 21:
				if(ChooseCharacter.character == "GHOST"){
						drawCardsSecond();
				}
				// "SpeedingUp Turn 1 (Ghost): Ghost chose to draw cards",
				break;
			case 22:
				if(ChooseCharacter.character == "GHOST"){
					playCard(CardNewB);
				}
				// "SpeedingUp Turn 2 (Ghost): Ghost played a CHANGEFLOOR card",
				break;
			case 23:
				// "Stealin, Resolving Move: Cheyenne moves to the adjacent train car",
			        cheyenne.transform.position = new Vector3 (cartOneTop[0] + 5F, cartOneTop[1], cartOneTop[2]);
                   		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;	
				break;
			case 24:
				// "Stealin, Resolving ChangeFloor: Django is moved to the top of the car",
			        django.transform.position = new Vector3 (cartOneTop[0] - 5F, cartOneTop[1], cartOneTop[2]);
                    		django.transform.position += django.transform.forward * Time.deltaTime * 10f;	
				break;
			case 25:
				// "Stealin, Resolving ChangeFloor: Ghost is moved to the top of the car",
			        ghost.transform.position = new Vector3 (cartZeroTop[0] - 1F, cartZeroTop[1], cartZeroTop[2]);
                    		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
				break;
			case 26: 
				// "Results: Game has ended. ADD SCORES Django is the winner!" 
				break;
			case 27:
				Debug.Log("Leaving room");
				//LeaveRoom();
				break;
		}
    }


	public void rob(){
		gem3.transform.position = new Vector3 (gemPosition[0], gemPosition[1], gemPosition[2]); //(1224.1, 1077.2, -364.9)
        gem3.transform.position += gem3.transform.forward * Time.deltaTime * 5f;
	}

	public void punch(){
		Debug.Log("GHOST IS PUNCHED");
        float posX = cartZeroBtm[0]; 
        float posY = cartZeroBtm[1]; 
        float posZ = cartZeroBtm[2]; 
        ghost.transform.position = new Vector3 (posX, posY, posZ);
        ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f; // can be any float number
		// shoot();  
	}

	public void shoot(){
		Debug.Log("GHOST IS SHOT");
		bulletCard.transform.position = new Vector3 (iconPosition[0], iconPosition[1], iconPosition[2]);
        bulletCard.transform.position += bulletCard.transform.forward * Time.deltaTime * 2f;
	}


    void Update(){
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		if (Input.GetMouseButtonDown(0)){
			// Debug.Log(this.gameObject.name);
			var clickedObjectName = this.gameObject.name; 
			// check if this obj is clickable using the List<obj>
			foreach(GameObject go in clickableGOs){
				if(go.name == clickedObjectName){
					// curr GO is clickable
					object clickableObj = objects[go];
					// pass clickableObj back! 
					// TODO: @Backend Team : action is the name(all caps) of the next method 
					// gm.action(clickableObj);
				}
			}

		}
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

    public static void trace(string msg) {
	//	debugText.text += (debugText.text != "" ? "\n" : "") + msg;
	}

	public void GoToWaitingRoom(){
		Invoke("GoToWaitingRoom2",5);
	}

	void GoToWaitingRoom2(){
		SceneManager.LoadScene("WaitingRoom");
	}

    void OnApplicationQuit() {
		ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}
}
