using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : IPlayerState
{
    private PlayerController player; // PlayerController

    public PlayerState(PlayerController player)
    {
        this.player = player; // PlayerController �ʱ�ȭ
    }

    public void Enter()
    {
        player.animator.SetBool("Run", false); // Idle ���·� �ʱ�ȭ
        player.animator.SetBool("Jump", false);
    }

    public void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // �Է��� ���� �� �ִϸ��̼��� Idle�� (���� ���� �ƴ� ����)
        if (!player.IJumping())
        {
            if (moveInput != 0)
            {
                player.Move(moveInput); // ĳ���� �̵�
                player.animator.SetBool("Run", true); // Run �ִϸ��̼� ��ȯ
            }
            else
            {
                player.animator.SetBool("Run", false); // Idle ���·� ��ȯ
            }
        }
        else
        {
            // ���� �߿��� �¿� �̵� ���������� Run �ִϸ��̼��� ���� Jump �ִϸ��̼� ����
            if (moveInput != 0)
            {
                player.Move(moveInput); // ���߿��� �¿� �̵�
            }
            player.animator.SetBool("Run", false); // ���� �߿� Run �ִϸ��̼� ����
        }

        // ���� (WŰ)
        if (Input.GetKeyDown(KeyCode.W) && player.IGrounded())
        {
            player.Jump(); // ���� ó��
        }
    }

    public void Exit()
    { }
}
