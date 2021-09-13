using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController2D controller;
  
    [Range(0, 100f)] public float runSpeed = 40f;
    [Range(0, 100f)] public float verticalSpeed = 40f;

    public static int j = 0; 
    bool jump = false;
    float horizontalMove = 0f;
    float verticalMove = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * verticalSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            if(Input.GetButtonDown("Jump"))
            {
                j ++;
            }
        }
    }
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove*Time.fixedDeltaTime, jump,j);
        jump = false;
    }
}
