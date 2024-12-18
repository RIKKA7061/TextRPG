using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    /// <summary>
    /// 다음장으로 넘어가는 매니저
    /// 걍 계속하기 버튼이랑 똑같은데 누를 때 장수가 +1 되는 것이 핵심
    /// 어떻게 보면 (1,2,3)장 매니저 역할도 한다고 볼수도 있음
    /// </summary>
    /// 

    // 장
    public TextMeshProUGUI Jwang;
    public int JwangNum = 1;

	public StackTextManager stackTextManager;

	private void Start()
	{
		SetJwangUI();
	}

	public void EndBtnMaker()
	{
		JwangNum++; // 장 넘버 UP
		SetJwangUI();
		stackTextManager.AddButtonBelowLastText(2, "다음 장 스토리");
	}

	public void SetJwangUI()
	{
		Jwang.text = JwangNum.ToString() + "장"; // UI 갱신
	}
}
