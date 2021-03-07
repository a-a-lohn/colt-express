using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseCharacter : MonoBehaviour
{
    public Text selected; 

    // Start is called before the first frame update
    void Start()
    {
         // rend = GetComponent<Renderer>();
         // name = this.GameObject;
 
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
             string name =  EventSystem.current.currentSelectedGameObject.name;
            //  Debug.Log(name + "ahh"); 
            selected.text = "Your bandit is: " + name;
        }
        
    }

    void OnMouseEnter()
 	{
 
    //   string objectName = gameObject.name;
    //   Debug.Log(objectName);
    //  startcolor = rend.material.color;
    //  rend.material.color = Color.grey;
     // Debug.Log(this.GameObject.name);
 	}

    void OnMouseExit()
 	{
 
 	}

 	void OnMouseDown()
 	{

 	}
}
