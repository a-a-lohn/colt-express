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
using Random=System.Random;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
	private static RestClient client = new RestClient("http://13.72.79.112:4242");
	public static string gameHash = WaitingRoom.gameHash;
	public static string savegameId = null;

	//debug variables
	public static Text debugText;
	public static string debugTextString;
    public Button button;
	public Button extension;
	public Button chooseChar;

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
	
	public Button gem1; 
	public Button gem2; 
	public Button gem3; 
	public Button gem4;
	public Button gem5;
	public Button gem6;

	public Button ghoLoot;

	public GameObject bulletCard;

	// propmpt messages 
	public Text promptDrawCardsOrPlayCardMsg;
	public Text promptChooseLoot; 
	public Text promptPunchTarget; 

	public static Dictionary<Button, object> buttonToObject = new Dictionary<Button, object>();

	public static ArrayList clickable = new ArrayList();
	public static string action = "";

	public Text cardNewAText;
	public Text cardNewABext;
	public Text cardNewCText;

	public GameObject playerE;

	public GameObject BelleBulletCard1; 
	public GameObject BelleBulletCard2;
	public GameObject BelleBulletCard3;
	public GameObject BelleBulletCard4;
	public GameObject BelleBulletCard5;
	public GameObject BelleBulletCard6;      

	public GameObject BelleActionMove; 
	public GameObject BelleActionChangeFloor; 
	public GameObject BelleActionPunch; 
	public GameObject BelleActionShoot; 

	public Text clickableGOsText;
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

	public Button ghostCard1; 
	public Button ghostCard2; 
	public Button ghostCard3; 
	public Button ghostCard4; 
	public Button ghostCard5;
	public Button ghostCard6;  
	public Button ghostCard7;

	private List<GameObject> clickableGOs; 
	public List<object> clickablebuttonToObject;  

    public static string punchedBandit; 

    void Start(){
		setAllNonClickable();

		// /* DUMMY BANDITS FOR TESTING PURPOSES */
		// Bandit b1 = new Bandit("GHOST");
		// Bandit b2 = new Bandit("BELLE");
		// Bandit b3 = new Bandit("CHEYENNE");	

		// ArrayList banditsArr = new ArrayList(); 
		// banditsArr.Add(b1); 
		// banditsArr.Add(b2); 
		// banditsArr.Add(b3); 

		// /* @TEST makeShootPossibilitiesClickable*/
		// buttonToObject.Add(ghost, b1);

		// ArrayList shootArr = new ArrayList(); 
		// shootArr.Add(b1);
		// makeShootPossibilitiesClickable(shootArr);
	
		// /* @OUTPUT now only GHOST is clickable 🎉*/

		// /* @TEST makePunchPossibilitiesClickable */
		// var selectedBanditName = EventSystem.current.currentSelectedGameObject;
        //  if (selectedBanditName != null)
        //      promptPunchTarget.text = "ahh" + selectedBanditName.name;
        //  else
        //      promptPunchTarget.text = "ahh NULLL POINTERR";
		// // promptPunchTarget.text = "ahh" + selectedBanditName;
		// promptPunchTarget.text = "ahh" + selectedBanditName.name;
		 

		// string selectedPunchBandit = makePunchPossibilitiesClickable(shootArr);
		// Debug.Log("YOU PUNCHED " + selectedPunchBandit);
		// promptPunchTarget.text = selectedPunchBandit + "IS PUNCHED";

		// initCards();
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
		EnterGameBoardScene();
    }

 	public void buttonClicked(Button btn){
        promptPunchTarget.text = btn.name + "IS CLICKED"; 
        punchedBandit = btn.name;
        // btn.interactable = false;
    }
	
	/*
	public static void testSerial() {
		ISFSObject obj = SFSObject.NewInstance();
		ExtensionRequest req = new ExtensionRequest("gm.testSerial",obj);
		SFS.Send(req);
		//EnterGameBoardScene();
		exitText.text = ""; 
		clickableGOsText.text = "";
		// init clickables should be called on update
		initClickables();
		exitText.text =""; 
    }
	*/

	/* setAllNonClickable sets all buttons to be non-clickable */
	public void setAllNonClickable(){
		Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
		foreach(Button aBtn in allButtons){
			aBtn.interactable = false; 
		}
	}


	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		doesItWork.text = responseParams.GetUtfString("log");
		Debug.Log("Received log message: "+ responseParams.GetUtfString("log"));
		gm = (GameManager)responseParams.GetClass("gm");
		GameManager.replaceInstance(gm);
		// REASSIGN ALL GAME buttonToObject USING DICTIONARY
		ArrayList banditsArray = gm.bandits;
		foreach (Bandit b in banditsArray) {
            if (b.characterAsString == "CHEYENNE") {
				//buttonToObject[cheyenne] = b;
                trace("Cheyenne added!");
					
            }
			if (b.characterAsString == "BELLE") {
                //buttonToObject[belle] = b;
                trace("Belle added!");
            }
			if (b.characterAsString == "TUCO") {
                buttonToObject[tuco] = b;
                trace("Tuco added!");
            }
			if (b.characterAsString == "DOC") {
                buttonToObject[doc] = b;
                trace("Doc added!");
            }
			if (b.characterAsString == "GHOST") {
                buttonToObject[ghost] = b;
                trace("Ghost added!");
				// map ghost's hand 
				ArrayList currCards = b.hand; 
				List<Button> ghoButtons = new List<Button>(){ghostCard1, ghostCard2, ghostCard3, ghostCard4, ghostCard5, ghostCard5, ghostCard6, ghostCard7};
				int index = 0; 
				foreach(Card c in currCards){
					 buttonToObject.Add(ghoButtons[index], c); 
				}
            }
			if (b.characterAsString == "DJANGO") {
                buttonToObject[django] = b;
                trace("Django added!");
            }
		}
		Debug.Log(SFS.step);
		// announcement.text = logMessages[SFS.step];

		// map the 13 neutral bullet cards
		// ArrayList neuturalBulletCards = gm.neutralBulletCard; 
		// for(int i=1; i<14; i++){
		// 	Button goBulletCard = goNeutralBulletCards[i];
		// 	buttonToObject[goBulletCard] = neuturalBulletCards[i];
		// }

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

		gm.playTurn();

    }


	public void mapActionCards(List<Button> goHand, string actionName, Card c, string banditName){
		foreach(Button g in goHand){
			string goName = banditName + g.name;
			if(actionName == goName.ToUpper()){
            	buttonToObject[g] = c;
			}
		}
	}

	public void mapBulletCards(string bName, int buSize, BulletCard bc){
		if(bName == "BELLE"){
			buttonToObject[goBELLEBulletCards[buSize]] = bc;
		}else if(bName == "CHEYENNE"){
			buttonToObject[goCHEYENNEBulletCards[buSize]] = bc;
		}else if(bName == "DOC"){
			buttonToObject[goDOCBulletCards[buSize]] = bc;
		}else if(bName == "TUCO"){
			buttonToObject[goTUCOBulletCards[buSize]] = bc;
		}else if(bName == "DJANGO"){
			buttonToObject[goDJANGOBulletCards[buSize]] = bc;
		}else if(bName == "GHOST"){
			buttonToObject[goGHOSTBulletCards[buSize]] = bc;
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
			if(gm != null && gm.currentBandit.getCharacter() == ChooseCharacter.character) {
				Debug.Log("ending my turn");
				// Bandit b = (Bandit) gm.bandits[0];
				// if (b.getCharacter() == gm.currentBandit.getCharacter()) {
				// 	gm.currentBandit = (Bandit) gm.bandits[1];
				// } else {
				// 	gm.currentBandit = (Bandit) gm.bandits[0];
				// }
				gm.endOfTurn();
				//SendNewGameState();
			}
		}

		if(works) {
			doesItWork.text = "it works!";
		} else {
			doesItWork.text = "";
		}

		/*if (SFS.debugText != debugText.text) {
            debugText.text = SFS.debugText;
        }
        clickableGOsText.text += "==== NOW GHOST IS SET TO NONACTIVE ===";
        // ghost.SetActive(false);
        foreach(GameObject go in allObjects){
            if(go.activeSelf == true){
                clickableGOsText.text += go.name;
            }
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
        Debug.Log("Sent enter scene message");
	}

	public static void SendNewGameState(string message) {
		ISFSObject obj = SFSObject.NewInstance();
		//Debug.Log("sending new game state");
		obj.PutClass("gm", gm);
		obj.PutUtfString("log", message);
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
}