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

    private int maxHp = 100;            // �ִ� ü��
    private int PresentHp;               // ���� ü��
    private int maxExp = 100;        // �ִ� ����ġ
    private int PresentExp;           // ���� ����ġ

    private int attackPower;                 // ���ݷ�
    private int defensePower;                // ����
    private int gold;                        // ���

    private void Start()
    {
        PresentHp = maxHp;         // ���� ü���� �ִ� ü������ �ʱ�ȭ
        PresentExp = 0;              // ����ġ�� 0���� �ʱ�ȭ

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // �������� �޾� ü�� ����
        PresentHp = Mathf.Clamp(PresentHp - damage, 0, maxHp);

        // ü�¹� �����̴� ���� ����
        HpBar.value = (float)PresentHp / maxHp;
    }

    public void GainExp(int experience)
    {
        // ����ġ�� �޾� ���� ����ġ ����
        PresentExp = Mathf.Clamp(PresentExp + experience, 0, maxExp);

        // ����ġ�� �����̴� ���� ����
        expBar.value = (float)PresentExp / maxExp;
    }

    public void UpdateStats(int attack, int defense, int goldAmount)
    {
        // ���ݷ�, ����, ��� ��
        attackPower = attack;
        this.defensePower = defense;
        gold = goldAmount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        statusText.text = $"���ݷ�: {attackPower:000}\n����: {defensePower:000}\nGOLD: {gold:000}";
    }
}