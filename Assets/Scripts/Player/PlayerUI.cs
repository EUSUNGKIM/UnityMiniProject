using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider HpBar;                // 체력바
    public Slider expBar;            // 경험치바
    public TextMeshProUGUI statusText;      // 공격력, 방어력, 골드 텍스트

    private int maxHp = 100;            // 최대 체력
    private int PresentHp;               // 현재 체력
    private int maxExp = 100;        // 최대 경험치
    private int PresentExp;           // 현재 경험치

    private int attackPower;                 // 공격력
    private int defensePower;                // 방어력
    private int gold;                        // 골드

    private void Start()
    {
        PresentHp = maxHp;         // 현재 체력을 최대 체력으로 초기화
        PresentExp = 0;              // 경험치를 0으로 초기화

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // 데미지를 받아 체력 감소
        PresentHp = Mathf.Clamp(PresentHp - damage, 0, maxHp);

        // 체력바 슬라이더 값을 갱신
        HpBar.value = (float)PresentHp / maxHp;
    }

    public void GainExp(int experience)
    {
        // 경험치를 받아 현재 경험치 갱신
        PresentExp = Mathf.Clamp(PresentExp + experience, 0, maxExp);

        // 경험치바 슬라이더 값을 갱신
        expBar.value = (float)PresentExp / maxExp;
    }

    public void UpdateStats(int attack, int defense, int goldAmount)
    {
        // 공격력, 방어력, 골드 값
        attackPower = attack;
        this.defensePower = defense;
        gold = goldAmount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        statusText.text = $"공격력: {attackPower:000}\n방어력: {defensePower:000}\nGOLD: {gold:000}";
    }
}