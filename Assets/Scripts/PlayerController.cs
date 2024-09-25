using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트
    private IPlayerState currentState; // 현재 상태
    private float lastMove = 0; // 마지막 이동 입력

    private void Start()
    {
        ChangeState(new PlayerState(this)); // 초기 상태 설정
    }

    private void Update()
    {
        currentState?.Update(); // 현재 상태가 null이 아닐 때만 Update 호출
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // 이전 상태 종료
        }

        currentState = newState; // 새로운 상태 설정
        currentState.Enter(); // 새로운 상태 진입
    }

    public void Move(float moveInput)
    {
        // 이동 처리
        Vector2 move = new Vector2(moveInput, 0);
        transform.Translate(move * Time.deltaTime * 5f); // 프레임 이동

        // 방향 전환하면 회전
        if (moveInput > 0 && lastMove <= 0)
        {
            // 오른쪽으로 방향 전환
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.localScale.x == 1)
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (moveInput < 0 && lastMove >= 0)
        {
            // 왼쪽으로 방향 전환
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.localScale.x == -1)
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // 현재 이동 입력 저장
        lastMove = moveInput;
    }
}