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
        player.animator.SetBool("Run", false); // Idle ���·� ����
    }

    public void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // �Է��� ���� �� �ִϸ��̼��� Idle��
        if (moveInput != 0)
        {
            player.Move(moveInput); // ĳ���� �̵�
            player.animator.SetBool("Run", true); // �ִϸ��̼��� Run ���·� ��ȯ
        }
        else
        {
            player.animator.SetBool("Run", false); // �ִϸ��̼��� false ���·� ��ȯ(Idle�� ��ȯ)
        }
    }

    public void Exit()
    { }
}
