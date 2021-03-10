using model;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
	public Text debugText;
	public static string debugTextString;
    public Button button;
	public Button extension;
	public Button chooseChar;
    public Text buttonLabel;
	
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

	// public GameObject tuco;
	// public GameObject doc; 
	// public GameObject django; 
    
    // ?? 
	// <T> not found // needs to be declared before using 
    public Dictionary<GameObject, object> objects = new Dictionary<GameObject, object>();

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
		debugTextString = "";
        debugText.text = "";

		//SendNewGameState();
		// ** THE DICTIONARIES ARE INITIALIZED(CLEARED) IN Start() ** 
		objects.Add(cheyenne, null);
		objects.Add(belle, null);
		objects.Add(tuco, null);
		objects.Add(doc, null);
		objects.Add(ghost, null);
		objects.Add(django, null);

		objects.Add(gem1, null);
		objects.Add(gem2, null);
		objects.Add(gem3, null);
		objects.Add(gem4, null);
		objects.Add(gem5, null);


    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		if (SFS.debugText != debugText.text) {
            debugText.text = SFS.debugText;
        }

		// for debugging
		if (SFS.moreText) {
            debugTextString += SFS.debugText;
            SFS.moreText = false;
        }
        if (debugTextString != debugText.text) {
            debugText.text = debugTextString;
        }
    }

	public static void SendNewGameState() {
		ISFSObject obj = SFSObject.NewInstance();
		
		/* testing purposes
		gm = new GameManager();
		ArrayList bandits = new ArrayList();
		Bandit doc = new Bandit();
		TrainUnit position = new TrainUnit();
		position.carTypeAsString = "LocomotiveRoof";
		doc.position = position;
		bandits.Add(doc);
		gm.bandits = bandits;
		*/

		obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.newGameState",obj);
        SFS.Send(req);
        trace("sent game state");
	}

	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public static void UpdateGameState(BaseEvent evt) {
        trace("updategamestate called");
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");

		// get the first Bandit obj from GM 
		// Bandit b = (Bandit) gm.bandits[0];
		
		// ** REASSIGN ALL GAME OBJECTS USING DICTIONARY -- CLEAR THEM FIRST ** 
		// all objects need identity
        // objects[cheyenne] = gm.ban...
		// objects[cheyenne] = gm.
		// ** Assign the game object cheyenne to the Bandit cheyenne ** 
		Bandit banditChey = (Bandit) gm.bandits[0]; 
		Bandit banditBelle = (Bandit) gm.bandits[1];
		Bandit banditTuco = (Bandit) gm.bandits[2];
		Bandit banditDoc = (Bandit) gm.bandits[3];
		Bandit banditGhost = (Bandit) gm.bandits[4];
		Bandit banditDjango = (Bandit) gm.bandits[5];

		// ... 
		objects[cheyenne] = gm.bandits[0]; 
		objects[belle] = gm.bandits[1]; 
		objects[tuco] = gm.bandits[2]; 
		objects[doc] = gm.bandits[3]; 
		objects[ghost] = gm.bandits[4]; 
		objects[django] = gm.bandits[4]; 

		// ** adding to the dictionary ** 
		objects.Add(cheyenne, banditChey); 
		objects.Add(belle, banditBelle);
		objects.Add(tuco, banditTuco);
		objects.Add(doc, banditDoc);
		objects.Add(ghost, banditGhost);
		objects.Add(django, banditDjango);

		gm.PlayTurn();

		//trace(b.strBanditName);
        // Extract expected parameters and reassign all game objects
        /*ArrayList banditsArray = (ArrayList)responseParams.GetClass("bandits");*/
        ArrayList banditsArray = (ArrayList)responseParams.GetClass("bandits");
		foreach (Bandit b in banditsArray) {
            if (b.strBanditName == "CHEYENNE") {
                bandits.Add(cheyenne, b);
                trace("Cheyenne added!");
            }
        }*/

		GameManager gm = (GameManager)responseParams.GetClass("gm");
		Bandit b = (Bandit)gm.bandits[0];
		trace(b.banditNameAsString + " YESSSS");
    }








    private void trace(string msg) {
		debugText.text += (debugText.text != "" ? "\n" : "") + msg;
	}

    public void OnButtonClick() {
		if (sfs == null || !sfs.IsConnected) {

			// CONNECT

			// Clear console
			debugText.text = "";
			
			trace("Now connecting...");
			
			// Initialize SFS2X client
			sfs = new SmartFox();

            // For C# serialization
			DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
			
            // Add listeners
			sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		    sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

			sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
			sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);
			sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);
			
			// Set connection parameters
			ConfigData cfg = new ConfigData();
			cfg.Host = defaultHost;
			cfg.Port = Convert.ToInt32(defaultTcpPort.ToString());
			cfg.Zone = zone;
			//cfg.Debug = true;
				
			// Connect to SFS2X
			sfs.Connect(cfg);
		} else {
			
			// Disconnect from SFS2X
			sfs.Disconnect();
            trace("Disconnected");
            buttonLabel.text = "CONNECT+ENTER";
		}
	}

    private void reset() {
		// Remove SFS2X listeners
		sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfs.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

		sfs.RemoveLogListener(LogLevel.INFO, OnInfoMessage);
		sfs.RemoveLogListener(LogLevel.WARN, OnWarnMessage);
		sfs.RemoveLogListener(LogLevel.ERROR, OnErrorMessage);
		
		sfs = null;
	}

    private void OnConnection(BaseEvent evt) {
		if ((bool)evt.Params["success"]) {
			trace("Connection established successfully");
			trace("Connection mode is: " + sfs.ConnectionMode);

            // Login with some username after having made connection
			sfs.Send(new Sfs2X.Requests.LoginRequest("coltplayer2"));

            buttonLabel.text = "DISCONNECT";
		} else {
			trace("Connection failed; is the server running at all?");
			
			// Remove SFS2X listeners and re-enable interface
			reset();
		}
	}
	
	private void OnConnectionLost(BaseEvent evt) {
		trace("Connection was lost; reason is: " + (string)evt.Params["reason"]);
		
		// Remove SFS2X listeners and re-enable interface
		reset();
	}
	
	//----------------------------------------------------------
	// SmartFoxServer log event listeners
	//----------------------------------------------------------
	
	public void OnInfoMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}
	
	public void OnWarnMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}
	
	public void OnErrorMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}
	
	private void ShowLogMessage(string level, string message) {
		message = "[SFS > " + level + "] " + message;
		trace(message);
		Debug.Log(message);
	}

    void OnApplicationQuit() {
		// Always disconnect before quitting
		if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
	}

    //** LOGIN STUFF **//


    private void OnLogin(BaseEvent evt) {
		User user = (User) evt.Params["user"];

		// Show system message
		string msg = "Login successful!\n";
		msg += "Logged in as " + user.Name;
		trace(msg);

		//EnterChooseCharacterScene();

	}
	
	private void OnLoginError(BaseEvent evt) {
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		reset();
		
		// Show error message
		debugText.text = "Login failed: " + (string) evt.Params["errorMessage"];
	}


}
			if (b.strBanditName == "BELLE") {
                bandits.Add(belle, b);
                trace("Belle added!");
            }
			if (b.strBanditName == "TUCO") {
                bandits.Add(tuco, b);
                trace("Tuco added!");
            }
			if (b.strBanditName == "DOC") {
                bandits.Add(doc, b);
                trace("Doc added!");
            }
			if (b.strBanditName == "GHOST") {
                bandits.Add(ghost, b);
                trace("Ghost added!");
            }
			if (b.strBanditName == "DJANGO") {
                bandits.Add(django, b);
                trace("Django added!");
            }
        }
    }

	private void ChooseCharacter(Character c) {
		// **Assumption: Character has a function(getName) that returns the name of the character**
        ISFSObject obj = SFSObject.NewInstance();
		string cName = c.getName; 
		// obj.PutUtfString("chosenCharacter", "TUCO");
		obj.PutUtfString("chosenCharacter", cName);
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        SFS.Send(req);
        trace("chose"+cName);
    }


	public static void trace(string msg) {
		debugTextString += (debugTextString != "" ? "\n" : "") + msg;
	}

	void OnApplicationQuit() {
		// Always disconnect before quitting
		SFS.Disconnect();
	}

   /* private void Test() {
        buttonLabel.text = "CONNECT+ENTER";
        button.onClick.AddListener(OnButtonClick);
		extension.onClick.AddListener(SendNewGameState);//EnterChooseCharacterScene);
		chooseChar.onClick.AddListener(ChooseCharacter);
    }


    private void trace(string msg) {
		debugText.text += (debugText.text != "" ? "\n" : "") + msg;
	}*/


}
