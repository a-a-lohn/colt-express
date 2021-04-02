using model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

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
using System.Reflection;
using System;
using Random=System.Random;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
	private static RestClient client = new RestClient("http://13.72.79.112:4242");
	public static string gameHash = WaitingRoom.gameHash;
	public static string savegameId = null;
	public static bool started = false;

	//debug variables
	public static Text debugText;
	public static string debugTextString;
    // public Button button;
	// public Button extension;
	// public Button chooseChar;

	public static bool works = false;
	public Text doesItWork;

	public static void setWorks() {
		Debug.Log("it works!");
		works = true;
	}

	public GameObject canvas;

	public Text Round;
	//public GameObject exit;
	public Text exitText;
	
    public Bandit b;

	public static GameManager gm;

	// LIST OF ALL GAME buttonToObject HERE
    public Button cheyenne;
	public Button belle; 
	public Button tuco; 
	public Button doc; 
	public Button ghost; 
	public Button django; 
	public Button marshal;
	private List<Button> playingBandits = new List<Button>();
	private List<Button> allBandits = new List<Button>();
	
	// public Button gem1; 
	// public Button gem2; 
	// public Button gem3; 
	// public Button gem4;
	// public Button gem5;
	// public Button gem6;

	// public Button ghoLoot;

	public GameObject bulletCard;

	// propmpt messages 
	public Text promptDrawCardsOrPlayCardMsg;
	public Text promptChooseLoot; 
	public Text promptPunchTarget; 

	public static Dictionary<Button, object> buttonToObject = new Dictionary<Button, object>();

	public static ArrayList clickable = new ArrayList();
	public static string action = "";

	// public Text clickableGOsText;
	public Text currentRound; 
	public Text currentBandit; 

	private List<Button> goNeutralBulletCards;

	// a list of bullet cards for each and every bandit 
	private List<Button> goBELLEBulletCards; 
	private List<Button> goCHEYENNEBulletCards; 
	private List<Button> goDOCBulletCards; 
	private List<Button> goTUCOBulletCards; 
	private List<Button> goDJANGOBulletCards; 
	private List<Button> goGHOSTBulletCards; 

	// a list of action cards for each and every bandit's hand 
	private List<Button> goBELLEHand; 
	private List<Button> goCHEYENNEHand; 
	private List<Button> goDOCHand; 
	private List<Button> goGHOSTHand; 
	private List<Button> goTUCOHand; 
	private List<Button> goDJANGOHand; 


	private List<GameObject> clickableGOs; 

	public Button handCard1; 
	public Button handCard2; 
	public Button handCard3; 
	public Button handCard4; 
	public Button handCard5;
	public Button handCard6; 
	public Button handCard7; 
	public Button handCard8; 
	public Button handCard9;  
	public Button handCard10; 
	public Button handCard11; 
	private List<Button> goHandCard = new List<Button>(); 
	
	/* a card has 4 attributes */
	public Text handCardActionType1; 
	// public Text handCardOneSaveForNetRound;
	// public Text handCardOneIsFaceDown; 
	// public Text handCardOneBelongsTo;

	public Text handCardActionType2;
	// public Text handCardTwoSaveForNetRound;
	// public Text handCardTwoIsFaceDown; 
	// public Text handCardTwoBelongsTo;

	public Text handCardActionType3; 
	// public Text handCardThreeSaveForNetRound;
	// public Text handCardThreeIsFaceDown; 
	// public Text handCardThreeBelongsTo;

	public Text handCardActionType4; 
	// public Text handCardFourSaveForNetRound;
	// public Text handCardFourIsFaceDown; 
	// public Text handCardFourBelongsTo;

	public Text handCardActionType5; 
	public Text handCardActionType6; 
	public Text handCardActionType7; 
	public Text handCardActionType8; 
	public Text handCardActionType9; 
	public Text handCardActionType10; 
	public Text handCardActionType11; 

	/* TrainUnit */
	public Button trainOneBtm; 
	public Button trainOneTop; 
	public Button trainTwoBtm; 
	public Button trainTwoTop;
	public Button trainThreeBtm; 
	public Button trainThreeTop;
	public Button trainFourBtm; 
	public Button trainFourTop;
	// public Button trainFiveBtm; 
	// public Button trainFiveTop;
	public Button locoBtm; 
	public Button locoTop;

	public List<Button> trainRoofs; 
	public List<Button> trainCabins; 

	/* horses ?*/
	
    public static string punchedBandit;

	public List<ActionCard> actionCardList; 

	bool calledMapTrain = false;

    void Start(){
		//setAllNonClickable();
		addAllBandits();
		Round.text = "ROUND 1:\n-Standard turn\n-Tunnel turn\n-Switching turn";
		SFS.setGameBoard();

		exitText.text ="";
		doesItWork.text = "";
		//Invoke("LeaveRoom",5);
		/*if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
        }
        if (!SFS.IsConnected()) {
            SFS.Connect("test");
        }*/
		initMap();
		EnterGameBoardScene();
    }

	public void addAllBandits(){
		allBandits.Add(belle);
		allBandits.Add(cheyenne);
		allBandits.Add(doc);
		allBandits.Add(django);
		allBandits.Add(tuco);
		allBandits.Add(ghost);
		allBandits.Add(marshal);
	}

	/* initMap initializes the <Button, object> hashmap */
	public void initMap(){
		/* init. current hand */
		buttonToObject.Add(handCard1, "null"); 
		buttonToObject.Add(handCard2, "null"); 
		buttonToObject.Add(handCard3, "null"); 
		buttonToObject.Add(handCard4, "null"); 
		buttonToObject.Add(handCard5, "null"); 
		buttonToObject.Add(handCard6, "null"); 
		buttonToObject.Add(handCard7, "null"); 
		buttonToObject.Add(handCard8, "null"); 
		buttonToObject.Add(handCard9, "null"); 
		buttonToObject.Add(handCard10, "null"); 
		buttonToObject.Add(handCard11, "null"); 

		/* init. all bandits */
		buttonToObject.Add(belle, "null"); 
		buttonToObject.Add(cheyenne, "null"); 
		buttonToObject.Add(doc, "null"); 
		buttonToObject.Add(django, "null"); 
		buttonToObject.Add(tuco, "null"); 
		buttonToObject.Add(ghost, "null"); 
		buttonToObject.Add(marshal, "null");


		/* init all traincarts */
		buttonToObject.Add(trainOneBtm, "null"); 
		buttonToObject.Add(trainOneTop, "null"); 
		buttonToObject.Add(trainTwoBtm, "null"); 
		buttonToObject.Add(trainTwoTop, "null"); 
		buttonToObject.Add(trainThreeBtm, "null"); 
		buttonToObject.Add(trainThreeTop, "null"); 
		buttonToObject.Add(trainFourBtm, "null"); 
		buttonToObject.Add(trainFourTop, "null");
		buttonToObject.Add(locoBtm, "null"); 
		buttonToObject.Add(locoTop, "null");

		trainCabins.Insert(0, locoBtm);
		trainCabins.Insert(1, trainOneBtm);
		trainCabins.Insert(2, trainTwoBtm);
		trainCabins.Insert(3, trainThreeBtm);
		trainCabins.Insert(4, trainFourBtm);

		trainRoofs.Insert(0, locoTop);
		trainRoofs.Insert(1, trainOneTop);
		trainRoofs.Insert(2, trainTwoTop);
		trainRoofs.Insert(3, trainThreeTop);
		trainRoofs.Insert(4, trainFourTop);

		goHandCard.Insert(0, handCard1);
		goHandCard.Insert(1, handCard2);
		goHandCard.Insert(2, handCard3);
		goHandCard.Insert(3, handCard4);
		goHandCard.Insert(4, handCard5);
		goHandCard.Insert(5, handCard6);
		goHandCard.Insert(6, handCard7);
		goHandCard.Insert(7, handCard8);
		goHandCard.Insert(8, handCard9);
		goHandCard.Insert(9, handCard10);
		goHandCard.Insert(10, handCard11);
	}

	void clearHand(){
		buttonToObject[handCard1] = "null"; 
		buttonToObject[handCard2] = "null"; 
		buttonToObject[handCard3] = "null"; 
		buttonToObject[handCard4] = "null";
		buttonToObject[handCard5] = "null"; 
		buttonToObject[handCard6] = "null"; 
		buttonToObject[handCard7] = "null"; 
		buttonToObject[handCard8] = "null"; 
		buttonToObject[handCard9] = "null"; 
		buttonToObject[handCard10] = "null"; 
		buttonToObject[handCard11] = "null"; 
	}

	// public void mapTrain(GameManager gm){
	// 	 public ArrayList trainRoof ;
    //     public ArrayList trainCabin;
	// 	foreach(object oneRoof in gm.trainRoof){
	// 		buttonToObject[] = oneRoof;

	// 	}
	// }

	public void mapBanditPositions(GameManager gm){
		foreach(TrainUnit cabin in gm.trainCabin){
			ArrayList occupied = cabin.getBanditsHere();
			foreach(Bandit b in occupied) {

				TrainUnit t = b.getPosition();
				Button button = buttonToObject.FirstOrDefault(x => x.Value.Equals(t)).Key;

				if(b.getCharacter().ToLower() == "belle"){
					Vector3 temp = button.transform.position;
					belle.transform.position = temp; // might need a delta function here 
				} else if(b.getCharacter().ToLower() == "cheyenne"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				} else if(b.getCharacter().ToLower() == "django"){
					Vector3 temp = button.transform.position;
					django.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "doc"){
					Vector3 temp = button.transform.position;
					doc.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "ghost"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "tuco"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				}	
			}
			}	 
	}

	public void mapTrain(GameManager gm){
		int index = 0; 
		foreach(object oneRoof in gm.trainRoof){
			buttonToObject[trainRoofs[index]] = oneRoof;
			index++;
		}
		index = 0;
		foreach(object oneCab in gm.trainCabin){
			buttonToObject[trainCabins[index]] = oneCab;
			index++;
		}
	}

	/* promptDrawOrPlayMessage displays the prompt message on gameboard*/
	public static void promptDrawOrPlayMessage(){
		// promptDrawCardsOrPlayCardMsg.text = "Please play a card or draw 3 cards!";
		// GameObject GameBoardGameObject = GameObject.Find("GameBoardGO");
		// GameBoardGameObject.
	}

	public void addNullListToMap(List<Button> aBtnList){
		foreach(Button aBtn in aBtnList){
			buttonToObject.Add(aBtn, "null");
		}
	}

 	public void buttonClicked(Button btn){
		if(!isMyTurn()) {
			Debug.Log("not my turn!");
		} else {
			Debug.Log( btn.name + " IS CLICKED");
			Debug.Log("Clickable has " + clickable.Count + "items");
			promptPunchTarget.text = btn.name + "IS CLICKED"; 
			//punchedBandit = btn.name;
			// if buttonToObject[btn] is an actioncard, call playCard(buttonToObject[btn])
			if(clickable.Contains(btn)) {
				Debug.Log("this is a clickable item!");
				//all calls back to GM should be here
			}
			try {
				ActionCard currActionCard = (ActionCard)buttonToObject[btn];
				gm.playCard(currActionCard); 
			} catch(Exception e) {
				Debug.Log("not an action card");
			}
		}
    }

	/* setAllNonClickable sets all buttons to be non-clickable */
	public void setAllNonClickable(){
		Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
		foreach(Button aBtn in allButtons){
			aBtn.interactable = false; 
		}
	}

	public void setAllClickable(){
		Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
		foreach(Button aBtn in allButtons){
			aBtn.interactable = true; 
		}
	}

	// prompt message: when a new state comes in, assign the non-static string using the str from the GM 
	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public void UpdateGameState(BaseEvent evt) {
		// if(!calledMapTrain){
		// 	mapTrain(gm);
		// 	calledMapTrain = true;
		// }
        Debug.Log("updategamestate called");
		setAllClickable();
		clearHand();
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		doesItWork.text = responseParams.GetUtfString("log");
		Debug.Log("Received log message: "+ (string)responseParams.GetUtfString("log"));
		gm = (GameManager)responseParams.GetClass("gm");
		GameManager.replaceInstance(gm);

		//actionCardList = new List<ActionCard>(); 

		// REASSIGN ALL GAME buttonToObject USING DICTIONARY
		ArrayList banditsArray = gm.bandits;
		foreach (Bandit b in banditsArray) {
            if (b.characterAsString == "CHEYENNE") {
				buttonToObject[cheyenne] = b;
				Debug.Log(b.characterAsString + " added as button");
				playingBandits.Add(cheyenne);
            }
			if (b.characterAsString == "BELLE") {
                buttonToObject[belle] = b;
				Debug.Log(b.characterAsString + " added as button");
               	playingBandits.Add(belle);
            }
			if (b.characterAsString == "TUCO") {
                buttonToObject[tuco] = b;
				Debug.Log(b.characterAsString + " added as button");
				playingBandits.Add(tuco);
            }
			if (b.characterAsString == "DOC") {
                buttonToObject[doc] = b;
				Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(doc);
            }
			if (b.characterAsString == "GHOST") {
                buttonToObject[ghost] = b;
				Debug.Log(b.characterAsString + " added as button");
               	playingBandits.Add(ghost);
            }
			if (b.characterAsString == "DJANGO") {
                buttonToObject[django] = b;
				Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(django);
            }
			addAllBandits();
			/* only keep the playing bandits */
			foreach(Button ab in allBandits){
				if(!playingBandits.Contains(ab)){
					Destroy(ab);
				}
			}

			/* init bandit position */

			//UPDATE HAND/DECK EVERY TIME

			if(b.characterAsString == gm.currentBandit.characterAsString){
				/*
				* OBJECTS ARE NEWLY CREATED WHEN SERIALIZED. IF MULTIPLE REFERENCES EXIST FOR THE SAME OBJECT, THEY WILL BE TREATED AS DIFFERENT OBJECTS
				*/
				Debug.Log("reassigning current bandit ref to " + b.characterAsString);
				gm.currentBandit = b;
			}

			if(b.characterAsString == ChooseCharacter.character){
				b.updateMainDeck();

				if (gm.strGameStatus.Equals("SCHEMIN")) {
					if(gm.currentRound.getTurnCounter() == 0 && b.hand.Count == 0){
						b.drawCards(6);
						if(b.getCharacter().Equals("DOC")){
							b.drawCards(1);
						}
						b.updateOtherDecks();
						b.updateOtherHands();
					}
				}
				
				b.updateMainHand();
				// assign to gameobjects on screen 
				//ArrayList currCards = b.hand;
				int index = 0; 
				ActionCard ac;
				BulletCard bc;
				Debug.Log("num of currcards: " + b.hand.Count);
				Debug.Log("num of currcards b1: " + gm.currentBandit.hand.Count);
				foreach(Card currCard in b.hand){
					try{
						ac = (ActionCard) currCard;
						buttonToObject[goHandCard[index]] = ac;
						//Debug.Log("trying to cast card as action card");
					} catch(Exception e) {
						bc = (BulletCard) currCard;
						buttonToObject[goHandCard[index]] = bc;
						Debug.Log("not initializing an action card");
					}
					index++;
				}
				mapActionCards(handCard1, handCardActionType1);
				mapActionCards(handCard2, handCardActionType2);
				mapActionCards(handCard3, handCardActionType3);
				mapActionCards(handCard4, handCardActionType4);
				mapActionCards(handCard5, handCardActionType5);
				mapActionCards(handCard6, handCardActionType6);
				mapActionCards(handCard7, handCardActionType7);
				mapActionCards(handCard8, handCardActionType8);
				mapActionCards(handCard9, handCardActionType9);
				mapActionCards(handCard10, handCardActionType10);
				mapActionCards(handCard11, handCardActionType11);
			}
		}
		Debug.Log(SFS.step);

		gm.playTurn();

    }

	public static bool isMyTurn() {
		if(gm.currentBandit.getCharacter().Equals(ChooseCharacter.character)){
			return true;
		}
		return false;
	}


	public void mapActionCards(Button button, Text buttonText/*List<Button> goHand, string actionName, Card c, string banditName*/){
		// foreach(Button g in goHand){
		// 	string goName = banditName + g.name;
		// 	if(actionName == goName.ToUpper()){
        //     	buttonToObject[g] = c;
		// 	}
		// }
		//Debug.Log("Called mapactioncards");

		try {
			string nullstr = (string)buttonToObject[button];
			buttonText.text = "";
			button.interactable = false;
			return;
		} catch(Exception e) {
		}

		try {
			ActionCard card = (ActionCard)buttonToObject[button];
			buttonText.text = card.actionTypeAsString;
			Debug.Log("setting " + button.name + " to " + card.actionTypeAsString);
		} catch(Exception e) {
			//Debug.Log("not an action card in MAP");
			buttonText.text = "Bullet";
		}
	}

	public void LeaveRoom() {
        SFS.LeaveRoom();
    }

	public void playCard(GameObject selectedCard){
		// draws 3 cards randomly and put in the hand
		Destroy(selectedCard);
	}

	/* Map all Buttons to their GM objects counterparts */
	public void mapAll(){
		
	}
	
	/* makeShootPossibilitiesClickable makes all possibilities clickable */
	public static void makeShootPossibilitiesClickable(ArrayList possibilities){
		Debug.Log("HELLO FROM makeShootPossibilitiesClickable");

		foreach(Bandit b in possibilities){
			foreach(Button oneBtn in buttonToObject.Keys){
				if(b.characterAsString == oneBtn.name.ToUpper()){
					oneBtn.interactable = true; 
				}
			}
		}
	}

    /* makePunchPossibilitiesClickable makes all possibilities clickable AND returns the clicked Bandit's name as a string */
    public static string makePunchPossibilitiesClickable(ArrayList possibilities){
        foreach(Bandit b in possibilities){
            foreach(Button oneBtn in buttonToObject.Keys){
                if(b.characterAsString == oneBtn.name.ToUpper()){
                    oneBtn.interactable = true; 
                }
            }
        }

        // user clicks on one of the highlighted bandits 
        while(punchedBandit is null){
            makePunchPossibilitiesClickable(possibilities);
        }   

        Debug.Log("PASSED BACK TO GM");
        return punchedBandit; 
    }


    // Update is called once per frame
    void Update()
    {

		// var selectedBanditName = EventSystem.current.currentSelectedGameObject;
        //  if (selectedBanditName != null)
        //      promptPunchTarget.text = "ahh" + selectedBanditName.name;
        //  else
        //      promptPunchTarget.text = "ahh NULLL POINTERR";

        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		if (Input.GetMouseButtonDown(0)){
			// MouseDown();
			Debug.Log("Clicked");
			works = false;
			Debug.Log("currentbandit on mouse: "+ gm.currentBandit.getCharacter());
			// if(gm != null && gm.currentBandit.getCharacter() == ChooseCharacter.character) {
			// 	Debug.Log("ending my turn");
				// Bandit b = (Bandit) gm.bandits[0];
				// if (b.getCharacter() == gm.currentBandit.getCharacter()) {
				// 	gm.currentBandit = (Bandit) gm.bandits[1];
				// } else {
				// 	gm.currentBandit = (Bandit) gm.bandits[0];
				// }
				// gm.endOfTurn();
				//SendNewGameState();
			// }
		}

		if(works) {
			doesItWork.text = "it works!";
		} else {
			doesItWork.text = "";
		}
    }

	public void EnterGameBoardScene() {
		Debug.Log("entering scene");
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterGameBoardScene",obj);
        SFS.Send(req);
        Debug.Log("Sent enter scene message");
	}

	public static void SendNewGameState(string message) {
		ISFSObject obj = SFSObject.NewInstance();
		//Debug.Log("sending new game state");
		obj.PutClass("gm", gm);
		//obj.PutUtfString("log", message);
        ExtensionRequest req = new ExtensionRequest("gm.newGameState",obj);
        SFS.Send(req);
        Debug.Log("sent game state");
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

	public void GoToWaitingRoom(){
		Invoke("GoToWaitingRoom2",5);
	}

	void GoToWaitingRoom2(){
		SceneManager.LoadScene("WaitingRoom");
	}

	public void GoToChat(){
		SceneManager.LoadScene("Chat");
	}

    void OnApplicationQuit() {
		ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}


	/*
	 The methods below implements the save game and launch saved game features
	
	*/
	private static string GetAdminToken() {
        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
            
        var obj = JObject.Parse(response.Content);
        string adminToken = (string)obj["access_token"];
        adminToken = adminToken.Replace("+", "%2B");

		return adminToken;
    }

	public void TestSave() {
		SaveGameState("test");
	}

	public static void SaveGameState(string savegameID) {

		//ONLY NEED TO SEND THE SAVEGAME REQUEST TO THE LS ONCE
		//(although making the same call multiple times can't hurt, and is simpler)
		/*var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var JObj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = JObj.ToObject<Dictionary<string, object>>();

		dynamic j = new JObject();
		var temp = JsonConvert.SerializeObject(sessionDetails["gameParameters"]);
		var gameParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(temp);

		string gameName = gameParameters["name"];
		j.gamename = gameName; // can replace with "ColtExpress"
		//below I deserialize a JSON object to a collection		
		temp = JsonConvert.SerializeObject(sessionDetails["players"]);
		j.players = JsonConvert.DeserializeObject<List<string>>(temp);//In case it doesn't work, debug by adding a .ToArray()
		j.savegameid = savegameID;

		request = new RestRequest("api/gameservices/" + gameName + "/savegames/" + savegameID + "?access_token=" + GetAdminToken(), Method.POST)
            .AddParameter("application/json", j.ToString(), ParameterType.RequestBody)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");

        response = client.Execute(request);*/

		// After saving the game, store the information to the server

		ISFSObject obj = SFSObject.NewInstance();
		Debug.Log("saving the current game state on the server");
		obj.PutUtfString("savegameId", savegameID);
		obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.saveGameState",obj);
        SFS.Send(req);
	}

	public static void promptHorseAttack(int trainIndex) {
		if (gm.bandits.Count == gm.banditPositions.Count) {
			started = true;
			return;
		}
		ISFSObject obj = SFSObject.NewInstance();
		if (1==1) { //gm.banditPositions.Contains()
			return;
		}
		String response;
		//prompt user whether they want to get off at this train (indicated by trainIndex). If yes, response should be "y", if no then "n"
		obj.PutUtfString("ans", response);
		ExtensionRequest req = new ExtensionRequest("gm.choosePosition", obj);
		SFS.Send(req);
	}
}
