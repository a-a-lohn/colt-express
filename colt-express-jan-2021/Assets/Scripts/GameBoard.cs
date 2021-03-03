using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

public class GameBoard : MonoBehaviour
{

    public GameObject cheyenne;
    public Bandit b;
    
    Dictionary<GameObject, Bandit> bandits = new Dictionary<GameObject, Bandit>();

    // Start is called before the first frame update
    void Start()
    {
        // Receive classes that were created on server upon game startup here
        
        bandits.Add(cheyenne, b);
    }

    // Update is called once per frame
    void Update()
    {
        // updateGameState method goes here and reassigns all game objects in dictionaries to received objects

        // prompts come in here and indicate (e.g. with a log message what user should do/click).
        // The prompt should send all the objects that are clickable
        
        // user clicks on a gameobject, we check that it is valid, i.e. is one of the available clickable objects
        // received from prompt, and if so it is sent to the server

        // have listener methods for different prompts that verifies if game object that user clicks
        // is valid using dictionary
    }

}
