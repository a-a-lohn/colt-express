using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
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

public class SFS : MonoBehaviour
{

    public Text debugText;
    public Text buttonLabel;
	public InputField uname;
    public InputField n1;
    public InputField n2;
    public Text result;
	public Button add;
	public Button serial;
    public Button button;
    private string defaultHost = "127.0.0.1";//13.90.26.131";//"127.0.0.1"; 	//
	private int defaultTcpPort = 9933;			// Default TCP port
	
	private SmartFox sfs;

    // Start is called before the first frame update
    void Start()
    {
        debugText.text = defaultTcpPort.ToString();
        button.onClick.AddListener(OnButtonClick);
		//changed this from Addition so we can test output text from serialization
		add.onClick.AddListener(Addition);
		//serial.onClick.AddListener(TestSerial);
    }

    // Update is called once per frame
    void Update()
    {
        // As Unity is not thread safe, we process the queued up callbacks on every frame
		if (sfs != null) {
			sfs.ProcessEvents();
		}
    }

    void OnApplicationQuit() {
		// Always disconnect before quitting
		if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
	}

    public void OnButtonClick() {
		if (sfs == null || !sfs.IsConnected) {

			// CONNECT

			// Enable interface
			enableInterface(false);
			
			// Clear console
			debugText.text = "";
			//debugScrollRect.verticalNormalizedPosition = 1;
			
			trace("Now connecting...");
			
			// Initialize SFS2X client and add listeners
			sfs = new SmartFox();
			DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            trace("still...");
			
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
			cfg.Zone = "MyExt";// "BasicExamples";//"ColtExpress";
			//cfg.Debug = true;
				
			// Connect to SFS2X
			sfs.Connect(cfg);
		} else {
            trace("not");
			// DISCONNECT

			// Disable button
			button.interactable = false;
			
			// Disconnect from SFS2X
			sfs.Disconnect();
		}
	}

    //----------------------------------------------------------
	// Private helper methods
	//----------------------------------------------------------
	
	private void enableInterface(bool enable) {
		//hostInput.interactable = enable;
		//portInput.interactable = enable;
		//debugToggle.interactable = enable;

		button.interactable = enable;
		buttonLabel.text = "CONNECT";
	}
	
	private void trace(string msg) {
		debugText.text += (debugText.text != "" ? "\n" : "") + msg;
		//Canvas.ForceUpdateCanvases();
		//debugScrollRect.verticalNormalizedPosition = 0;
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
		
		// Enable interface
		enableInterface(true);
	}

	//----------------------------------------------------------
	// SmartFoxServer event listeners
	//----------------------------------------------------------
	
	private void OnConnection(BaseEvent evt) {
		if ((bool)evt.Params["success"]) {
			trace("Connection established successfully");
			trace("SFS2X API version: " + sfs.Version);
			trace("Connection mode is: " + sfs.ConnectionMode);
			
			// Enable disconnect button
			button.interactable = true;
			buttonLabel.text = "DISCONNECT";

            // Login with given username after having made connection
			sfs.Send(new Sfs2X.Requests.LoginRequest(uname.text));
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


    //** LOGIN STUFF **//


    private void OnLogin(BaseEvent evt) {
		User user = (User) evt.Params["user"];

		// Show system message
		string msg = "Login successful!\n";
		msg += "Logged in as " + user.Name;
		trace(msg);
        // add once login succeeds
	}
	
	private void OnLoginError(BaseEvent evt) {
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		reset();
		
		// Show error message
		debugText.text = "Login failed: " + (string) evt.Params["errorMessage"];
	}


	Bandit b;

	// client side: receiving input from user
    private void Addition() {
        ISFSObject obj = SFSObject.NewInstance();
		
        obj.PutInt("n1",Convert.ToInt32(n1.text));
        obj.PutInt("n2",Convert.ToInt32(n2.text));

		if(Convert.ToInt32(n1.text) == 8 && b != null) {
			b.num = 555;
			debugText.text = b.num.ToString();
			obj.PutClass("sent", b);
		}
		
        ExtensionRequest req = new ExtensionRequest("math",obj);
        sfs.Send(req);
    }

	/*private void TestSerial(){
		ExtensionRequest req = new ExtensionRequest("serial", obj);
		sfs.Send(req);
	}*/

	// client side: receiving feedback from server
    private void OnExtensionResponse(BaseEvent evt) {
        String cmd = (String)evt.Params["cmd"];
		if (cmd == "math") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];

			// We expect an int parameter called "sum", as labelled in the server
			result.text = responseParams.GetInt("sum").ToString();
			b = (Bandit)responseParams.GetClass("b");
			if (b != null) {
				ActionCard card = (ActionCard)b.hand[0];
				int numb = card.actionnum;
				result.text = result.text + "\n num: " + Convert.ToInt32(b.num) + "\n Char: " + b.strBanditName + "\n actionnum: " + Convert.ToInt32(numb);
			}
		}
    }

}