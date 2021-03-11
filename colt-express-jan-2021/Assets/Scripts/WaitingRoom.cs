using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using RestSharp;
using Newtonsoft.Json.Linq;

public class WaitingRoom : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.90.26.131:4242");

    public Text fToken;
    public GameObject NewGameButton;
    private static string hash;
    private static string token;
    private static string username;

    // Start is called before the first frame update
    void Start()
    {
        token = PlayerPrefs.GetString("token", "No token found");
        username = PlayerPrefs.GetString("username", "No username found");
        NewGameButton.SetActive(false);
        // fToken.text = waitToken;
    }

    // Update is called once per frame
    void Update()
    {
        if (SessionCreated())
        {
            NewGameButton.SetActive(true);
            if (NewGameButton.activeSelf)
            {
                if (LaunchStatus())
                {
                    fToken.text = "Launched!";
                }
            }
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void JoinGame()
    {
        var request = new RestRequest("api/sessions/" + ExtractHash() + "/players/" + username + "?access_token=" + token, Method.PUT)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
    }

    private static string ExtractHash()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        string content = response.Content;
        int index = content.IndexOf('"', 15);
        return content.Substring(14, index - 14);
    }

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
        hash = response.Content;
    }

    private void GetSessions()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        Console.WriteLine(response.Content);
    }

    private static Boolean SessionCreated()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        string content = response.Content;
        if (content.Contains("sessions\":{}"))
        {
            return false;
        }

        return true;
       }

    public void LaunchSession()
    {
        var request = new RestRequest("api/sessions/" + hash + "?access_token=" + token, Method.POST)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
    }

    private static Boolean LaunchStatus()
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
    }
}