using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Animator ������Ʈ
    private IPlayerState currentState; // ���� ����
    private float lastMove = 0; // ������ �̵� �Է�
    public float jumpForce = 5f; // ���� ��
    private Rigidbody2D rigid; // ������ٵ�
    private bool Grounded = true; // ĳ���Ͱ� ���� �ִ��� ���� Ȯ��
    private bool iJumping = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        ChangeState(new PlayerState(this)); // �ʱ� ���� ����
    }

    private void Update()
    {
        // ���� ���°� null�� �ƴ� ���� Update ȣ��
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // ���� ���� ����
        }

        currentState = newState; // ���ο� ���� ����
        currentState.Enter(); // ���ο� ���� ����
    }

    public void Move(float moveInput)
    {
        // �̵� ó��
        Vector2 move = new Vector2(moveInput, 0);
        transform.Translate(move * Time.deltaTime * 5f); // ������ �̵�

        // ���� ��ȯ�ϸ� ȸ��
        if (moveInput > 0 && lastMove <= 0)
        {
            // ���������� ���� ��ȯ
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.localScale.x == 1)
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (moveInput < 0 && lastMove >= 0)
        {
            // �������� ���� ��ȯ
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.localScale.x == -1)
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // ���� �̵� �Է� ����
        lastMove = moveInput;
    }

    public void Jump()
    {
        // ���� ó��
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        animator.SetBool("Jump", true); // ���� �ִϸ��̼� ����
        Grounded = false; // ���߿� ����
        iJumping = true; // ���� ��
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ĳ���Ͱ� ���� ������ ���� ���� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            iJumping = false;
            animator.SetBool("Jump", false); // ���� �ִϸ��̼� ����
        }
    }

    public bool IGrounded()
    {
        return Grounded; // ĳ���Ͱ� ���� �ִ��� ����
    }

    public bool IJumping()
    {
        return iJumping; // ĳ���Ͱ� ���� ������ ���� ��ȯ
    }
}