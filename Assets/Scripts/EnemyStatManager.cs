using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatManager : MonoBehaviour
{
	/// <summary>
	/// battle
	/// Monster:0(scriptable object) -> stat -> UI
	/// </summary>

	[Header("Battle UI")]
	public Image Battle_Icon;
	public TextMeshProUGUI Battle_HP;
	public TextMeshProUGUI Battle_Job;

	[Header("Stat UI")]
	public TextMeshProUGUI statName;
	public TextMeshProUGUI statHP;
	public TextMeshProUGUI statAtk;
	public TextMeshProUGUI statAvoidRate;

	public Status[] Enemies;

	static public string Job;
	static public Sprite Icon;
	static public int Atk;
	static public int nowHP;
	static public int maxHP;
	static public int AvoidRate;

	public Slider HpBarSlider;

	// HP -> Slider(UI)
	public void CheckHp()
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

	public void OnClick_EnemyStatShowBtn()
	{
		// Name, HP, Atk, AvoidRate
		statName.text = Job.ToString() + "(��) " + "�������ͽ�";
		statHP.text = "���� ü��: " + nowHP.ToString();
		statAtk.text = "���ݷ�: " + Atk.ToString();
		statAvoidRate.text = "ȸ����" + AvoidRate.ToString();
	}
}
