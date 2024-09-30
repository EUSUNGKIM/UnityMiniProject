using System.Collections;
using UnityEngine;

public class GreanSlimeController : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    private ISlimeState currentState; // 현재 상태
    public float moveSpeed = 2f; // 슬라임 이동 속도
    private bool iAttack = false;   // 공격 중인지 여부
    public int attackDamage = 10; // 슬라임 공격력
    private PlayerController player;

    private void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        ChangeState(new GreanSlimeState(this)); // 초기 상태 설정
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // 현재 상태가 null이 아닐 때만 Update 호출
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(ISlimeState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // 이전 상태 종료
        }
        currentState = newState; // 새로운 상태로 변경
        currentState.Enter(); // 새로운 상태 초기화
    }

    public void MoveAi(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        Move(direction.x); // 슬라임을 플레이어 쪽으로 이동
    }

    public void Move(float moveInput)
    {
        // 이동 처리
        if (!iAttack) // 공격 중일 때와 넉백 중일 때는 이동 false
        {
            Vector2 move = new Vector2(moveInput, 0);
            transform.Translate(move * Time.deltaTime * moveSpeed); // 이동

            // 방향 전환
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // 오른쪽으로 방향 전환
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // 왼쪽으로 방향 전환
            }
        }
    }

    public void Attack()
    {
        // 공격
        if (!iAttack)
        {
            iAttack = true;
            animator.SetBool("Attack", true); // 공격 애니메이션 실행
            StartCoroutine(EndAttack()); // 공격 종료 후 상태 변경
            StartCoroutine(DealDamageAfterDelay()); // 모션에 맞춰서 데미지 입히기
        }
    }
    private IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 애니메이션과 맞춰 조정
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= 1f)
        {
            player.TakeDamage(attackDamage); // 플레이어에게 데미지 주기
        }
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.967f); // 애니메이션 시간에 맞춰 조정
        animator.SetBool("Attack", false); // 공격 애니메이션 종료
        iAttack = false;    // 공격 상태 해제
    }
    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 슬라임이 플레이어와 충돌했을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
        }
    }

    public bool IAttack()
    {
        return iAttack;
    }
}
