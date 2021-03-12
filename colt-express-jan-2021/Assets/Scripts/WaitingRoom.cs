using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Linq;
using TMPro;

public class WaitingRoom : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.90.26.131:4242");

    public Text fToken;
    public Button NewGameButton;
    public Button GoToGameButton;
    public Button LaunchGameButton;

    private static string gameHash = null;
    private static string token;
    private static string username;

    public Text joinText;

    private static bool joined = false;
    private static bool hosting = false;

    private static Dictionary<Button, string> hashes = new Dictionary<Button, string>();

    // Start is called before the first frame update
    void Start()
    {
        token = PlayerPrefs.GetString("token", "No token found");
        username = PlayerPrefs.GetString("username", "No username found");

        NewGameButton.interactable = false;
        GoToGameButton.interactable = false;
        LaunchGameButton.interactable = false;
        // fToken.text = waitToken;
        GetSessions();
    }

    // Update is called once per frame
    void Update()
    {
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
        GetSessions();
        if(hosting && !LaunchGameButton.interactable) {
            ActivateLaunchGameButton();
        }
        ActivateGoToGameButton();
    }

    public void ActivateGoToGameButton() {
        if (gameHash != null && joined && !GoToGameButton.interactable) {
            var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            var obj = JObject.Parse(response.Content);
            Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();

            if (sessionDetails["launched"].ToString().ToLower() == "true") {
                GoToGameButton.interactable = true;
            }
            
        }
    }

    public void ActivateLaunchGameButton() {
        var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();
        int numPlayers = 1 + sessionDetails["players"].ToString().ToCharArray().Count(c => c == ',');

        if (numPlayers >= 2) { //change to >2
            LaunchGameButton.interactable = true;
        }
    }

    public void JoinGame(Button gameToJoin)
    {
        // replace this with the id present in the text of the clicked button
        gameHash = hashes[gameToJoin];
        var request = new RestRequest("api/sessions/" + gameHash + "/players/" + username + "?access_token=" + token, Method.PUT)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        gameToJoin.interactable = false;
        joined = true;
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
        hosting = true;
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
                NewGameButton.onClick.AddListener(delegate{JoinGame(NewGameButton);});
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
		// Always disconnect before quitting
		SFS.Disconnect();
	}
}