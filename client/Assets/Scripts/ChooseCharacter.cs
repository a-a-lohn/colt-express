using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using RestSharp;
using Newtonsoft.Json.Linq;
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

using model;

public class ChooseCharacter : MonoBehaviour
{

    public Text display;
    public Text info;

    private static bool alreadyCalled;

    private static RestClient client = new RestClient("http://127.0.0.1:4242");

    //SAVE THE CHOSEN CHARACTER IN THIS STRING SO IT CAN BE USED BY GAMEMANAGER
    public static string character;

    // BOOLEANS FOR CHARACTER AVAILABILITY
    public bool BelleIsAvailable; 
    public bool CheyenneIsAvailable; 
    public bool TucoIsAvailable; 
    public bool DjangoIsAvailable; 
    public bool DocIsAvailable; 
    public bool GhostIsAvailable;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(800, 600, true);

        character = "";
        alreadyCalled = false;

        info.text = "You will be brought to the game once all " + WaitingRoom.numPlayers + " players have chosen a character!";
        display.text = "";

        BelleIsAvailable = true;
        CheyenneIsAvailable = true; 
        TucoIsAvailable = true; 
        DjangoIsAvailable = true; 
        DocIsAvailable = true; 

        SFS.setChooseCharacter();

    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected() && !alreadyCalled) {
            alreadyCalled = true;
            EnterChooseCharacterScene();
        }
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

    }

    public static void trace(string msg) {
		//debugText += (debugText != "" ? "\n" : "") + msg;
	}

	public void EnterChooseCharacterScene() {
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterChooseCharacterScene",obj);
        SFS.Send(req);
        trace("Sent enter scene message");
	}


    public void CharacterChoice(Button b) {
        character = b.name.ToUpper();
        SetAllNotInteractable();
        ISFSObject obj = SFSObject.NewInstance();
		obj.PutUtfString("chosenCharacter", character);
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        SFS.Send(req);
    }

    public void UpdateDisplayText(string ut) {
        display.text += ut;
        SFS.chosenCharText = "";
    }

	public void DisplayRemainingCharacters(BaseEvent evt) {
		ISFSObject responseParams = (SFSObject)evt.Params["params"];
        try {
            ISFSArray a = responseParams.GetSFSArray("characterList");
            int size = responseParams.GetSFSArray("characterList").Size();
            // loop through all the buttons
            // if a character's name is in the input list -> active the button // otherwise deactive the btn 
            SetAllNotInteractable();
            if(character == "") { // only let buttons be interactable if you haven't chosen yet
                var foundButtonObjects = FindObjectsOfType<Button>();
                for (int i = 0; i < size; i++) {
                    foreach(Button btn in foundButtonObjects){
                        string banditName = (string)a.GetUtfString(i);
                        if(btn.name == "Tuco" && banditName == "TUCO"){; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Belle" && banditName == "BELLE"){
                            btn.interactable = true; 
                        }
                        if(btn.name == "Cheyenne" && banditName == "CHEYENNE"){
                            btn.interactable = true; 
                        }
                        if(btn.name == "Django" && banditName == "DJANGO"){
                            btn.interactable = true; 
                        }
                        if(btn.name == "Ghost" && banditName == "GHOST"){
                            btn.interactable = true; 
                        }
                        if(btn.name == "Doc" && banditName == "DOC"){
                            btn.interactable = true; 
                        }
                    }
                }
            }
        } catch (Exception e) {
            Invoke("NextScene",3);
        }
	}

    private void SetAllNotInteractable() {
        var foundButtonObjects = FindObjectsOfType<Button>();
        foreach(Button btn in foundButtonObjects){
            if(btn.name == "Tuco"){
                btn.interactable = false; 
            }
            if(btn.name == "Belle"){
                btn.interactable = false; 
            }
            if(btn.name == "Cheyenne"){
                btn.interactable = false;
            }
            if(btn.name == "Django"){
                btn.interactable = false; 
            }
            if(btn.name == "Ghost"){
                btn.interactable = false; 
            }
            if(btn.name == "Doc"){
                btn.interactable = false; 
            }
        }
    }

    private void NextScene() {
        SFS.enteredGame = true;
        SceneManager.LoadScene("GameBoard");
    }

    public static void RemoveLaunchedSession() {
        Debug.Log("removing session?");
        if (true/*WaitingRoom.hosting*/) {
            Debug.Log("removing session!");
            Debug.Log("hash: " + WaitingRoom.gameHash);
            var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            
            var obj = JObject.Parse(response.Content);
            string adminToken = (string)obj["access_token"];
            adminToken = adminToken.Replace("+", "%2B");

            var request2 = new RestRequest("api/sessions/" + WaitingRoom.gameHash + "?access_token=" + adminToken, Method.DELETE)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response2 = client.Execute(request2);
        }
    }

    void OnApplicationQuit() {
        RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}