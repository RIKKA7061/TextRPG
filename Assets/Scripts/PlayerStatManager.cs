using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
	/// <summary>
	/// 플레이어
	/// 현재 체력 nowHP
	/// 최대 체력 maxHP
	/// 회피율    AvoidRate
	/// 이것들을 신호 받아서 해당 직업 스크립터블 오브젝트의 값에다가 전달해줍니다.
	/// 그리고 그 스탯을 UI로 나타내줍니다.
	/// 
	/// </summary>
	/// 

	// 스토리 UI
	[Header("스토리 UI")]
	public Image UI_Job_img;
	public TextMeshProUGUI HP;
	public TextMeshProUGUI NowCharacter_Job;

	// 배틀 UI
	[Header("배틀 UI")]
	public Image Battle_Icon;
	public TextMeshProUGUI Battle_HP;
	public TextMeshProUGUI Battle_Job;

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
	}
}
