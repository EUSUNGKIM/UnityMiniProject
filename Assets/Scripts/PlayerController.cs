using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트
    private IPlayerState playerState; // 현재 상태
    private float lastMove = 0; // 마지막 이동 입력
    public float jumpForce = 5f; // 점프 힘
    public float rollSpeed = 1f; // 구르기 속도
    private Rigidbody2D rigid; // 리지드바디
    private bool Grounded = true; // 캐릭터가 땅에 있는지 여부 확인
    private bool iMove = true; // 캐릭터가 움직이는 중인지 여부
    private bool iJumping = false; // 점프 중인지 여부
    private bool iRolling = false; // 구르기 중인지 여부
    private bool iAttacking = false; // 공격 중인지 여부

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        ChangeState(new PlayerState(this)); // 초기 상태 설정
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
    }

    public void Attack()
    {
        if (!iAttacking) // 공격 중이 아닐 때만
        {
            iMove = false;
            iAttacking = true; // 공격 상태로 변경
            animator.SetBool("Attack", true); // 공격 애니메이션 시작
            
            // 공격 로직 추가 (예: 적에게 데미지 주기)
            StartCoroutine(EndAttack());
        }
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.667f); // 애니메이션 시간에 맞춰 조정
        animator.SetBool("Attack", false); // 공격 애니메이션 종료
        iAttacking = false; // 공격 상태 해제
        iMove = true; // 이동 상태
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
}