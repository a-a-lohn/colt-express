using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using RestSharp; 
using Newtonsoft.Json.Linq;

using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;


public class Login : MonoBehaviour {
    public InputField username; // used to store user info
    public InputField password; 
    public Text fText; // used to put the username+password to the screen (for testing purposes)

    public static RestClient client = new RestClient("http://13.90.26.131:4242");
    public static string token;
    // public static string username;
    // public static string password;

    public void VerifyUser(){
        fText.text = "the username entered is:" + username.text + " and password is: " + password.text;
        //aaron's PutToken code
        // var request = new RestRequest("oauth/token", Method.POST)
        //     .AddParameter("grant_type", "password")
        //     .AddParameter("username", username.text)
        //     .AddParameter("password", password.text)
        //     .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        // IRestResponse response = client.Execute(request);
        // var obj = JObject.Parse(response.Content);
        // // string str = (string)obj;
        // // Debug.Log(str);
        // fText.text = (string)obj["access_token"];

        // if verified: 
        // go to the WaitingRoom scene 
       // Invoke("GoToWR", 2); //this will happen after 2 seconds
        // SceneManager.LoadScene("WaitingRoom");


        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", username.text)
            .AddParameter("password", password.text)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        
        try {
            var obj = JObject.Parse(response.Content);
             token = (string)obj["access_token"];
            token = token.Replace("+", "%2B");
            PlayerPrefs.SetString("token", token);
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.Save();
            fText.text = token;
            GoToWR();
        } catch (Exception e) {
            fText.text = "Invalid username or password";
        }
    }

    public void GoToWR(){
        // Initialize SFS2X client. This can be done in an earlier scene instead
		SmartFox sfs = new SmartFox();
        // For C# serialization
		DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
        SFS.setSFS(sfs);
        SFS.Connect();

        SceneManager.LoadScene("WaitingRoom");
    }

}
