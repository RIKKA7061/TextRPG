using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    /// <summary>
    /// StoryPlayManager���� StoryPlay() ����� case�� Reward�Ͻ�
    /// ���ݱ��� ������ story�迭 + �ʷϻ���� �� �κ� EX. ü��ȸ�� +10, ȸ���� 2���� (���� ȸ����: 8), �ִ�ü�� 5����
    /// �̷��� ���־�� �ڴ�.
    /// 
    /// switch�� �����ϰڴ�.
    /// Case�� �̷��� �ΰڴ�.
    /// ȸ��, ȸ���� ����, �ִ�ü�� ����
    /// 
    /// 
    /// </summary>
    /// 

    public PlayerStatManager playerStatManager;

    public string RewardAction(string Reward)
    {
        Debug.Log(Reward);
        string [] rewardArr = Reward.Split(':');
        string rewardType = rewardArr[0];
        int rewardNum = int.Parse(rewardArr[1]);
        string resultMessage;
        switch (rewardType)
        {
            case "ȸ��":
                PlayerStatManager.nowHP = PlayerStatManager.nowHP + rewardNum;
				resultMessage = $"ü�� +{rewardNum}";
                if (PlayerStatManager.nowHP > PlayerStatManager.maxHP) PlayerStatManager.nowHP = PlayerStatManager.maxHP;
				break;
			case "�ִ�ü�� ����":
				PlayerStatManager.maxHP = PlayerStatManager.maxHP + rewardNum;
                PlayerStatManager.nowHP = PlayerStatManager.nowHP + rewardNum;
                resultMessage = $"�ִ� ü�� +{rewardNum}";
				break;
			case "ȸ���� ����":
				PlayerStatManager.AvoidRate = PlayerStatManager.AvoidRate + rewardNum;
                resultMessage = $"ȸ���� +{rewardNum} (���� ȸ����: {PlayerStatManager.AvoidRate})";
				break;
            default:
				resultMessage = "�� �� ���� ���� Ÿ���Դϴ�.";
				break;
		}
		playerStatManager.SetUI(); // UI ����
		return resultMessage;
	}
}
