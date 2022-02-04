using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{// [HideInInspector]
   public bool mustPatrol;
   public float walkSpeed;
   public Rigidbody2D rigidbody2;
   public Transform groundCheckPos;
   private bool mustTurn;
   public LayerMask groundLayer;
   public Animator animator;
   [SerializeField] private BoxCollider2D bodyCollider;
    void Start()
    {
        mustPatrol = true;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    { if(mustPatrol){

            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(mustPatrol){
            animator.SetFloat("Speed",Mathf.Abs(walkSpeed));
            Patrol();
        }
    }
    void Patrol(){
        if(mustTurn || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        rigidbody2.velocity = new Vector2(walkSpeed * 10f * Time.fixedDeltaTime, rigidbody2.velocity.y);
    }
    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}
