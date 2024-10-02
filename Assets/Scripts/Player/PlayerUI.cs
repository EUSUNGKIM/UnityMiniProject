using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider HpBar;                // ü�¹�
    public Slider expBar;            // ����ġ��
    public TextMeshProUGUI statusText;      // ���ݷ�, ����, ��� �ؽ�Ʈ

    public int maxHp = 100;            // �ִ� ü��
    public int PresentHp;               // ���� ü��
    public int maxExp = 100;        // �ִ� ����ġ
    public int PresentExp;           // ���� ����ġ

    public int level;
    public int attackPower;                 // ���ݷ�
    public int defensePower;                // ����
    public int gold;                        // ���

    private void Start()
    {
        PresentHp = maxHp;         // ���� ü���� �ִ� ü������ �ʱ�ȭ
        PresentExp = 0;              // ����ġ�� 0���� �ʱ�ȭ
        level = 1;

        expBar.value = 0;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // ������������ ü�� ����
        PresentHp = Mathf.Clamp(PresentHp - damage, 0, maxHp);
        // ü�¹� �����̴� ��
        HpBar.value = PresentHp;
    }

    public void GainExp(int experience)
    {
        PresentExp += experience; // ����ġ �߰�
        // ������ üũ
        if (PresentExp >= maxExp)
        {
            LevelUp();
            PresentExp -= maxExp;
        }
        expBar.value = PresentExp;
    }
    
    private void LevelUp()
    {
        level++; // ���� ����

        // ������ �� ���ݷ� �� ���� ����
        attackPower += 5;
        defensePower += 2;

        // �ִ� ü�� ����
        maxHp += 10;
        PresentHp = maxHp;
        UpdateStats(level, attackPower, defensePower, gold);
    }
    public void UpdateStats(int levelup ,int attack, int defense, int goldAmount)
    {
        // ����, ���ݷ�, ����, ��� ��
        level = levelup;
        attackPower = attack;
        defensePower = defense;
        gold = goldAmount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        statusText.text = ($"���� : {level:000}\n���ݷ� : {attackPower:000}\n���� : {defensePower:000}\nGOLD : {gold:000}");
    }
}