using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatManager : MonoBehaviour
{
	/// <summary>
	/// 적의 스테이터스를 만드는 일이에요.
	/// 실행할 조건 = 전투하기 눌렀을 때
	/// 일반몹A:0 문자열 받아서 -> :로 나눈다.
	/// 기존 enemystat에서 <- 0번 몬스터 스텟 적용한다.
	/// 배틀 UI에 적용시키는 함수도 만든다. <- 나중에 전투시 이 함수 발동 시켜서 적용된 몬스터 스텟을 UI에 쏜다.
	/// 
	/// 하는 일은 Player Stat Manager와 거의 다른점이 없다.
	/// 당연히 몬스터들은 스크립터블 오브젝트로 관리되며
	/// 배열의 순번에 따라서 된다.
	/// 
	/// 즉, 
	/// 0번때 = 게임개발자
	/// 1번때 = 보안전문가
	/// 
	/// [] Status 해서 끌어치기 하면 될듯
	/// </summary>
	/// 

	// 배틀 UI
	[Header("적 배틀 UI")]
	public Image Battle_Icon;
	public TextMeshProUGUI Battle_HP;
	public TextMeshProUGUI Battle_Job;

	public Status[] Enemies;

	static public string Job;
	static public Sprite Icon;
	static public int Atk;
	static public int nowHP;
	static public int maxHP;
	static public int AvoidRate;

	public Slider HpBarSlider;

	public void CheckHp() //*HP 갱신
	{
		if (maxHP > 0)
		{
			float clampedNowHP = Mathf.Clamp(nowHP, 0, maxHP); // nowHP 범위 제한
			HpBarSlider.value = (float)clampedNowHP / (float)maxHP;
		}
		else
		{
			HpBarSlider.value = 0; // maxHP가 0일 때 슬라이더 값 기본값 설정
			Debug.LogWarning("maxHP가 0입니다. 나눗셈을 수행할 수 없습니다.");
		}
	}

	public void SetEnemyStat(int i)
	{
		Job = Enemies[i].Job;
		Icon = Enemies[i].icon;
		Atk = Enemies[i].Atk;
		nowHP = Enemies[i].HP;
		maxHP = Enemies[i].HP;
		AvoidRate = Enemies[i].AvoidRate;
	}

	public void SetEnemeyBattleUI()
	{
		Battle_Icon.sprite = Icon;
		Battle_HP.text = nowHP + "/" + maxHP;
		Battle_Job.text = Job;
		CheckHp();
	}
}
