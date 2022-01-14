using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float movementSpeed = 1.5f;
    Vector2 movement = new Vector2();
    bool isHorizonMove;
    Rigidbody2D rigidbody2D;
    Animator anim;
    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if(Input.GetButtonDown("Horizontal")) isHorizonMove = true;
        else if(Input.GetButtonDown("Vertical")) isHorizonMove = false;
        else if(Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical")) isHorizonMove = movement.x != 0;

        if(anim.GetInteger("hAxisRaw") != (int)movement.x){
            anim.SetBool("isMove", true);
            anim.SetInteger("hAxisRaw", (int)movement.x);
            
        }else if(anim.GetInteger("vAxisRaw") != (int)movement.y){
            anim.SetBool("isMove", true);
            anim.SetInteger("vAxisRaw", (int)movement.y);
            
        }else anim.SetBool("isMove", false);
    }

    private void FixedUpdate() {
        if(isHorizonMove) movement.y = 0;
        else movement.x = 0;

        movement.Normalize();
        rigidbody2D.velocity = movement * movementSpeed;
    }
}
