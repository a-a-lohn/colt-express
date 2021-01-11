// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerMove : MonoBehaviour
// {
//     private Rigidbody2D rb;
//     private bool moveLeft;
//     private bool moveRight;
//     private float horizontalMove;
//     public float speed = 5;

//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         moveLeft = false;
//         moveRight = false;

//     }

//     //Left button pressed
//     public void PointerDownLeft(){
//         moveLeft = true;
//     }
//     //Left button released
//     public void PointerUpLeft(){
//         moveLeft = false;
//     }
//     //Right button pressed
//      public void PointerDownRight(){
//         moveRight = true;
//     }
//     //Right button released
//     public void PointerUpRight(){
//         moveRight = false;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//          rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
//          Movement();
//     }

//     private void Movement(){
//         //If I press the left button
//         if(moveLeft){
//             horizontalMove = -speed;
//         }
//         //If I press the right button
//         else if(moveRight){
//             horizontalMove = speed;
//         }
//         //If no button is pressed
//         else{
//             horizontalMove = 0;
//         }
//     }

//     //add the movement force to the player
//     private void fixedUpdate(){
       
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private float currentX;
    private float currentY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveLeft(){
        currentX = rb.position.x;
        currentY = rb.position.y;
        rb.position = new Vector2(currentX-4,currentY);
    }

    public void MoveRight(){
        currentX = rb.position.x;
        currentY = rb.position.y;
        rb.position = new Vector2(currentX+4,currentY);
    }

}