using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    Vector2 movement = new Vector2();
    bool isHorizonMove;
    Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start(){
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Vertical")) isHorizonMove = true;
        else if(Input.GetButtonDown("Vertical") || Input.GetButtonUp("Horizontal")) isHorizonMove = false;
    }

    private void FixedUpdate() {
        if(isHorizonMove) movement.y = 0;
        else movement.x = 0;
        
        movement.Normalize();
        rigidbody2D.velocity = movement * movementSpeed;
    }
}
