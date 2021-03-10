using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public static class SFS
{
    public static SmartFox sfs;
    public static string defaultHost; 
	public static int defaultTcpPort;
    public static string zone;
    public static string debugText = "";
    public static bool moreText = false;
    public static string username = "coltplayer2"; //should be set to logged in user's name

    static SFS(){
        defaultHost = "127.0.0.1";  //"13.90.26.131";
	    defaultTcpPort = 9933;
        zone = "MergedExt";
    }

    public static void setSFS(SmartFox Sfs) {
        sfs = Sfs;
    }

    public static void trace(string msg) {
		//debugText += (debugText != "" ? "\n" : "") + msg;
        debugText = msg;
        moreText = true;
	}

    
    public static void ProcessEvents() {
        sfs.ProcessEvents();
    }

    public static void Send(ExtensionRequest req) {
        sfs.Send(req);
    }

    // client side: receiving feedback from SERVER
    private static void OnExtensionResponse(BaseEvent evt) {
        String cmd = (String)evt.Params["cmd"];
        trace("response received: " + cmd);
		if (cmd == "remainingCharacters") {
			ChooseCharacter.DisplayRemainingCharacters(evt);
		} else if (cmd == "updateGameState") {
            // GameBoard.UpdateGameState(evt);
        }
    }

    public static void Connect() {
		if (sfs == null || !sfs.IsConnected) {

			// CONNECT

			// Clear console
			//debugText.text = "";
			
			trace("Now connecting...");	
			
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
		}
	}

    public static bool IsConnected() {
        if (sfs != null && sfs.IsConnected) {
            return true;
        }
        return false;
    }


    private static void reset() {
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

    private static void OnConnection(BaseEvent evt) {
		if ((bool)evt.Params["success"]) {
			trace("Connection established successfully");
			trace("Connection mode is: " + sfs.ConnectionMode);

            // Login with some username after having made connection
			sfs.Send(new Sfs2X.Requests.LoginRequest(username));

		} else {
			trace("Connection failed; is the server running at all?");
			
			// Remove SFS2X listeners and re-enable interface
			reset();
		}
	}
	
	private static void OnConnectionLost(BaseEvent evt) {
		trace("Connection was lost; reason is: " + (string)evt.Params["reason"]);
		
		// Remove SFS2X listeners and re-enable interface
		reset();
	}
	
	//----------------------------------------------------------
	// SmartFoxServer log event listeners
	//----------------------------------------------------------
	
	public static void OnInfoMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}
	
	public static void OnWarnMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}
	
	public static void OnErrorMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}
	
	private static void ShowLogMessage(string level, string message) {
		message = "[SFS > " + level + "] " + message;
		trace(message);
		Debug.Log(message);
	}

    /*void OnApplicationQuit() {
		// Always disconnect before quitting
		if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
	}*/

    public static void Disconnect() {
        if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
    }

    //** LOGIN STUFF **//


    private static void OnLogin(BaseEvent evt) {
		User user = (User) evt.Params["user"];

		// Show system message
		string msg = "Login successful!\n";
		msg += "Logged in as " + user.Name;
		trace(msg);

	}
	
	private static void OnLoginError(BaseEvent evt) {
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		reset();
		
		// Show error message
		debugText = "Login failed: " + (string) evt.Params["errorMessage"];
	}
}
