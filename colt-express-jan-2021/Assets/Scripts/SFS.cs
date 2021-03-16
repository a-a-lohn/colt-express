using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//using System.Windows;

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
    public static string username; //should be set to logged in user's name
	public static bool enteredGame = false;

	public static string chosenCharText = "";

	public static int step = 0;

	public static GameBoard gb;
	public static ChooseCharacter cc;

    static SFS(){
        defaultHost = "127.0.0.1";//"13.72.79.112";
	    defaultTcpPort = 9933;
        zone = "MergedExt";
    }

    public static void setSFS(SmartFox Sfs) {
        sfs = Sfs;
    }

	public static SmartFox getSFS() {
        return sfs;
    }

	public static void setGameBoard() {
		gb = GameObject.Find("GameBoardGO").GetComponent<GameBoard>();
	}

	public static void setChooseCharacter() {
		cc = GameObject.Find("ChooseCharacterGO").GetComponent<ChooseCharacter>();
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
        trace("response received: " + cmd); // shpows up after "in-class" debug message
		if (cmd == "remainingCharacters") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			string player = responseParams.GetUtfString("player");
			if(player != null) {
				string chosen = responseParams.GetUtfString("chosenCharacter");
				chosenCharText += "\n" + player + " chose " + chosen + "!";
				cc.UpdateDisplayText(chosenCharText);
			}
			cc.DisplayRemainingCharacters(evt);
		} else if (cmd == "updateGameState") {
            gb.UpdateGameState(evt);
        } else if (cmd == "nextAction") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			step = responseParams.GetInt("step");
			Debug.Log("received step " + step);
			gb.executeHardCoded(step);
		}
    }

    public static void Connect(string uname) {
		if (sfs == null || !sfs.IsConnected) {
			
			username = uname;
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

			//sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
			sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
			//sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
			//sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
			sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
			//sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
			
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

		//sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
		//sfs.RemoveEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		//sfs.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		//sfs.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
		
		sfs = null;

		//clearRoomList();
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

    /*public static void OnApplicationQuit() {
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
		/*User user = (User) evt.Params["user"];

		// Show system message
		string msg = "Login successful!\n";
		msg += "Logged in as " + user.Name;
		trace(msg);

		// Populate Room list
		populateRoomList(sfs.RoomList);*/

		// Join first Room in Zone
		if (sfs.RoomList.Count > 0) {
			sfs.Send(new Sfs2X.Requests.JoinRoomRequest(sfs.RoomList[0].Name));
		}
	}

	public static void LeaveRoom() {
		sfs.Send(new Sfs2X.Requests.LeaveRoomRequest());
	}
	
	private static void OnLoginError(BaseEvent evt) {
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		reset();
		
		// Show error message
		debugText = "Login failed: " + (string) evt.Params["errorMessage"];
	}


	/*private static void populateRoomList(List<Room> rooms) {
		// Clear current Room list
		clearRoomList();

		// For the roomlist we use a scrollable area containing a separate prefab button for each Room
		// Buttons are clickable to join Rooms
		foreach (Room room in rooms) {
			int roomId = room.Id;

			GameObject newListItem = Instantiate(roomListItem) as GameObject;
			RoomItem roomItem = newListItem.GetComponent<RoomItem>();
			roomItem.nameLabel.text = room.Name;
			roomItem.maxUsersLabel.text = "[max " + room.MaxUsers + " users]";
			roomItem.roomId = roomId;

			roomItem.button.onClick.AddListener(() => OnRoomItemClick(roomId));

			newListItem.transform.SetParent(roomListContent, false);
		}
	}

	private static void clearRoomList() {
		foreach (Transform child in roomListContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	private static void populateUserList(List<User> users) {
		// For the userlist we use a simple text area, with a user name in each row
		// No interaction is possible in this example

		// Get user names
		List<string> userNames = new List<string>();

		foreach (User user in users) {

			string name = user.Name;

			if (user == sfs.MySelf)
				name += " <color=#808080ff>(you)</color>";

			userNames.Add(name);
		}

		// Sort list
		userNames.Sort();

		// Display list
		userListText.text = "";
		userListText.text = String.Join("\n", userNames.ToArray());
	}


	private static void OnRoomJoin(BaseEvent evt) {
		Room room = (Room) evt.Params["room"];

		// Clear chat (uless this is the first time a Room is joined - or the initial system message would be deleted)
		if (!firstJoin)
			chatText.text = "";

		firstJoin = false;
		
		// Show system message
		printSystemMessage("\nYou joined room '" + room.Name + "'\n");

		// Enable chat controls
		chatControls.interactable = true;

		// Populate users list
		populateUserList(room.UserList);
	}*/
	
	private static void OnRoomJoinError(BaseEvent evt) {
		// Show error message
		Debug.Log("Room join failed: " + (string) evt.Params["errorMessage"]);
	}
	
	/*private static void OnUserEnterRoom(BaseEvent evt) {
		User user = (User) evt.Params["user"];
		Room room = (Room) evt.Params["room"];

		// Show system message
		printSystemMessage("User " + user.Name + " entered the room");

		// Populate users list
		populateUserList(room.UserList);
	}*/
	
	private static void OnUserExitRoom(BaseEvent evt) {
		User user = (User) evt.Params["user"];

		if (user != sfs.MySelf) {
			//Room room = (Room)evt.Params["room"];
			
			// Show system message
			//printSystemMessage("User " + user.Name + " left the room");
			
			// Populate users list
			//populateUserList(room.UserList);
			Debug.Log(user.Name + " left the game!");
			gb.exit.text = user.Name + " left the game! You will now be redirected to the Waiting Room";
			//Invoke("GoToWaitingRoom", 5);
			gb.GoToWaitingRoom();
		} else {
			gb.exit.text = "You will now be redirected to the Waiting Room";
			Debug.Log("Returning to waiting room");
			//Invoke("GoToWaitingRoom", 5);
			gb.GoToWaitingRoom();
		}
	}

	/*private static void OnRoomAdd(BaseEvent evt) {
		// Re-populate Room list
		populateRoomList(sfs.RoomList);
	}*/
}
