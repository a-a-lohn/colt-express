// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using UnityEngine.SceneManagement;
// // using System;
// // using RestSharp; 

// // public class Login : MonoBehaviour{
// //     public void Start(){

// //     }
// //     public void Update(){

// //     }
    
// //     public void Host(){
// //         verification();
// //         // CreateNewUser();
// //         SceneManager.LoadScene("HostGame"); // load all scenes named "HostGame"
// //     }

// //     public void Join(){
// //         SceneManager.LoadScene("JoinGame"); // load all scenes named "JoinGame"
// //     }

// //     public void Quit(){
// //         Application.Quit(); // quit the game
// //     }

//     public void CreateNewUser(){
//         var client = new RestClient("http://127.0.0.1:4242/api/users/aaron?access_token=6u0t5GGq4OmHGQxZj1n9Ii%2BVwWY=");
//         client.Timeout = -1;
//         var request = new RestRequest(Method.PUT);
//         request.AddHeader("Content-Type", "application/json");
//         request.AddParameter("application/json", "{\"name\":\"aaron\",\"password\":\"abc123_ABC123\",\"preferredColour\":\"01FFFF\",\"role\":\"ROLE_PLAYER\"}",  ParameterType.RequestBody);
//         IRestResponse response = client.Execute(request);
//         Console.WriteLine(response.Content);
//         // Debug.log(response.Content); 
//     }

// //     public void verification(){
// //         var client = new RestClient("http://127.0.0.1:4242/oauth/token?grant_type=password&username=admin&password=admin");
// //         client.Timeout = -1;
// //         var request = new RestRequest(Method.POST);
// //         request.AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
// //         IRestResponse response = client.Execute(request);
// //         Console.WriteLine(response.Content);
// //         // Debug.Log(response.Content.token);
// //         string str = response.Content; 
// //         Debug.Log("we're inside verification!"); 
// //         Debug.Log(str); 
// //     }

// // }

// using UnityEngine;
// using System.Collections;
// using UnityEngine.UI; 
// using UnityEngine.SceneManagement;

// public class Login : MonoBehaviour {
//     public InputField username; // used to store user info
//     public InputField password; 
//     public Text fText; // used to put the username+password to the screen (for testing purposes)

//     public void VerifyUser(string username, string password){
//         fText.text = "the username entered is:" + username.text + " and password is: " + password.text;
//         username = "admin"; 
//         password = "admin";
//         var request = new RestRequest("oauth/token", Method.POST)
//             .AddParameter("grant_type", "password")
//             .AddParameter("username", username)
//             .AddParameter("password", password)
//             .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
//         IRestResponse response = client.Execute(request);
//         var obj = JObject.Parse(response.Content);
//         // string str = (string)obj;
//         // Debug.Log(str);
//         return (string)obj["access_token"];

//         // if verified: 
//         // go to the WaitingRoom scene 
//         Invoke("GoToWR", 5); //this will happen after 2 seconds
//         // SceneManager.LoadScene("WaitingRoom");
//     }

//     public void GoToWR(){
//         SceneManager.LoadScene("WaitingRoom");
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System;
// using RestSharp; 

// public class Login : MonoBehaviour{
//     public void Start(){

//     }
//     public void Update(){

//     }
    
//     public void Host(){
//         verification();
//         // CreateNewUser();
//         SceneManager.LoadScene("HostGame"); // load all scenes named "HostGame"
//     }

//     public void Join(){
//         SceneManager.LoadScene("JoinGame"); // load all scenes named "JoinGame"
//     }

//     public void Quit(){
//         Application.Quit(); // quit the game
//     }

//     public void CreateNewUser(){
//         var client = new RestClient("http://127.0.0.1:4242/api/users/aaron?access_token=6u0t5GGq4OmHGQxZj1n9Ii%2BVwWY=");
//         client.Timeout = -1;
//         var request = new RestRequest(Method.PUT);
//         request.AddHeader("Content-Type", "application/json");
//         request.AddParameter("application/json", "{\"name\":\"aaron\",\"password\":\"abc123_ABC123\",\"preferredColour\":\"01FFFF\",\"role\":\"ROLE_PLAYER\"}",  ParameterType.RequestBody);
//         IRestResponse response = client.Execute(request);
//         Console.WriteLine(response.Content);
//         // Debug.log(response.Content); 
//     }

//     public void verification(){
//         var client = new RestClient("http://127.0.0.1:4242/oauth/token?grant_type=password&username=admin&password=admin");
//         client.Timeout = -1;
//         var request = new RestRequest(Method.POST);
//         request.AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
//         IRestResponse response = client.Execute(request);
//         Console.WriteLine(response.Content);
//         // Debug.Log(response.Content.token);
//         string str = response.Content; 
//         Debug.Log("we're inside verification!"); 
//         Debug.Log(str); 
//     }

// }

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.CoreModule;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using RestSharp; 
using Newtonsoft.Json.Linq;


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
        Invoke("GoToWR", 3); //this will happen after 2 seconds
        // SceneManager.LoadScene("WaitingRoom");


        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", username.text)
            .AddParameter("password", password.text)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        token = (string)obj["access_token"];
        PlayerPrefs.SetString("token", token);
        PlayerPrefs.SetString("username", username.text);
        PlayerPrefs.Save();

        fText.text = (string)obj["access_token"];
    }

    public void GoToWR(){
        SceneManager.LoadScene("WaitingRoom");
    }
}

