using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lootToMove; 

    void Start()
    {
        // transform.position = Vector3.Lerp(startPoint, endPoint, (Time.deltaTime));
    }

    // void OnMouseDown(){
    //     // Vector3 p = lootToMove.transform.position; 
    //     // float x = lootToMove.transform.position.x;
    //     // x = x + 50; 
    //     // float y = lootToMove.transform.position.y;
    //     // float z = lootToMove.transform.position.z;
    //     // y += 60;
    //     // lootToMove.transform.position = new Vector3(x, y, z);
    //     //  startPoint = transform.position;
    //     //  endPoint = new Vector3(beginX, beginY, 0);

    // }

    // void OnMouseUp(){
    //      startPoint = transform.position;
    //      endPoint = new Vector3(beginX, beginY, 0);
    // }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        // this wont move the object forwards but will reset it's position to 0, 0, 1
        Debug.Log(lootToMove.transform.position);
            lootToMove.transform.position = new Vector3 (1219, 819, -364);
        // this code will do the trick
            lootToMove.transform.position += lootToMove.transform.forward * Time.deltaTime * 5f; // can be any float number
        }
    }
}
