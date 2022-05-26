using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Update()
    {
        //Quit game when player presses the escape key.
        if (Input.GetKey("escape"))         
        {
            Application.Quit();
        }
    }
}
