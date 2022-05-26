using UnityEngine;

public class PlayerCharCont : MonoBehaviour
{
    CharacterController charCont;

    private Vector3 moveDirection = Vector3.zero;   //Player direction of movement.
    [SerializeField] private float speed;           //Player speed modifier.
    [SerializeField] private float sprintSpeed;
    private float startSpeed;                       //Holds initial speed, so we can default to that speed when player finishes sprinting.
    [SerializeField] private float gravity = 20f;   //Gravity.
    [SerializeField] private float jumpSpeed = 8f;  //Jump height.

    [SerializeField] private float pushSpeed;       //How fast player pushes interactables.


    //Store reference to player's character controller and set default speed.
    private void Start()    
    {
        charCont = GetComponent<CharacterController>(); 
        startSpeed = speed;                             
    }


    void Update() 
    {
        PlayerMove();       //Handles player movement
    }


    
    //Player Movement
    void PlayerMove()                          
    {
        
        if (charCont.isGrounded)                        
        {   
            //Get keyboard input
            float moveX = Input.GetAxis("Horizontal");  
            float moveZ = Input.GetAxis("Vertical");

            //Sprint speed / resetting to default after sprint ends.
            if (Input.GetButton("Sprint"))              
            {
                speed = sprintSpeed;                    
            }
            else
            {
                speed = startSpeed;                     
            }

            //Calculate vector for movement and modify by the speed modifier.
            moveDirection = transform.right * moveX + transform.forward * moveZ; 
            moveDirection *= speed;                                              

            //Modify movement vector to account for jumping.
            if (Input.GetButton("Jump"))        
            {
                moveDirection.y = jumpSpeed;    
            }

        }

        //Modify movement vector to account for gravity and then apply movement to the player.
        moveDirection.y -= gravity * Time.deltaTime;
        charCont.Move(moveDirection * Time.deltaTime);
    }

    

    //Handles collisions.
    private void OnControllerColliderHit(ControllerColliderHit hit)     
    {
        
        //Store reference to rigidbody on the collided gameobject.
        Rigidbody body = hit.collider.attachedRigidbody;    

        //Error mitigation. Ends function if no rigidbody or "is kinematic" is checked in the inspector.
        if (body == null || body.isKinematic)               
        {
            return;                                         
        }

        // Calculate push direction from move direction. Here, we are using it to make interactavles pushable. Not pushable on the y axis.
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //Apply push to the interactable.
        body.velocity = pushDir * pushSpeed;
    }

}
