using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using RestSharp; 
using Newtonsoft.Json.Linq;

public class Login : MonoBehaviour {
    public InputField username; // used to store user info
    public InputField password; 
    public Text fText; // used to display info on the screen

    public static RestClient client = new RestClient("http://127.0.0.1:4242");
    public static string token;

    public void VerifyUser(){
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
            GoToWR();
        } catch (Exception e) {
            fText.text = "Invalid username or password";
        }
    }

    public void GoToWR(){       
        SceneManager.LoadScene("WaitingRoom");
    }

    public void GoToMM() {
        SceneManager.LoadScene("MainMenu");
    }

}
