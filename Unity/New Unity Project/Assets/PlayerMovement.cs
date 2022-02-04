using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool sprint = false;
   bool slide = false;
   
float runSpeed = 40f;
    void Update()
    {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed",Mathf.Abs(horizontalMove));

if(Input.GetButtonDown("Jump")) {
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
if(Input.GetButtonDown("Sprint"))
{
sprint = true;
animator.SetBool("IsSprinting",true);
}
else if (Input.GetButtonUp("Sprint"))
{
  sprint = false;
  animator.SetBool("IsSprinting",false);  
}
if(Input.GetButtonDown("Slide") && sprint == true){
  

slide = true;
animator.SetBool("IsSlideing",true);
StartCoroutine("stopSlide");
 
}

  }
    

  public void OnCrouching(bool isCrouching)
  {
animator.SetBool("IsCrouching", isCrouching);
  }

    public void OnLanding()
    {
      if(jump == false)
 {
   animator.SetBool("IsJumping", false);
 }
    }
//     public void OnSprinting(bool isSprinting)
//   {
// animator.SetBool("IsSprinting", isSprinting);
//   }

IEnumerator stopSlide(){
  yield return new WaitForSeconds (0.8f);
  animator.SetBool("IsSlideing",false);
  slide = false;
}
    void FixedUpdate ()
    {
        {
controller.Move(horizontalMove *Time.fixedDeltaTime, crouch, jump, sprint, slide);
 jump = false;
 

        }
    }
}
