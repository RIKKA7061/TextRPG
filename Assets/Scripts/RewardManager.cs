using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    /// <summary>
    /// StoryPlayManager에서 StoryPlay() 재생후 case가 Reward일시
    /// 지금까지 저장한 story배열 + 초록색깔로 한 부분 EX. 체력회복 +10, 회피율 2증가 (현재 회피율: 8), 최대체력 5증가
    /// 이렇게 해주어야 겠다.
    /// 
    /// switch로 관리하겠다.
    /// Case를 이렇게 두겠다.
    /// 회복, 회피율 증가, 최대체력 증가
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
            case "회복":
                PlayerStatManager.nowHP = PlayerStatManager.nowHP + rewardNum;
                Debug.Log($"체력이 {rewardNum}만큼 회복되었어요.");
				resultMessage = $"체력이 {rewardNum}만큼 회복되었어요.";
                if (PlayerStatManager.nowHP > PlayerStatManager.maxHP) PlayerStatManager.nowHP = PlayerStatManager.maxHP;
				break;
			case "최대체력 증가":
				PlayerStatManager.maxHP = PlayerStatManager.maxHP + rewardNum;
                PlayerStatManager.nowHP = PlayerStatManager.nowHP + rewardNum;
				Debug.Log($"최대 체력이 {rewardNum}만큼 늘어나게 되었어요.");
                resultMessage = $"최대 체력이 {rewardNum}만큼 늘어나게 되었어요.";
				break;
			case "회피율 증가":
				PlayerStatManager.AvoidRate = PlayerStatManager.AvoidRate + rewardNum;
				Debug.Log($"회피율이 {rewardNum} 상승하게 되었어요.");
                resultMessage = $"회피율이 {rewardNum} 상승하게 되었어요. (현재 회피율: {PlayerStatManager.AvoidRate})";
				break;
            default:
				resultMessage = "알 수 없는 보상 타입입니다.";
				break;
		}
		playerStatManager.SetUI(); // UI 갱신
		return resultMessage;
	}
}
