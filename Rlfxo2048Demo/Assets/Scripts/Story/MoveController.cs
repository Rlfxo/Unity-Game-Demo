using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float movementSpeed = 1.5f;
    public GameManager manager;
    Vector2 movement = new Vector2();
    bool isHorizonMove;
    Vector3 dirVec;
    GameObject scanObject;
    Rigidbody2D rigidbody2D;
    Animator anim;
    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        // movement
        movement.x = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        movement.y = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");
        
        // XY movement
        if(Input.GetButtonDown("Horizontal")) isHorizonMove = true;
        else if(Input.GetButtonDown("Vertical")) isHorizonMove = false;
        else if(Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical")) isHorizonMove = movement.x != 0;

        // Animation
        if(anim.GetInteger("hAxisRaw") != (int)movement.x){
            anim.SetBool("isMove", true);
            anim.SetInteger("hAxisRaw", (int)movement.x);
            
        }else if(anim.GetInteger("vAxisRaw") != (int)movement.y){
            anim.SetBool("isMove", true);
            anim.SetInteger("vAxisRaw", (int)movement.y);
            
        }else anim.SetBool("isMove", false);

        // Get direction
        if (Input.GetButtonDown("Vertical") && movement.y == 1) dirVec = Vector3.up;
        else if(Input.GetButtonDown("Vertical") && movement.y == -1) dirVec = Vector3.down;
        else if(Input.GetButtonDown("Horizontal") && movement.x == -1) dirVec = Vector3.left;
        else if(Input.GetButtonDown("Horizontal") && movement.x == 1) dirVec = Vector3.right;

        // Scan Object
        if(Input.GetButtonDown("Jump") && scanObject != null) manager.Action(scanObject);
        //Debug.Log("Object" + scanObject.name);
    }

    private void FixedUpdate() {
        if(isHorizonMove) movement.y = 0;
        else movement.x = 0;
        // Move
        movement.Normalize();
        rigidbody2D.velocity = movement * movementSpeed;

        // Ray
        Debug.DrawRay(rigidbody2D.position, dirVec * 1f, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigidbody2D.position, dirVec, 0.6f, LayerMask.GetMask("Object"));
        if(rayHit.collider != null) {
            scanObject = rayHit.collider.gameObject;
        }else scanObject = null;
    }
}
