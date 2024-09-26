using System.Collections;
using UnityEngine;

public class GreanSlimeController : MonoBehaviour
{
    public Animator animator; // �ִϸ����� ������Ʈ
    private ISlimeState currentState; // ���� ����
    public float moveSpeed = 2f; // ������ �̵� �ӵ�
    private bool iAttack = false;   // ���� ������ ����

    private void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        ChangeState(new GreanSlimeState(this)); // �ʱ� ���� ����
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
        Vector3 direction = (playerPosition - transform.position).normalized;
        Move(direction.x); // �������� �÷��̾� ������ �̵�
    }

    public void Move(float moveInput)
    {
        // �̵� ó��
        if (!iAttack) // ���� ���� ���� �̵� false
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
        }
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.967f); // �ִϸ��̼� �ð��� ���� ����
        animator.SetBool("Attack", false); // ���� �ִϸ��̼� ����
        iAttack = false;    // ���� ���� ����
    }

    public bool IAttack()
    {
        return iAttack; // ĳ���Ͱ� ���� ������ ����
    }
}
