using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : IPlayerState
{
    private PlayerController player; // PlayerController

    public PlayerState(PlayerController player)
    {
        this.player = player; // PlayerController 초기화
    }

    public void Enter()
    {
        player.animator.SetBool("Run", false); // Idle 상태로 초기화
        player.animator.SetBool("Jump", false);
        player.animator.SetBool("Roll", false);
        player.animator.SetBool("Attack", false);
    }

    public void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // 입력이 없을 때 애니메이션을 Idle로
        if (!player.IJumping() && !player.IRolling() && !player.IAttacking())
        {
            if (moveInput != 0)
            {
                player.Move(moveInput); // 캐릭터 이동
                player.animator.SetBool("Run", true); // Run 애니메이션 전환
            }
            else
            {
                player.animator.SetBool("Run", false); // Idle 상태로 전환
            }
        }
        else
        {
            // 점프 중에도 좌우 이동 가능하지만 Run 애니메이션은 끄고 Jump 애니메이션 유지
            if (moveInput != 0)
            {
                player.Move(moveInput); // 공중에서 좌우 이동
            }
            player.animator.SetBool("Run", false); // 점프 중엔 Run 애니메이션 끄기
        }

        // 점프 (W키)
        if (Input.GetKeyDown(KeyCode.W) && player.IGrounded() && !player.IRolling() && !player.IAttacking())
        {
            player.Jump(); // 점프 처리
        }

        // 구르기 (Shift키)
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.IGrounded() && !player.IAttacking())
        {
            player.Roll(); // 구르기 처리
        }
        // 공격 (스페이스바)
        if (Input.GetKeyDown(KeyCode.Space) && !player.IRolling())
        {
            player.Attack(); // 공격 처리
        }
    }

    public void Exit()
    { }
}
