using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(1, 3)] [SerializeField] private float m_SprintSpeed = 2f;			// Amount of maxSpeed applied to sprinting movement. 1 = 100%

	[Range(1, 3)] [SerializeField] private float m_SlideSpeed = 2f;			// Amount of maxSpeed applied to sprinting movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	[SerializeField] private CapsuleCollider2D m_CapsuleCollider;
	[SerializeField] private CapsuleCollider2D m_SlideDisableCollider;
	[SerializeField] private CircleCollider2D m_CircleCollider;
	// [SerializeField] private BoxCollider2D m_BoxColliderWall;


	// private Vector4 antiWallJump = new Vector4(0.2f, 0, 0);

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_Walled;				// Whether or not the player is tuching wall.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public BoolEvent OnSprintEvent;
	private bool m_wasSprinting = false;
	public BoolEvent OnSlideEvent;
	private bool m_wasSlideing = false;


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		if (OnSprintEvent == null)
			OnSprintEvent = new BoolEvent();

		if (OnSlideEvent == null)
			OnSlideEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		// bool wasWalled = m_Walled;
		// m_Walled = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.

 		RaycastHit2D raycastHit2d = Physics2D.BoxCast(m_CircleCollider.bounds.center, m_CircleCollider.bounds.size, 0f, Vector2.down, .01f, m_WhatIsGround);
       // Debug.Log(raycastHit2d.collider);
        if(raycastHit2d.collider != null){
			m_Grounded = true;
			if (!wasGrounded)
			OnLandEvent.Invoke();
			
		}
		// RaycastHit2D isTouchingWall = Physics2D.BoxCast(m_BoxColliderWall.bounds.center, m_BoxColliderWall.bounds.size, 0f, Vector2.right, .01f, m_WhatIsGround);
        // Debug.Log(isTouchingWall.collider);
        // if(isTouchingWall.collider != null){
		// 	m_Walled = true; }
		// 	if (!wasGrounded)
		// 	OnLandEvent.Invoke();
			
		
		
		// Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		// for (int i = 0; i < colliders.Length; i++)
		// {
		// 	if (colliders[i].gameObject != gameObject)
		// 	{
		// 		m_Grounded = true;
		// 		if (!wasGrounded)
		// 			OnLandEvent.Invoke();
		// 	}
		// }

		// else
		// {
		// 	m_Grounded = false;
		// 	OnLandEvent.Invoke();
		// }
		

	}


	public void Move(float move, bool crouch, bool jump, bool sprint ,bool slide)
	{
		m_SlideDisableCollider.enabled = false;
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
				
			}
			
		}
	

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
					
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
			// If slideing
			if(m_Grounded && slide)
			{
			if (!m_wasSlideing)
				{
					m_wasSlideing = true;
					OnSlideEvent.Invoke(true);
				}

				// Increse the slide by the slideSpeed multiplier
				move *= m_SlideSpeed;
				if (m_CrouchDisableCollider != null)
					m_SlideDisableCollider.enabled = true;
					m_CrouchDisableCollider.enabled = false;
					m_CapsuleCollider.enabled = false;
			} else
			{
				
				if (m_wasSlideing)
				{
					m_wasSlideing = false;
					OnSlideEvent.Invoke(false);
					m_CapsuleCollider.enabled = true;
				}
			}
			// If Sprinting
			if (sprint)
			{
				if (!m_wasSprinting)
				{
					m_wasSprinting = true;
					OnSprintEvent.Invoke(true);
				}

				// Increse the speed by the sprintSpeed multiplier
				move *= m_SprintSpeed;

			} else
			{

				if (m_wasSprinting)
				{
					m_wasSprinting = false;
					OnSprintEvent.Invoke(false);
					
				}
			}
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			// m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			
			
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	
}
