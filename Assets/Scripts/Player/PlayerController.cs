using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int levelup = 1;
    public int attackDamage = 20; // ���ݷ�
    private int defense = 10; // ����
    private int gold = 50; // ���

    public float attackRange = 1f; // ���� ���� ����

    public bool iKnockedBack = false; // �˹� ������ ����
    public float knockBackForce = 5f; // �˹� ��
    public float knockBackDuration = 0.5f; // �˹� ���� �ð�

    public Animator animator; // Animator ������Ʈ
    private IPlayerState playerState; // ���� ����
    private float lastMove = 0; // ������ �̵� �Է�
    public float jumpForce = 4f; // ���� ��
    public float rollSpeed = 1f; // ������ �ӵ�
    private Rigidbody2D rigid; // ������ٵ�
    private bool Grounded = true; // ĳ���Ͱ� ���� �ִ��� ���� Ȯ��
    private bool iMove = true; // ĳ���Ͱ� �����̴� ������ ����
    private bool iJumping = false; // ���� ������ ����
    private bool iRolling = false; // ������ ������ ����
    private bool iAttacking = false; // ���� ������ ����
    private bool idead = false; // �÷��̾ �׾����� ����
    public PlayerUI playerUI;  // UI
    private Collider2D playerCollider;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        playerCollider = GetComponent<Collider2D>(); // Collider2D ������Ʈ ��������
        ChangeState(new PlayerState(this)); // �ʱ� ���� ����
        playerUI = FindObjectOfType<PlayerUI>();  // PlayerUI ã��
        playerUI.UpdateStats(levelup, attackDamage, defense, gold);
    }

    private void Update()
    {
        // ���� ���°� null�� �ƴ� ���� Update ȣ��
        if (playerState != null)
        {
            playerState.Update();
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (playerState != null)
        {
            playerState.Exit(); // ���� ���� ����
        }

        playerState = newState; // ���ο� ���� ����
        playerState.Enter(); // ���ο� ���� ����
    }

    public void Move(float moveInput)
    {
        if (iMove)
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

    public void Jump()
    {
        // ���� ó��
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        animator.SetBool("Jump", true); // ���� �ִϸ��̼� ����
        Grounded = false; // ���߿� ����
        iJumping = true; // ���� ��
    }

    public void Roll()
    {
        if (!iRolling)  // ������ ���� �ƴ� ����
        {
            iRolling = true; // ������ ����
            animator.SetBool("Roll", true); // ������ �ִϸ��̼� ����
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            float rollDirection = transform.localScale.x; // ���� �ٶ󺸴� �������� ������
            rigid.velocity = new Vector2(rollSpeed * rollDirection, rigid.velocity.y); // ������ �ӵ� �ݿ�
            // ������� ��� ���� ����־� ���� �ִϸ��̼� ���Ḧ ���� �ڷ�ƾ�� �ʿ�(��¦ �ٿ�� ����� ������ ������ �̰Դ�����)
            
            StartCoroutine(EndRoll());
        }
    }
    private IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(1f); // ������ �ִϸ��̼� �ð�
        iRolling = false; // ������ ���� ����
        animator.SetBool("Roll", false); // ������ �ִϸ��̼� ����
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Attack()
    {
        if (!iAttacking) // ���� ���� �ƴ� ����
        {
            
            iAttacking = true; // ���� ���·� ����

            if (iJumping)   // ��������
            {
                animator.SetBool("JumpAttack", true);   // ���� ���� �ִϸ��̼� ����
            }
            else
            {
                iMove = false;  // ���ڸ� ���ݿ��� ������ �� ����
                animator.SetBool("Attack", true); // ���� �ִϸ��̼� ����
            }

            StartCoroutine(EndAttack());
            StartCoroutine(DealDamageAfterDelay());
        }
    }
    

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.667f); // �ִϸ��̼� �ð��� ���� ����
        animator.SetBool("Attack", false); // ���� �ִϸ��̼� ����
        animator.SetBool("JumpAttack", false);
        iAttacking = false; // ���� ���� ����
        iMove = true; // �̵� ����
    }
    private IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(0.267f); // ������ �����ϴ� ���� ������ ����

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            GreanSlimeController slime = enemy.GetComponent<GreanSlimeController>();
            if (slime != null)
            {
                slime.TakeDamage(attackDamage); // �����ӿ��� ������ �ֱ�
            }
        }
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

    // �ǰ� ó�� �޼���
    public void TakeDamage(int damage)
    {
        // ������ �߿��� �������� ���� ����
        if (!iRolling)
        {
            // ������
            playerUI.TakeDamage(damage - defense);

            // ���
            if (playerUI.PresentHp <= 0)
            {
                Die();
            }

            // �˹� ó��
            if (!iKnockedBack) // �˹� ���� �ƴ� ����
            {
                // �˹� ���� ���� �ٸ� �ִϸ��̼� ��
                animator.SetBool("Run", false);
                animator.SetBool("Jump", false);

                StartCoroutine(KnockBack());
            }
        }

    }
    public IEnumerator KnockBack()
    {
        iKnockedBack = true; // �˹� ���� ����
        animator.SetBool("Hit", true); // �ǰ� �ִϸ��̼� ����

        // ĳ������ ���⿡ ���� �˹� ����
        Vector2 knockBackDirection;

        if (transform.localScale.x > 0)
        {
            knockBackDirection = Vector2.left; // �������� �˹�
        }
        else
        {
            knockBackDirection = Vector2.right; // ���������� �˹�
        }
        rigid.velocity = knockBackDirection * knockBackForce;

        yield return new WaitForSeconds(knockBackDuration); // �˹� ���� �ð� ���

        iKnockedBack = false; // �˹� ���� ����
        animator.SetBool("Hit", false); // �ǰ� �ִϸ��̼� ����
    }
    public void Die()
    {
        animator.SetBool("Die", true); // die �ִϸ��̼� ����
        StartCoroutine(EndGame());
        gameObject.tag = "Die";
        idead = true;// die true
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f); // 1�� ���
        Application.Quit(); // ���� ����
    }

    // ����ġ ȹ��
    public void GainExp(int exp)
    {
        Debug.Log(playerUI.PresentExp);
        playerUI.GainExp(exp);
    }
    
    // GOLD ȹ��
    public void GetGold(int amount)
    {
        gold += amount; // ��� ����
        /*playerUI.UpdateStats(levelup, attackDamage, defense, gold);*/
    }
    public void UpdatePlayerStats(int levelup, int attack, int defense, int goldAmount)
    {
        levelup = playerUI.level;
        attackDamage = playerUI.attackPower;
        defense = playerUI.defensePower;
        gold = playerUI.gold;

        playerUI.UpdateStats(levelup, attackDamage, defense, gold); // UI ������Ʈ
    }
    public bool IGrounded()
    {
        return Grounded; // ĳ���Ͱ� ���� �ִ��� ����
    }

    public bool IJumping()
    {
        return iJumping; // ĳ���Ͱ� ���� ������ ����
    }
    public bool IRolling()
    {
        return iRolling; // ĳ���Ͱ� ������ ������ ����
    }
    public bool IAttacking()
    {
        return iAttacking; // ĳ���Ͱ� ���� ������ ����
    }
    public bool IDead ()
    {
        return idead; // ĳ���Ͱ� �׾����� ����
    }
    public bool IKnockedBack() // �˹� ����
    {
        return iKnockedBack;
    }
}