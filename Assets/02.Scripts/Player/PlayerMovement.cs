using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _02.Scripts.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float jumpForce = 400f;							// 점프력
		[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// 부드러운 이동 수치
		[SerializeField] private bool canAirControl = true;						// 공중에서 컨트롤 가능 여부
		[SerializeField] private LayerMask groundLayer;							// 땅 레이어
		[SerializeField] private LayerMask wallLayer;							//벽 레이어
		[SerializeField] private Transform groundCheck;								// 땅 체크용 Transform
		[SerializeField] private Transform wallCheck;								// 벽 체크용 Transform

		const float GroundRadius = .5f;		// 땅 체크 범위
		public bool isGrounded;            // 땅에 붙어있는지 여부
		private Rigidbody2D rb;
		private bool isFacingRight = true;  // 바라보는 방향
		private Vector3 velocity = Vector3.zero;
		private float limitFallSpeed = 25f; // 떨어지는 최대 스피드

		public bool canDoubleJump = true; //더블 점프 가능 여부
		[SerializeField] private float m_DashForce = 25f;
		private bool canDash = true;
		public bool isDashing = false; 
		public bool isWall = false; 
		private bool isWallSliding = false; 
		private bool oldWallSlidding = false; 
		private float prevVelocityX = 0f;
		private bool canCheck = false; 

		public float life = 10f; 
		public bool invincible = false; 
		private bool canMove = true; 

		private PlayerAttack playerAttack;
		private Animator animator;
		public ParticleSystem particleJumpUp; //점프시 따라오는 효과
		public ParticleSystem particleJumpDown; //착지 시 나오는 효과

		private float jumpWallStartX = 0;
		private float jumpWallDistX = 0; 
		private bool limitVelOnWallJump = false; 

		[Header("Events")]
		public UnityEvent OnFallEvent;
		public UnityEvent OnLandEvent;

		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponentInChildren<Animator>();
			playerAttack = GetComponent<PlayerAttack>();

			if (OnFallEvent == null)
				OnFallEvent = new UnityEvent();

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();
		}


		private void FixedUpdate()
		{
			bool wasGrounded = isGrounded;
			isGrounded = false;
			
			Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundRadius, groundLayer);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
					isGrounded = true;
				if (!wasGrounded )
				{
					OnLandEvent.Invoke();
					if (!isWall && !isDashing) 
						particleJumpDown.Play();
					canDoubleJump = true;
					if (rb.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
			}

			isWall = false;

			if (!isGrounded)
			{
				OnFallEvent.Invoke();
				Collider2D[] collidersWall = Physics2D.OverlapCircleAll(wallCheck.position, 0.2f, wallLayer);
				for (int i = 0; i < collidersWall.Length; i++)
				{
					if (collidersWall[i].gameObject != null)
					{
						isDashing = false;
						isFacingRight = true;
					}
				}
				prevVelocityX = rb.velocity.x;
			}

			if (limitVelOnWallJump)
			{
				if (rb.velocity.y < -0.5f)
					limitVelOnWallJump = false;
				jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
				if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
				{
					canMove = true;
				}
				else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
				{
					canMove = true;
					rb.velocity = new Vector2(10f * transform.localScale.x, rb.velocity.y);
				}
				else if (jumpWallDistX < -2f) 
				{
					limitVelOnWallJump = false;
					rb.velocity = new Vector2(0, rb.velocity.y);
				}
				else if (jumpWallDistX > 0) 
				{
					limitVelOnWallJump = false;
					rb.velocity = new Vector2(0, rb.velocity.y);
				}
			}
		}


		public void Move(float move, bool jump, bool dash)
		{
			if (canMove) {
				if (dash && canDash && !isWallSliding)
				{
					rb.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
					StartCoroutine(DashCooldown());
				}
				if (isDashing)
				{
					rb.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
				}
				//플레이어가 땅에 붙어있거나 공중 제어 가능 상태일때만 움직일 수 있게
				else if (isGrounded || canAirControl)
				{
					if (rb.velocity.y < -limitFallSpeed)
						rb.velocity = new Vector2(rb.velocity.x, -limitFallSpeed);
					Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
					rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

					if (move > 0 && !isFacingRight && !isWallSliding)
					{
						Flip();
					}
					else if (move < 0 && isFacingRight && !isWallSliding)
					{
						Flip();
					}
				}
				// 플레이어 점프
				if (isGrounded && jump)
				{
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true);
					isGrounded = false;
					rb.AddForce(new Vector2(0f, jumpForce));
					canDoubleJump = true;
					particleJumpDown.Play();
					particleJumpUp.Play();
				}
				else if (!isGrounded && jump && canDoubleJump && !isWallSliding)
				{
					canDoubleJump = false;
					rb.velocity = new Vector2(rb.velocity.x, 0);
					rb.AddForce(new Vector2(0f, jumpForce / 1.2f));
					animator.SetBool("IsDoubleJumping", true);
				}

				else if (isWall && !isGrounded)
				{
					if (!oldWallSlidding && rb.velocity.y < 0 || isDashing)
					{
						isWallSliding = true;
						wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
						Flip();
						StartCoroutine(WaitToCheck(0.1f));
						canDoubleJump = true;
						animator.SetBool("IsWallSliding", true);
					}
					isDashing = false;

					if (isWallSliding)
					{
						if (move * transform.localScale.x > 0.1f)
						{
							StartCoroutine(WaitToEndSliding());
						}
						else 
						{
							oldWallSlidding = true;
							rb.velocity = new Vector2(-transform.localScale.x * 2, -5);
						}
					}

					if (jump && isWallSliding)
					{
						animator.SetBool("IsJumping", true);
						animator.SetBool("JumpUp", true); 
						rb.velocity = new Vector2(0f, 0f);
						rb.AddForce(new Vector2(transform.localScale.x * jumpForce *1.2f, jumpForce));
						jumpWallStartX = transform.position.x;
						limitVelOnWallJump = true;
						canDoubleJump = true;
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
						canMove = false;
					}
					else if (dash && canDash)
					{
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
						canDoubleJump = true;
						StartCoroutine(DashCooldown());
					}
				}
				else if (isWallSliding && !isWall && canCheck) 
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
					canDoubleJump = true;
				}
			}
		}


		private void Flip()
		{
			isFacingRight = !isFacingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public void ApplyDamage(float damage, Vector3 position) 
		{
			if (!invincible)
			{
				animator.SetBool("Hit", true);
				life -= damage;
				Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
				rb.velocity = Vector2.zero;
				rb.AddForce(damageDir * 10);
				if (life <= 0)
				{
					StartCoroutine(WaitToDead());
				}
				else
				{
					StartCoroutine(Stun(0.25f));
					StartCoroutine(MakeInvincible(1f));
				}
			}
		}

		IEnumerator DashCooldown()
		{
			animator.SetBool("IsDashing", true);
			isDashing = true;
			canDash = false;
			yield return new WaitForSeconds(0.1f);
			isDashing = false;
			yield return new WaitForSeconds(0.5f);
			canDash = true;
		}

		IEnumerator Stun(float time) 
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}
		IEnumerator MakeInvincible(float time) 
		{
			invincible = true;
			yield return new WaitForSeconds(time);
			invincible = false;
		}
		IEnumerator WaitToMove(float time)
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}

		IEnumerator WaitToCheck(float time)
		{
			canCheck = false;
			yield return new WaitForSeconds(time);
			canCheck = true;
		}

		IEnumerator WaitToEndSliding()
		{
			yield return new WaitForSeconds(0.1f);
			canDoubleJump = true;
			isWallSliding = false;
			animator.SetBool("IsWallSliding", false);
			oldWallSlidding = false;
			wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		}

		IEnumerator WaitToDead()
		{
			animator.SetBool("IsDead", true);
			canMove = false;
			invincible = true;
			playerAttack.enabled = false;
			yield return new WaitForSeconds(0.4f);
			rb.velocity = new Vector2(0, rb.velocity.y);
			yield return new WaitForSeconds(1.1f);
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
