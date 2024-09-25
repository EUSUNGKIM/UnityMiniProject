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

    private void Start()
    {
        ChangeState(new PlayerState(this)); // �ʱ� ���� ����
    }

    private void Update()
    {
        currentState?.Update(); // ���� ���°� null�� �ƴ� ���� Update ȣ��
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
}