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

    public int maxHp = 100;            // 최대 체력
    public int PresentHp;               // 현재 체력
    public int maxExp = 100;        // 최대 경험치
    public int PresentExp;           // 현재 경험치

    public int level;
    private int attackPower;                 // 공격력
    private int defensePower;                // 방어력
    private int gold;                        // 골드

    private void Start()
    {
        PresentHp = maxHp;         // 현재 체력을 최대 체력으로 초기화
        PresentExp = 0;              // 경험치를 0으로 초기화
        level = 1;

        expBar.value = 0;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // 데미지받으면 체력 감소
        PresentHp = Mathf.Clamp(PresentHp - damage, 0, maxHp);
        // 체력바 슬라이더 값
        HpBar.value = PresentHp;
    }

    public void GainExp(int experience)
    {
        PresentExp += experience; // 경험치 추가
        PresentExp = Mathf.Clamp(PresentExp, 0, maxExp); // 최대 경험치 클램프

        // 레벨업 체크
        CheckLevelUp(); // 레벨업 체크 호출

        // 경험치바 슬라이더 값
        expBar.value = PresentExp;
    }
    private void CheckLevelUp()
    {
        // 현재 경험치가 최대 경험치 이상일 때 레벨업
        while (PresentExp >= maxExp)
        {
            Debug.Log("Level up!");
            LevelUp(); // 레벨업 메서드 호출
        }
    }
    private void LevelUp()
    {
        level++; // 레벨 증가

        // 레벨업 시 공격력 및 방어력 증가
        attackPower += 5;
        defensePower += 2;

        // 최대 체력 증가
        maxHp += 10;
        PresentHp = Mathf.Clamp(PresentHp, 0, maxHp);
        UpdateStats(level, attackPower, defensePower, gold);
    }
    public void UpdateStats(int levelup ,int attack, int defense, int goldAmount)
    {
        // 레벨, 공격력, 방어력, 골드 값
        level = levelup;
        attackPower = attack;
        defensePower = defense;
        gold = goldAmount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        statusText.text = ($"레벨 : {level:000}\n공격력 : {attackPower:000}\n방어력 : {defensePower:000}\nGOLD : {gold:000}");
    }
}