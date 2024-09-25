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
        player.animator.SetBool("Run", false); // Idle 상태로 진입
    }

    public void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // 입력이 없을 때 애니메이션을 Idle로
        if (moveInput != 0)
        {
            player.Move(moveInput); // 캐릭터 이동
            player.animator.SetBool("Run", true); // 애니메이션을 Run 상태로 전환
        }
        else
        {
            player.animator.SetBool("Run", false); // 애니메이션을 false 상태로 전환(Idle로 전환)
        }
    }

    public void Exit()
    { }
}
