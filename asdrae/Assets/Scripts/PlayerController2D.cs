using UnityEngine;
using UnityEngine.Events;

public class PlayerController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	bool jump = false;

	private static float yMin = 2.16f, yMax = 2.88f;

	private float jumpingPoint = yMax +0.5f;

	const float k_GroundedRadius = 0f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private bool canDoubleJump = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Update()
	{
		
		if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
		if (jump)
		{	
			jumpingPoint = transform.position.y;
			this.GetComponent<Rigidbody2D>().gravityScale = 3f; //Enable gravity on this object
			// Add a vertical force to the player.
			if (m_Grounded && !canDoubleJump)
			{
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = true;
				m_Grounded = false;
				jump = false;
			}
			if(canDoubleJump)
			{
				Debug.Log("Segundo");
				m_Rigidbody2D.velocity = Vector2.zero;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = false;
			}
		}
		
		if (m_GroundCheck.position.y < jumpingPoint)
		{
			this.GetComponent<Rigidbody2D>().gravityScale = 0f; //disable gravity on this object
			transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y, yMin, yMax),transform.position.z);
		}		

	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{		
				m_Grounded = true;
				PlayerMovement.j = 0;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Move(float move,float vertic_move, bool jump, int j)
	{
		
		Debug.Log(Input.GetAxisRaw("Horizontal"));
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, vertic_move * 50f );
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
		// if (jump)
		// {	
		// 	this.GetComponent<Rigidbody2D>().gravityScale = 3f; //Enable gravity on this object
		// 	// Add a vertical force to the player.
		// 	if (m_Grounded && !canDoubleJump)
		// 	{
		// 		jumpingPoint = transform.position.y;
		// 		m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*10));
		// 		canDoubleJump = true;
		// 		m_Grounded = false;
		// 	}
		// 	if(canDoubleJump && j == 1)
		// 	{
		// 		Debug.Log("Segundo");
		// 		m_Rigidbody2D.velocity = Vector2.zero;
		// 		m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		// 		canDoubleJump = false;
		// 	}
		// }
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