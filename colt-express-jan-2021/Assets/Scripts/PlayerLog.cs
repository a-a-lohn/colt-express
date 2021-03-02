using UnityEngine;
 using UnityEngine.EventSystems;
 using System.Collections;
 using System.Collections.Generic;
 
 public class PlayerLog : MonoBehaviour 
 {
     // Private VARS
     private List<string> Eventlog = new List<string>();
     public string guiText = "";
 
     // Public VARS
     public int maxLines = 3;

     public void AddEvent(string eventString)
     {
         Eventlog.Add(eventString);
 
         if (Eventlog.Count >= maxLines)Eventlog.RemoveAt(0);
 
         foreach (string logEvent in Eventlog)
         {
             guiText += logEvent;
             guiText += "\n";
        }
     }
 
    private PlayerLog eventLog;
 
     void Start()
     {
         eventLog = GetComponent<PlayerLog>();
     }
     
     void Update () 
     {
        string button = EventSystem.current.currentSelectedGameObject.name;
        
        if (button == EventSystem.current.currentSelectedGameObject.name)
         eventLog.AddEvent("Player clicked on " + button);

        else if (Input.GetKey(KeyCode.LeftArrow))
         eventLog.AddEvent("Player Moves Left");
 
        else if (Input.GetKey(KeyCode.RightArrow))
         eventLog.AddEvent("Player Moves Right");

     }


    void OnGUI() {
 
      GUI.Label(new Rect(5,(Screen.height - 150),300f,150f), guiText,GUI.skin.textArea);    
         
     }
 }