using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController2D controller;
  
    public float runSpeed = 40f;

    public static int j = 0; 
    bool jump = false;
    bool jump2 = false;
    float horizontalMove = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            if(Input.GetButtonDown("Jump"))
            {
                jump2 = true;
                j ++;
            }
        }
    }
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump,j);
        jump = false;
        jump2 = false;
    }
}
