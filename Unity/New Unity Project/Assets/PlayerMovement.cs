using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
   
float runSpeed = 40f;
    void Update()
    {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed",Mathf.Abs(horizontalMove));

            if(Input.GetButtonDown("Jump"))
{
jump = true;
animator.SetBool("IsJumping",true);
}
            if(Input.GetButtonDown("Crouch"))
{
crouch = true;
}
else if (Input.GetButtonUp("Crouch"))
{
  crouch = false;  
}
    }


  public void OnCrouching(bool isCrouching)
  {
animator.SetBool("IsCrouching", isCrouching);
  }
    public void OnLanding()
    {
animator.SetBool("ISJumping", false);
    }
    void FixedUpdate ()
    {
        {
controller.Move(horizontalMove *Time.fixedDeltaTime, crouch, jump);
 jump = false;
        }
    }
}
