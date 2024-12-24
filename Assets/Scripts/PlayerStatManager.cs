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
	// 0 = ����	 
	// 1 = ��������
	// 2 = ����
	// 3 = ���

	static public string Job;
	static public Sprite Icon;
	static public int Atk;
	static public int nowHP;
    static public int maxHP;
    static public int AvoidRate;

	public void CheckHp() //*HP ����
	{
		if (maxHP > 0)
		{
			float clampedNowHP = Mathf.Clamp(nowHP, 0, maxHP); // nowHP ���� ����
			HpBarSlider.value = (float)clampedNowHP / (float)maxHP;
		}
		else
		{
			HpBarSlider.value = 0; // maxHP�� 0�� �� �����̴� �� �⺻�� ����
			Debug.LogWarning("maxHP�� 0�Դϴ�. �������� ������ �� �����ϴ�.");
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
		statName.text = Job.ToString() + " " + "�������ͽ�";
		statHP.text = "���� ü��: " + nowHP.ToString();
		statAtk.text = "���ݷ�: " + Atk.ToString();
		statAvoidRate.text = "ȸ����" + AvoidRate.ToString();
	}
}
