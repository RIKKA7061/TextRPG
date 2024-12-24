using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
	/// <summary>
	/// 
	/// player
	/// nowHP
	/// maxHP
	/// AvoidRate
	/// Scriptable Object -> static -> UI
	/// 
	/// </summary>

	public Slider HpBarSlider;

	[Header("Story UI")]
	public Image UI_Job_img;
	public TextMeshProUGUI HP;
	public TextMeshProUGUI NowCharacter_Job;

	[Header("Battle UI")]
	public Image Battle_Icon;
	public TextMeshProUGUI Battle_HP;
	public TextMeshProUGUI Battle_Job;

	[Header("Stat UI")]
	public TextMeshProUGUI statName;
	public TextMeshProUGUI statHP;
	public TextMeshProUGUI statAtk;
	public TextMeshProUGUI statAvoidRate;

	public StoryPlayManager storyPlayManager;

	public Status[] Characters;
	// 0 = 기자	 
	// 1 = 역사학자
	// 2 = 형사
	// 3 = 배우

	static public string Job;
	static public Sprite Icon;
	static public int Atk;
	static public int nowHP;
    static public int maxHP;
    static public int AvoidRate;

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



	public void SetStat(int i)
    {
		Job = Characters[i].Job;
		Icon = Characters[i].icon;
		Atk = Characters[i].Atk;
		nowHP = Characters[i].HP;
        maxHP = Characters[i].HP;
        AvoidRate = Characters[i].AvoidRate;
	}

	public void SetUI()
	{
		UI_Job_img.sprite = Icon;
		HP.text = nowHP + "/" + maxHP;
		NowCharacter_Job.text = Job;
	}

	public void SetPlayerBattleUI()
	{
		Battle_Icon.sprite = Icon;
		Battle_HP.text = nowHP + "/" + maxHP;
		Battle_Job.text = Job;
		CheckHp();
	}

	public void OnClick_StatShowBtn()
	{
		// Name, HP, Atk, AvoidRate
		statName.text = Job.ToString() + " " + "스테이터스";
		statHP.text = "현재 체력: " + nowHP.ToString();
		statAtk.text = "공격력: " + Atk.ToString();
		statAvoidRate.text = "회피율" + AvoidRate.ToString();
	}
}
