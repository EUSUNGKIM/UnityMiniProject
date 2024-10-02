using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int levelup = 1;
    public int attackDamage = 20; // 공격력
    private int defense = 10; // 방어력
    private int gold = 50; // 골드

    public float attackRange = 1f; // 공격 범위 설정

    public bool iKnockedBack = false; // 넉백 중인지 여부
    public float knockBackForce = 5f; // 넉백 힘
    public float knockBackDuration = 0.5f; // 넉백 지속 시간

    public Animator animator; // Animator 컴포넌트
    private IPlayerState playerState; // 현재 상태
    private float lastMove = 0; // 마지막 이동 입력
    public float jumpForce = 4f; // 점프 힘
    public float rollSpeed = 1f; // 구르기 속도
    private Rigidbody2D rigid; // 리지드바디
    private bool Grounded = true; // 캐릭터가 땅에 있는지 여부 확인
    private bool iMove = true; // 캐릭터가 움직이는 중인지 여부
    private bool iJumping = false; // 점프 중인지 여부
    private bool iRolling = false; // 구르기 중인지 여부
    private bool iAttacking = false; // 공격 중인지 여부
    private bool idead = false; // 플레이어가 죽었는지 여부
    public PlayerUI playerUI;  // UI
    private Collider2D playerCollider;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        playerCollider = GetComponent<Collider2D>(); // Collider2D 컴포넌트 가져오기
        ChangeState(new PlayerState(this)); // 초기 상태 설정
        playerUI = FindObjectOfType<PlayerUI>();  // PlayerUI 찾기
        playerUI.UpdateStats(levelup, attackDamage, defense, gold);
    }

    private void Update()
    {
        // 현재 상태가 null이 아닐 때만 Update 호출
        if (playerState != null)
        {
            playerState.Update();
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (playerState != null)
        {
            playerState.Exit(); // 이전 상태 종료
        }

        playerState = newState; // 새로운 상태 설정
        playerState.Enter(); // 새로운 상태 진입
    }

    public void Move(float moveInput)
    {
        if (iMove)
        {
            // 이동 처리
            Vector2 move = new Vector2(moveInput, 0);
            transform.Translate(move * Time.deltaTime * 5f); // 프레임 이동

            // 방향 전환하면 회전
            if (moveInput > 0 && lastMove <= 0)
            {
                // 오른쪽으로 방향 전환
                transform.localScale = new Vector3(1, 1, 1);
                if (transform.localScale.x == 1)
                {
                    transform.position += new Vector3(1, 0, 0);
                }
            }
            else if (moveInput < 0 && lastMove >= 0)
            {
                // 왼쪽으로 방향 전환
                transform.localScale = new Vector3(-1, 1, 1);
                if (transform.localScale.x == -1)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            // 현재 이동 입력 저장
            lastMove = moveInput;
        }
    }

    public void Jump()
    {
        // 점프 처리
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        animator.SetBool("Jump", true); // 점프 애니메이션 시작
        Grounded = false; // 공중에 있음
        iJumping = true; // 점프 중
    }

    public void Roll()
    {
        if (!iRolling)  // 구르기 중이 아닐 때만
        {
            iRolling = true; // 구르기 상태
            animator.SetBool("Roll", true); // 구르기 애니메이션 시작
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            float rollDirection = transform.localScale.x; // 현재 바라보는 방향으로 구르기
            rigid.velocity = new Vector2(rollSpeed * rollDirection, rigid.velocity.y); // 구르기 속도 반영
            // 구르기는 계속 땅에 닿아있어 따로 애니메이션 종료를 위한 코루틴이 필요(살짝 뛰우는 방법도 있지만 본인은 이게더편함)
            
            StartCoroutine(EndRoll());
        }
    }
    private IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(1f); // 구르기 애니메이션 시간
        iRolling = false; // 구르기 상태 해제
        animator.SetBool("Roll", false); // 구르기 애니메이션 종료
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Attack()
    {
        if (!iAttacking) // 공격 중이 아닐 때만
        {
            
            iAttacking = true; // 공격 상태로 변경

            if (iJumping)   // 점프상태
            {
                animator.SetBool("JumpAttack", true);   // 점프 공격 애니메이션 시작
            }
            else
            {
                iMove = false;  // 제자리 공격에선 움직일 수 없음
                animator.SetBool("Attack", true); // 공격 애니메이션 시작
            }

            StartCoroutine(EndAttack());
            StartCoroutine(DealDamageAfterDelay());
        }
    }
    

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.667f); // 애니메이션 시간에 맞춰 조정
        animator.SetBool("Attack", false); // 공격 애니메이션 종료
        animator.SetBool("JumpAttack", false);
        iAttacking = false; // 공격 상태 해제
        iMove = true; // 이동 상태
    }
    private IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(0.267f); // 공격이 적중하는 맞춰 딜레이 조정

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            GreanSlimeController slime = enemy.GetComponent<GreanSlimeController>();
            if (slime != null)
            {
                slime.TakeDamage(attackDamage); // 슬라임에게 데미지 주기
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 캐릭터가 땅에 닿으면 점프 상태 해제
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            iJumping = false;
            animator.SetBool("Jump", false); // 점프 애니메이션 종료
        }
    }

    // 피격 처리 메서드
    public void TakeDamage(int damage)
    {
        // 구르기 중에는 데미지를 받지 않음
        if (!iRolling)
        {
            // 데미지
            playerUI.TakeDamage(damage - defense);

            // 사망
            if (playerUI.PresentHp <= 0)
            {
                Die();
            }

            // 넉백 처리
            if (!iKnockedBack) // 넉백 중이 아닐 때만
            {
                // 넉백 상태 전에 다른 애니메이션 끔
                animator.SetBool("Run", false);
                animator.SetBool("Jump", false);

                StartCoroutine(KnockBack());
            }
        }

    }
    public IEnumerator KnockBack()
    {
        iKnockedBack = true; // 넉백 상태 설정
        animator.SetBool("Hit", true); // 피격 애니메이션 시작

        // 캐릭터의 방향에 따라 넉백 방향
        Vector2 knockBackDirection;

        if (transform.localScale.x > 0)
        {
            knockBackDirection = Vector2.left; // 왼쪽으로 넉백
        }
        else
        {
            knockBackDirection = Vector2.right; // 오른쪽으로 넉백
        }
        rigid.velocity = knockBackDirection * knockBackForce;

        yield return new WaitForSeconds(knockBackDuration); // 넉백 지속 시간 대기

        iKnockedBack = false; // 넉백 상태 해제
        animator.SetBool("Hit", false); // 피격 애니메이션 종료
    }
    public void Die()
    {
        animator.SetBool("Die", true); // die 애니메이션 실행
        StartCoroutine(EndGame());
        gameObject.tag = "Die";
        idead = true;// die true
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f); // 1초 대기
        Application.Quit(); // 게임 종료
    }

    // 경험치 획득
    public void GainExp(int exp)
    {
        Debug.Log(playerUI.PresentExp);
        playerUI.GainExp(exp);
    }
    
    // GOLD 획득
    public void GetGold(int amount)
    {
        gold += amount; // 골드 증가
        /*playerUI.UpdateStats(levelup, attackDamage, defense, gold);*/
    }
    public void UpdatePlayerStats(int levelup, int attack, int defense, int goldAmount)
    {
        levelup = playerUI.level;
        attackDamage = playerUI.attackPower;
        defense = playerUI.defensePower;
        gold = playerUI.gold;

        playerUI.UpdateStats(levelup, attackDamage, defense, gold); // UI 업데이트
    }
    public bool IGrounded()
    {
        return Grounded; // 캐릭터가 땅에 있는지 여부
    }

    public bool IJumping()
    {
        return iJumping; // 캐릭터가 점프 중인지 여부
    }
    public bool IRolling()
    {
        return iRolling; // 캐릭터가 구르기 중인지 여부
    }
    public bool IAttacking()
    {
        return iAttacking; // 캐릭터가 공격 중인지 여부
    }
    public bool IDead ()
    {
        return idead; // 캐릭터가 죽었는지 여부
    }
    public bool IKnockedBack() // 넉백 여부
    {
        return iKnockedBack;
    }
}