using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
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

public class WaitingRoom : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.72.79.112:4242");

    public Text fToken;
    public Button NewGameButton;
    //public Button GoToGameButton;
    public Button LaunchGameButton;
    public Button HostGameButton;
    public Text InfoText;
    public Text launchText;


    public static string gameHash = null;
    private static string token;
    private static string username;

    private static bool joined = false;
    public static bool hosting = false;
    public static int numPlayers;

    private static Dictionary<Button, string> hashes = new Dictionary<Button, string>();

    // Start is called before the first frame update
    void Start()
    {
        token = PlayerPrefs.GetString("token", "No token found");
        username = PlayerPrefs.GetString("username", "No username found");
        
        if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
            SFS.Connect(username);
        }

        NewGameButton.interactable = false;
        //GoToGameButton.interactable = false;
        LaunchGameButton.interactable = false;
        // fToken.text = waitToken;
        GetSessions();

    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

        /*if (SessionCreated())
        {
            NewGameButton.SetActive(true);
            if (NewGameButton.activeSelf)
            {
                if (LaunchStatus())
                {
                    fToken.text = "A new game has been launched";
                }
            }
        }*/
        //Invoke("GetSessions", 1);
        GetSessions();
        if(hosting && !LaunchGameButton.interactable) {
            ActivateLaunchGameButton();
        }
        
        GoToGame();
    }

    public void GoToGame() {
        if (gameHash != null && joined)/* && !GoToGameButton.interactable) */{
            var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            var obj = JObject.Parse(response.Content);
            Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();

            if (sessionDetails["launched"].ToString().ToLower() == "true") {
               numPlayers = 1 + sessionDetails["players"].ToString().ToCharArray().Count(c => c == ',');
               SceneManager.LoadScene("ChooseYourCharacter");
            }
            
        }
    }

    public void ActivateLaunchGameButton() {
        var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();
        numPlayers = 1 + sessionDetails["players"].ToString().ToCharArray().Count(c => c == ',');

        if (numPlayers >= 2) { //change to >2
            LaunchGameButton.interactable = true;
        }
    }

    public void JoinGame(Button gameToJoin)
    {
        gameHash = hashes[gameToJoin];
        var request = new RestRequest("api/sessions/" + gameHash + "/players/" + username + "?access_token=" + token, Method.PUT)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        foreach(KeyValuePair<Button, string> entry in hashes){
            entry.Key.interactable = false;
        } 
        LaunchGameButton.interactable = false;
        HostGameButton.interactable = false;
        joined = true;
        InfoText.text = "You will be brought to the next scene once the host launches the game!";
    }

    /*private static string ExtractHash()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        string content = response.Content;
        int index = content.IndexOf('"', 15);
        return content.Substring(14, index - 14);
    }*/

    public void CreateSession()
    {
        dynamic j = new JObject();
        j.creator = username;
        j.game = "ColtExpress";
        j.savegame = "";

        var request = new RestRequest("api/sessions?access_token=" + token, Method.POST)
            //.AddParameter("access_token", token)
            .AddParameter("application/json", j.ToString(), ParameterType.RequestBody)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        gameHash = response.Content;
        HostGameButton.interactable = false;
        hosting = true;
        InfoText.text = "You will be brought to the next scene once you launch your game!";
    }

    private void GetSessions()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);

        var obj = JObject.Parse(response.Content);
        Dictionary<string, Dictionary<string, Dictionary<string, object>>> sessions = obj.ToObject<Dictionary<string, Dictionary<string, Dictionary<string, object>>>>();
        //Dictionary<string, Dictionary<string, Dictionary<string, string>>> sessionsPlayers = obj.ToObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();

        foreach(KeyValuePair<string, Dictionary<string, object>> entry in sessions["sessions"])
        {
            // create a gamebutton for each game here

            if (!hashes.ContainsKey(NewGameButton)) {
                hashes.Add(NewGameButton, entry.Key);
                if(!hosting) {
                    NewGameButton.interactable = true;
                }
                //NewGameButton.onClick.AddListener(JoinGame);//delegate{JoinGame(NewGameButton);});
            }
            
            fToken.text = "\nSession ID: " + entry.Key;
            fToken.text += "\nCreated by: " + entry.Value["creator"].ToString();
            string players = entry.Value["players"].ToString();
            var charsToRemove = new string[] { "[", "]", "\n", "\"", " "};
            foreach (var c in charsToRemove)
            {
                players = players.Replace(c, string.Empty);
            }
            fToken.text += "\nCurrrent participants: " + players;
            fToken.text += "\nLaunch status: " + entry.Value["launched"].ToString();


            

            // for now, we only have one button, so break after first session
            break;
            
        }

    }

    /*private static Boolean SessionCreated()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        string content = response.Content;

        if (content.Contains("sessions\":{}"))
        {
            return false;
        }

        return true;
    }*/

    /*public void IntentToJoinSession() {
        if(intentToJoin) {
            intentToJoin = false;
            joinText.text = "Don't join";
        } else {
            intentToJoin = true;
            joinText.text = "Join a game";
        }
    }*/

    public void LaunchSession()
    {
        var request = new RestRequest("api/sessions/" + gameHash + "?access_token=" + token, Method.POST)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
        joined = true;
        launchText.text = "Your game is launching!";
    }

    /*private static Boolean LaunchStatus()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        var status = (string)obj["sessions"][ExtractHash()]["launched"];
        if (status.ToLower().Equals("true"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    void OnApplicationQuit() {
        ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}