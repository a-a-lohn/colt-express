using UnityEngine;
 using UnityEngine.EventSystems;
 using System.Collections;
 using System.Collections.Generic;
 
 public class PlayerLog : MonoBehaviour {
     // Private VARS
     private List<string> Eventlog = new List<string>();
     public string guiText = "";
     private PlayerLog eventLog;
     
 
     void Start(){
         eventLog = GetComponent<PlayerLog>();
         eventLog.AddEvent("Player Moves Left");
         eventLog.AddEvent("Player Moves Right");
          string name =  EventSystem.current.currentSelectedGameObject.name;
          // Debug.Log(name);
         AddEvent("added");
     }

     void Update () 
     {
//         string button = EventSystem.current.currentSelectedGameObject.name;
//         
//         if (button == EventSystem.current.currentSelectedGameObject.name)
//          eventLog.AddEvent("Player clicked on " + button);

//         else if (Input.GetKey(KeyCode.LeftArrow))
//          eventLog.AddEvent("Player Moves Left");
//  
//         else if (Input.GetKey(KeyCode.RightArrow))
//          eventLog.AddEvent("Player Moves Right");
       string name =  EventSystem.current.currentSelectedGameObject.name;
          // Debug.Log(name);
        if (Input.GetKey(KeyCode.LeftArrow))
             eventLog.AddEvent("Player Moves Left");
 
         if (Input.GetKey(KeyCode.RightArrow))
             eventLog.AddEvent("Player Moves Right");

     }

     // Public VARS
     public int maxLines = 3;


    void OnGUI() {
        GUI.Label(new Rect(10,(Screen.height - 150),300f,150f), guiText,GUI.skin.textArea);     
     }

     public void AddEvent(string eventString){
         Eventlog.Add(eventString);
 
         if (Eventlog.Count >= maxLines)Eventlog.RemoveAt(0);
 
         foreach (string logEvent in Eventlog)
         {
             guiText += logEvent;
             guiText += "\n";
        }
     }

  

//     void OnMouseDown(){
//         Debug.Log("loggg"); 
//         string button = EventSystem.current.currentSelectedGameObject.name;
//         if (button == EventSystem.current.currentSelectedGameObject.name){
//             Eventlog.Add("ADDED TO EVENTLOG!");
//             AddEvent("EVENT ADDED");
//         }

// //         if (button == EventSystem.current.currentSelectedGameObject.name)
// //          eventLog.AddEvent("Player clicked on " + button);

// //         else if (Input.GetKey(KeyCode.LeftArrow))
// //          eventLog.AddEvent("Player Moves Left");
// //  
// //         else if (Input.GetKey(KeyCode.RightArrow))
// //          eventLog.AddEvent("Player Moves Right");
//     }
 }