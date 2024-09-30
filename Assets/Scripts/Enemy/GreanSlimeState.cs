using UnityEngine;

public class GreanSlimeState : ISlimeState
{
    private GreanSlimeController slimeController;
    private Transform player;   // �÷��̾� ������Ʈ Ʈ������
    private float attackRange = 1f; // ���� �Ÿ�

    public GreanSlimeState(GreanSlimeController controller)
    {
        slimeController = controller;
        player = GameObject.FindWithTag("Player").transform; // �÷��̾� ������Ʈ�� �±׷� ã��
    }

    public void Enter()
    {
        // ���� ���� �� �ִϸ��̼� �ʱ�
        slimeController.animator.SetBool("Attack", false);
    }

    public void Update()
    {
        // �����Ӱ� �÷��̾� ������ �Ÿ�
        float PlayerDistance = Vector3.Distance(slimeController.transform.position, player.position);

        // �÷��̾ �׾����� üũ
        if (player.CompareTag("Die"))
        {
            return;
        }
        // �÷��̾���� �Ÿ� üũ
        if (PlayerDistance > attackRange)
        {
            slimeController.MoveAi(player.position); // �÷��̾� ������ �̵�
        }
        else
        {
            if (!slimeController.IAttack())  // �������� ���������� ������
            {
                slimeController.Attack(); // ���� �ִϸ��̼� ����
            }
        }
    }

    public void Exit()
    { }
}