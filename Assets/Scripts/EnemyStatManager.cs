using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatManager : MonoBehaviour
{
	/// <summary>
	/// ���� �������ͽ��� ����� ���̿���.
	/// ������ ���� = �����ϱ� ������ ��
	/// �Ϲݸ�A:0 ���ڿ� �޾Ƽ� -> :�� ������.
	/// ���� enemystat���� <- 0�� ���� ���� �����Ѵ�.
	/// ��Ʋ UI�� �����Ű�� �Լ��� �����. <- ���߿� ������ �� �Լ� �ߵ� ���Ѽ� ����� ���� ������ UI�� ���.
	/// 
	/// �ϴ� ���� Player Stat Manager�� ���� �ٸ����� ����.
	/// �翬�� ���͵��� ��ũ���ͺ� ������Ʈ�� �����Ǹ�
	/// �迭�� ������ ���� �ȴ�.
	/// 
	/// ��, 
	/// 0���� = ���Ӱ�����
	/// 1���� = ����������
	/// 
	/// [] Status �ؼ� ����ġ�� �ϸ� �ɵ�
	/// </summary>
	/// 

	// ��Ʋ UI
	[Header("�� ��Ʋ UI")]
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
