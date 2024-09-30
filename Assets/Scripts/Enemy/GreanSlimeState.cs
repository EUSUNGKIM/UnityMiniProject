using UnityEngine;

public class GreanSlimeState : ISlimeState
{
    private GreanSlimeController slimeController;
    private Transform player;   // 플레이어 오브젝트 트렌스폼
    private float attackRange = 1f; // 공격 거리

    public GreanSlimeState(GreanSlimeController controller)
    {
        slimeController = controller;
        player = GameObject.FindWithTag("Player").transform; // 플레이어 오브젝트를 태그로 찾기
    }

    public void Enter()
    {
        // 상태 진입 시 애니메이션 초기
        slimeController.animator.SetBool("Attack", false);
    }

    public void Update()
    {
        // 슬라임과 플레이어 사이의 거리
        float PlayerDistance = Vector3.Distance(slimeController.transform.position, player.position);

        // 플레이어가 죽었는지 체크
        if (player.CompareTag("Die"))
        {
            return;
        }
        // 플레이어와의 거리 체크
        if (PlayerDistance > attackRange)
        {
            slimeController.MoveAi(player.position); // 플레이어 쪽으로 이동
        }
        else
        {
            if (!slimeController.IAttack())  // 슬라임이 공격중이지 않으면
            {
                slimeController.Attack(); // 공격 애니메이션 실행
            }
        }
    }

    public void Exit()
    { }
}