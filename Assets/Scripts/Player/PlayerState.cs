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
        player.animator.SetBool("Roll", false);
        player.animator.SetBool("Attack", false);
    }

    public void Update()
    {
        // �÷��̾ �׾��� �� �˹�����϶� �ൿ�Ұ�
        if (player.IDead() || player.IKnockedBack())
        {
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");

        // �Է��� ���� �� �ִϸ��̼��� Idle��
        if (!player.IJumping() && !player.IRolling() && !player.IAttacking())
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

        // ���� (�����̽��� Ű)
        if (Input.GetKeyDown(KeyCode.Space) && player.IGrounded() && !player.IRolling() && !player.IAttacking())
        {
            player.Jump(); // ���� ó��
        }

        // ������ (ShiftŰ)
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.IGrounded() && !player.IAttacking())
        {
            player.Roll(); // ������ ó��
        }
        // ���� (�����̽���)
        if (Input.GetMouseButtonDown(0) && !player.IRolling())
        {
            player.Attack(); // ���� ó��
        }
    }
    public void Exit()
    { }
}
