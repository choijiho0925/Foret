using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _02.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f; // 부드러운 이동 수치
        [SerializeField] private bool canAirControl = true;                     // 공중에서 컨트롤 가능 여부
        [SerializeField] private LayerMask groundLayer;                         // 땅 레이어
        [SerializeField] private LayerMask wallLayer;                           //벽 레이어
        [SerializeField] private Transform groundCheck;                             // 땅 체크용 Transform
        [SerializeField] private Transform wallCheck;                               // 벽 체크용 Transform

        private const float GroundRadius = .2f;     // 땅 체크 범위
        private const float WallRadius = 0.2f;      //벽 체크 범위
        private bool isGrounded;            // 땅에 붙어있는지 여부
        private bool isFacingRight = true;  // 바라보는 방향
        private float limitFallSpeed = 15f; // 떨어지는 최대 스피드

        private bool canJump = true;    //점프 가능 여부
        private bool canDoubleJump; //더블 점프 가능 여부
        private bool canDash = true;    //대쉬 가능 여부
        private bool isDashing;         //대쉬중
        private bool isWall;            //벽에 붙음
        private bool isWallSliding;     //벽 슬라이딩 중
        private bool oldWallSliding;

        public bool canMove = true;

        private PlayerStat playerStat;
        private PlayerAttack playerAttack;
        private SpriteRenderer mainSprite;
        private Animator animator;
        private Rigidbody2D rb;
        private Vector3 velocity = Vector3.zero;

        [Header("점프 효과")]
        [SerializeField] private GameObject jumpEffect;

        [Header("대쉬 효과")]
        [SerializeField] private GameObject afterimagePrefab; // 잔상 프리팹
        [SerializeField] private float afterimageSpawnRate = 0.05f; // 잔상 생성 간격 (초)

        [Header("벽 점프 설정")]
        [SerializeField] private float wallJumpControlLockTime = 0.5f; // 벽 점프 후 조작이 잠기는 시간

        //애니메이션 ID
        private static readonly int animIDGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int animIDSpeed = Animator.StringToHash("Speed");
        private static readonly int animIDJumping = Animator.StringToHash("IsJumping");
        private static readonly int animIDDoubleJumping = Animator.StringToHash("IsDoubleJumping");
        private static readonly int animIDDashing = Animator.StringToHash("IsDashing");
        private static readonly int animIDWallSliding = Animator.StringToHash("IsWallSliding");
        private static readonly int animIDIsHit = Animator.StringToHash("IsHit");
        private static readonly int animIDIsDead = Animator.StringToHash("IsDead");

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            playerStat = GetComponent<PlayerStat>();
            playerAttack = GetComponent<PlayerAttack>();
            mainSprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;

            //땅 체크
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundRadius, groundLayer);

            // 착지한 순간에만 처리
            if (!wasGrounded && isGrounded)
            {
                animator.SetBool(animIDGrounded, true);
                // 착지 시 점프 상태 해제
                animator.SetBool(animIDJumping, false);
                animator.SetBool(animIDDoubleJumping, false);
                canJump = true;
                canDoubleJump = true;
            }
            // 땅에서 떨어진 순간에만 처리
            else if (wasGrounded && !isGrounded)
            {
                animator.SetBool(animIDGrounded, false);
            }

            isWall = false;

            if (!isGrounded)
            {
                //벽 체크
                if (Physics2D.OverlapCircle(wallCheck.position, WallRadius, wallLayer))
                {
                    isDashing = false;
                    isWall = true;
                }
            }
        }

        public void Move(float move, bool jump, bool dash)
        {
            if (canMove)
            {
                animator.SetFloat(animIDSpeed, Mathf.Abs(move));
                //플레이어 대쉬
                if (dash && canDash && !isWallSliding)
                {
                    if (playerStat.UseEnergy(10))
                    {
                        rb.AddForce(new Vector2(playerStat.CurrentDashForce * transform.localScale.x, 0f));
                        StartCoroutine(DashCooldown());
                    }
                }

                if (isDashing)
                {
                    rb.velocity = new Vector2(playerStat.CurrentDashForce * transform.localScale.x, 0);
                }

                //플레이어가 땅에 붙어있거나 공중 제어 가능 상태일때만 움직일 수 있게
                else if (isGrounded || canAirControl)
                {
                    if (rb.velocity.y < -limitFallSpeed)    //떨어지는 속도 제한
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
                if (jump && canJump)
                {
                    animator.SetBool(animIDJumping, true);
                    canJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0f, playerStat.CurrentJumpForce));
                    canDoubleJump = true;
                    StartCoroutine(ShowJumpEffect());
                }   //더블 점프
                else if (jump && canDoubleJump && !isWallSliding)
                {
                    animator.SetBool(animIDDoubleJumping, true);
                    canDoubleJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0f, playerStat.CurrentJumpForce / 1.2f));
                }

                else if (isWall && !isGrounded)
                {
                    // 벽 슬라이딩 '시작' 조건: 아직 슬라이딩 중이 아니고, 아래로 떨어지고 있을 때
                    if (!isWallSliding)
                    {
                        isWallSliding = true;
                        oldWallSliding = true; // oldWallSlidding을 여기서 true로 설정하여 중복 진입 방지
                        canDoubleJump = true; // 벽에 붙으면 더블 점프 기회 획득
                        animator.SetBool(animIDJumping, false); // 점프 애니메이션 종료
                        animator.SetBool(animIDWallSliding, true); // 벽 슬라이딩 애니메이션 시작
                    }

                    // '벽 슬라이딩 중'일 때의 물리 및 조작 처리
                    if (isWallSliding)
                    {
                        // 벽 반대 방향으로 키를 입력하면 슬라이딩 상태 해제
                        if (move * transform.localScale.x < -0.1f)
                        {
                            isWallSliding = false; // 즉시 상태 해제
                            animator.SetBool(animIDWallSliding, false);
                        }
                        else // 가만히 있거나 벽 방향으로 키를 입력하면
                        {
                            // 부드럽게 미끄러지도록 속도 제어
                            rb.velocity = new Vector2(0, -2f);
                        }

                        // 벽 슬라이딩 중 벽 점프
                        if (jump)
                        {
                            isWallSliding = false;
                            animator.SetBool(animIDWallSliding, false);
                            animator.SetBool(animIDJumping, true);

                            rb.velocity = Vector2.zero; // 점프 전 속도 초기화
                            Flip(); // 방향 전환
                            rb.AddForce(new Vector2(transform.localScale.x * playerStat.CurrentJumpForce * 0.5f, playerStat.CurrentJumpForce));

                            StartCoroutine(ShowJumpEffect());
                            // 점프 직후 잠시 조작을 막음
                            StartCoroutine(WaitToMove(wallJumpControlLockTime));
                        }
                        // 벽 슬라이딩 중 대쉬
                        else if (dash && canDash)
                        {
                            if (playerStat.UseEnergy(10))
                            {
                                isWallSliding = false;
                                animator.SetBool(animIDWallSliding, false);
                                StartCoroutine(DashCooldown());
                            }
                        }
                    }
                }
                // 벽에서 떨어졌을 때 '슬라이딩 상태 종료'
                else if (isWallSliding && !isWall)
                {
                    isWallSliding = false;
                    animator.SetBool(animIDWallSliding, false);
                }

                //  isWallSliding이 false로 바뀌는 모든 경우에 대비한 최종 안전장치
                if (!isWallSliding && oldWallSliding)
                {
                    oldWallSliding = false; // 상태 플래그 초기화
                                            //canMove = true; // 이동 가능 상태로 복구
                }
            }
        }

        public void Stop()  //플레이어 대화, 사망 시 움직임을 멈추는 메서드
        {
            rb.velocity = Vector2.zero;
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private IEnumerator ShowJumpEffect()
        {
            jumpEffect.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            jumpEffect.SetActive(false);
        }

        //대쉬 코루틴
        private IEnumerator DashCooldown()
        {
            animator.SetBool(animIDDashing, true);
            isDashing = true;
            canDash = false;
            playerStat.isInvincible = true; //대쉬 중 무적

            // 이팩트 생성 코루틴 시작
            //dashEffect.SetActive(true);				//일반 대쉬 효과
            StartCoroutine(SpawnAfterimages()); //잔상 효과

            yield return new WaitForSeconds(0.1f);	//실제 대쉬가 지속되는 시간
            playerStat.isInvincible = false;
            isDashing = false;
            animator.SetBool(animIDDashing, false);
            yield return new WaitForSeconds(0.5f);
            canDash = true;
        }

        // 잔상을 생성하는 코루틴
        private IEnumerator SpawnAfterimages()
        {
            // isDashing이 true인 동안 계속 반복
            while (isDashing)
            {
                // 잔상 프리팹을 현재 플레이어 위치에 생성
                GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);

                // 생성된 잔상의 스프라이트를 현재 플레이어의 스프라이트와 동일하게 설정
                SpriteRenderer imageRenderer = afterimage.GetComponent<SpriteRenderer>();
                imageRenderer.sprite = mainSprite.sprite;

                // 플레이어의 방향(좌우 뒤집힘)도 잔상에 그대로 적용
                imageRenderer.flipX = !(transform.localScale.x > 0);

                // 정해진 시간만큼 대기
                yield return new WaitForSeconds(afterimageSpawnRate);
            }
        }

        private IEnumerator Stun(float time)
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            canMove = true;
        }

        private IEnumerator WaitToMove(float time)
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            canMove = true;
        }

        private IEnumerator WaitToEndSliding()
        {
            yield return new WaitForSeconds(0.1f);
            canDoubleJump = true;
            isWallSliding = false;
            animator.SetBool(animIDWallSliding, false);
            oldWallSliding = false;
            wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
        }

        private IEnumerator WaitToDead()
        {
            animator.SetBool(animIDIsDead, true);
            canMove = false;
            //invincible = true;
            playerAttack.enabled = false;
            yield return new WaitForSeconds(0.4f);
            rb.velocity = new Vector2(0, rb.velocity.y);
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
