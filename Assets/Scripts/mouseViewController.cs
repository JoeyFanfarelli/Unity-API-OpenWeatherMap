using UnityEngine;

public class mouseViewController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;


    //lock cursor to center of screen and make it disappear.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
    }


    void Update()
    {
        mouseMovement();    //Allows player to alter view by moving mouse.
    }

    //Allows player to alter view by moving the mouse.
    void mouseMovement()    
    {
        //Get player mouse movement.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Calculate rotation, and define min/maximum rotations boundaries (i.e., so player can't see behind without also rotating body).
        xRotation = xRotation - mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        //Apply rotation and actually rotate the player.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
