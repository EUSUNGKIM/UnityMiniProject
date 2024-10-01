using System.Collections;
using UnityEngine;

public class GreanSlimeController : MonoBehaviour
{
    public Animator animator;
    private ISlimeState currentState; // ���� ����
    public float moveSpeed = 2f; // ������ �̵� �ӵ�
    private bool iAttack = false;   // ���� ������ ����
    private bool iHit = false;  // �ǰ� ������ ����
    public int attackDamage = 10; // ������ ���ݷ�
    public int maxHp = 50; // ������ �ִ� ü��
    private int presentHp; // ������ ���� ü��
    private PlayerController player;

    private void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        presentHp = maxHp; // ������ ü�� �ʱ�ȭ
        ChangeState(new GreanSlimeState(this)); // �ʱ� ���� ����
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // ���� ���°� null�� �ƴ� ���� Update ȣ��
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(ISlimeState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // ���� ���� ����
        }
        currentState = newState; // ���ο� ���·� ����
        currentState.Enter(); // ���ο� ���� �ʱ�ȭ
    }

    public void MoveAi(Vector3 playerPosition)
    {
        if (!iHit)  // �ǰ� ���� �� �̵����� ����
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            Move(direction.x);
        }
    }

    public void Move(float moveInput)
    {
        // �̵� ó��
        if (!iAttack && !iHit) // ���� ���� ���� �ǰ� ���� ���� �̵� false
        {
            Vector2 move = new Vector2(moveInput, 0);
            transform.Translate(move * Time.deltaTime * moveSpeed); // �̵�

            // ���� ��ȯ
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // ���������� ���� ��ȯ
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // �������� ���� ��ȯ
            }
        }
    }

    public void Attack()
    {
        // ����
        if (!iAttack)
        {
            iAttack = true;
            animator.SetBool("Attack", true); // ���� �ִϸ��̼� ����
            StartCoroutine(EndAttack()); // ���� ���� �� ���� ����
            StartCoroutine(DealDamageAfterDelay()); // ��ǿ� ���缭 ������ ������
        }
    }
    private IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // �ִϸ��̼ǰ� Ÿ�̹� ���߱�
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= 1f)
        {
            player.TakeDamage(attackDamage); // �÷��̾�� ������ �ֱ�
        }
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.967f); // �ִϸ��̼� �ð��� ���� ����
        animator.SetBool("Attack", false); // ���� �ִϸ��̼� ����
        iAttack = false;    // ���� ���� ����
    }
    // �浹 ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �������� �÷��̾�� �浹���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
        }
    }
    // �ǰ� ó��
    public void TakeDamage(int damage)
    {
        if (!iHit)
        {
            presentHp -= damage; // ü�� ����

            if (iAttack)
            {
                iAttack = false; // ���� ���� ����
                animator.SetBool("Attack", false); // ���� �ִϸ��̼� ����
            }
            if (presentHp <= 0)
            {
                Die(); // ü���� 0 ���ϰ� �Ǹ� ���� ó��
            }
            else
            {
                animator.SetBool("Attack", false);
                StartCoroutine(HitAction()); // �ǰ� �ִϸ��̼� �� ó��
            }
        }
    }

    private IEnumerator HitAction()
    {
        iHit = true; // �ǰ� ����
        animator.SetBool("Hit", true); // �ǰ� �ִϸ��̼� ����

        yield return new WaitForSeconds(0.684f); // �ǰ� �ִϸ��̼� �ð� ����
        animator.SetBool("Hit", false); // �ǰ� �ִϸ��̼� ����
        iHit = false; // �ǰ� ���� ����
    }

    private void Die()
    {
        animator.SetBool("Die", true); // Die �ִϸ��̼� ����
        Destroy(gameObject, 1.6f); // �ִϸ��̼� ������ ������Ʈ ����
    }
    public bool IAttack()
    {
        return iAttack;
    }
}
