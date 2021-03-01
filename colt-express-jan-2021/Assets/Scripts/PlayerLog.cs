 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public class PlayerLog : MonoBehaviour 
 {
 public int maxLines = 5;
 
 private Queue queue = new Queue();
 private string[] testStrings = {"Eating crackers", "Stomping foot", "Trying lock", "Drinking potion", "Picking up dagger", "Hitting the wall", "Turning off lights"};
 private string text = "";
 
 void Start() {
     InvokeRepeating("Test", 0, 2);
 }
 
 void Test() {
     NewActivity(testStrings[Random.Range(0,testStrings.Length)]);
 }
 
 public void NewActivity(string activity) {
     if (queue.Count >= 5)
         queue.Dequeue();

     queue.Enqueue(activity);
     
     text = "";
     foreach (string st in queue)
         text = text + st + "\n";
 }
 

    void OnGUI() {
 
      GUI.Label(new Rect(5,(Screen.height - 150),300f,150f), text,GUI.skin.textArea);    
         
     }

 }