using UnityEngine;
 using UnityEngine.EventSystems;
using UnityEngine.UI;
 using System.Collections;
 using System.Collections.Generic;
 
 public class PlayerLog : MonoBehaviour {
     // Private VARS
     public List<string> Eventlog = new List<string>();
      // public string guiText = "";
     public string guiText = "Player Played: ";
     private string logmsg; 
     public int maxLines = 3; 
        public GameObject cardA; 
        public Text log; 
     // public Text msg; 
//      void OnGUI() {
//         GUI.Label(new Rect(10,(Screen.height - 150),300f,150f), guiText,GUI.skin.textArea);     
//      }

//      public void AddEvent(string eventString){
//          Eventlog.Add(eventString);
//  
//          if (Eventlog.Count >= maxLines) Eventlog.RemoveAt(0);
         
//          guiText = ""; 

//          foreach (string logEvent in Eventlog)
//          {
//              guiText += logEvent;
//              guiText += "\n";
//         }
//      }

     void Start(){
       // log = GetComponent<Text>(); 
     //     eventLog.AddEvent("Player Moves Left");
     //     eventLog.AddEvent("Player Moves Right");
          // string name =  EventSystem.current.currentSelectedGameObject.name;
          // Debug.Log(name);
         // AddEvent("added");
         log.text = guiText;
 
 
     }

     public void OnButtonClick()
     {
         var go = EventSystem.current.currentSelectedGameObject;
         if (go != null)
             Debug.Log("Clicked on : "+ go.name);
         else
             Debug.Log("curr game obj is null :(");

          guiText += go.name;
          guiText += "\n";
     }

//      public void AddEvent(string eventString){
 
//  
//          // if (Eventlog.Count >= maxLines) Eventlog.RemoveAt(0);
         
//          guiText = ""; 
//          guiText += eventString; 

// //          foreach (string logEvent in Eventlog)
// //          {
// //              guiText += logEvent;
// //              guiText += "\n";
// //         }
//      }

     void Update () 
     {
          log.text = guiText;
          // msg.text = guiText; 
          // if(Input.GetButtonDown("CardA")){
          //      Debug.Log("CARDA");

          //      Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
          //      RaycastHit hit;
          //      if( Physics.Raycast( ray, out hit, 100 ) )
          //      {
          //           Debug.Log( hit.transform.gameObject.name );
          //      } 

          // }
     
//         string button = EventSystem.current.currentSelectedGameObject.name;
//         
//         if (button == EventSystem.current.currentSelectedGameObject.name)
//          eventLog.AddEvent("Player clicked on " + button);

//         else if (Input.GetKey(KeyCode.LeftArrow))
//          eventLog.AddEvent("Player Moves Left");
//  
//         else if (Input.GetKey(KeyCode.RightArrow))
//          eventLog.AddEvent("Player Moves Right");
      // string name =  EventSystem.current.currentSelectedGameObject.name;
          // Debug.Log(name);
          
     //    if (Input.GetKey(KeyCode.LeftArrow))
     //         eventLog.AddEvent("Player Moves Left");
 
     //     if (Input.GetKey(KeyCode.RightArrow))
     //         eventLog.AddEvent("Player Moves Right");

     }

     // void OnMouseDown(){
     //      // if (!Input.GetMouseButtonDown(0)) return;
     //      Debug.Log (this.gameObject.name);
     //      eventLog.AddEvent("Player Moves Left");
     //      eventLog.AddEvent("Player Moves Right");
     // }

     // Public VARS
     // public int maxLines = 3;


  

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