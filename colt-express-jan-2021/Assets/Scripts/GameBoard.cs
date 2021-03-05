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

using model;

public class GameBoard : MonoBehaviour
{

    public GameObject cheyenne;
	// 
	public GameObject testingObj; // there're exceptions so these GameObj's don't show up in the scene 
	// 
    public Text debugText;
    public Button button;
	public Button extension;
    public Text buttonLabel;
    public Bandit b;
    
    Dictionary<GameObject, Bandit> bandits = new Dictionary<GameObject, Bandit>();


    private SmartFox sfs;
    private string defaultHost = "127.0.0.1"; //"13.90.26.131"; //"127.0.0.1"; //
	private int defaultTcpPort = 9933;			// Default TCP port
    private string zone = "MergedExt"; //"NewZone"; //"ColtExpress"; //"BasicExamples";// "MyExt";

    // Start is called before the first frame update
    void Start()
    {
		
        Test();

        // Receive classes that were created on server upon game startup here
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sfs != null) {
			sfs.ProcessEvents();
		}


        // updateGameState method goes here and reassigns all game objects in dictionaries to received objects

        // prompts come in here and indicate (e.g. with a log message what user should do/click).
        // The prompt should send all the objects that are clickable
        
        // user clicks on a gameobject, we check that it is valid, i.e. is one of the available clickable objects
        // received from prompt, and if so it is sent to the server

        // have listener methods for different prompts that verifies if game object that user clicks
        // is valid using dictionary
    }

    private void Test() {
        buttonLabel.text = "CONNECT+ENTER";
        button.onClick.AddListener(OnButtonClick);
		extension.onClick.AddListener(ChooseCharacter);
    }

	private void EnterChooseCharacterScene() {
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterChooseCharacterScene",obj);
        sfs.Send(req);
        trace("Sent enter scene message");
	}


	// client side: receiving input from USER (for testing purposes it's called automatically upon login)
    private void ChooseCharacter() {

        ISFSObject obj = SFSObject.NewInstance();
		obj.PutUtfString("character", "TUCO");
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        sfs.Send(req);
        trace("chose Tuco");
    }


	// client side: receiving feedback from SERVER
    private void OnExtensionResponse(BaseEvent evt) {
        String cmd = (String)evt.Params["cmd"];
        trace("response received");
        /*
        * DELEGATE COMMANDS TO DIFFERENT METHODS HERE, BASED ON VALUE OF cmd
        */
		if (cmd == "updateGameState") {
            UpdateGameState(evt);
		} else if (cmd == "remainingCharacters") {
			DisplayRemainingCharacters(evt);
		}
    }

	private void DisplayRemainingCharacters(BaseEvent evt) {
		ISFSObject responseParams = (SFSObject)evt.Params["params"];
        String resp = (string)responseParams.GetSFSArray("characterList").GetUtfString(0);
		//string resp2 = responseParams.GetUtfString("cl");
		int size = responseParams.GetSFSArray("characterList").Size();
		trace("Characters to choose from: " + size + resp);
		/*foreach (var s in resp) {
			trace(s.ToString());
		}*/
	}

    private void  UpdateGameState(BaseEvent evt) {
        trace("updategamestate called");
        // REASSIGN ALL GAME OBJECTS -- CLEAR THEM FIRST
        bandits = new Dictionary<GameObject, Bandit>();
        //...

        ISFSObject responseParams = (SFSObject)evt.Params["params"];
        string resp = responseParams.GetUtfString("testStr");
        trace(resp);
		//Bandit b = (Bandit)responseParams.GetClass("bandits");
		//trace(b.strBanditName);
        // Extract expected parameters and reassign all game objects
        /*ArrayList banditsArray = (ArrayList)responseParams.GetClass("bandits");
        foreach (Bandit b in banditsArray) {
            if (b.strBanditName == "CHEYENNE") {
                bandits.Add(cheyenne, b);
                trace("Cheyenne added!");
            }
        }*/

		GameManager gm = (GameManager)responseParams.GetClass("gm");
		Bandit b = (Bandit)gm.bandits[0];
		trace(b.strBanditName + " YESSSS");
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
			sfs.Send(new Sfs2X.Requests.LoginRequest("coltplayer"));

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

		EnterChooseCharacterScene();

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