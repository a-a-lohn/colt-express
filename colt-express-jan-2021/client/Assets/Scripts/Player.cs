// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Player : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{

    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            // *0.1f to make it move slower
            Vector3 movement = new Vector3(moveHorizontal * 0.1f, moveVertical * 0.1f, 0);
            transform.position = transform.position + movement;
        }
    }

    void Update()
    {
        HandleMovement();
    }

    // public override void OnStartServer()
    // {
    //     Debug.Log("Player has been spawned on the server!");
    // }

    // [Command]
    // void Hola()
    // {
    //     Debug.Log("Received Hola from Client!");
    //     holaCount += 1;
    //     ReplyHola();
    // }

    // [TargetRpc]
    // void ReplyHola()
    // {
    //     Debug.Log("Received Hola from Server!");
    // }

    // [ClientRpc]
    // void TooHigh()
    // {
    //     Debug.Log("Too high!");
    // }

    // void OnHolaCountChanged(int oldCount, int newCount)
    // {
    //     Debug.Log($"We had {oldCount} holas, but now we have {newCount} holas!");
    // }
}