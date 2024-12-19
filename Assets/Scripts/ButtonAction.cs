using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
	/// <summary>
	/// 1번 클릭시 (YesAction이라고 가정) 
	/// 1번에 있는 내용인 "초대장을 조사한다." 와
	/// 스토리 흐름에 있는 root1의 value값 들을 "당신은..."
	/// 전부 string 배열에 넣고 story 재생
	/// story 출력 후 계속하기 버튼 만들기
	/// 
	/// 동적 버튼 생성시 동적 버튼의 함수들의 집합체
	/// ex. 분기점 선택 버튼, 계속하기 버튼, 전투하기 버튼 등등
	/// 
	/// 분기점 선택 이후
	/// 내가 선택하게된 결과 스토리 출력 후 계속하기 버튼 생성
	/// 계속하기 버튼이란
	/// 다음 스토리 재생을 누르는 버튼과도 같다.
	/// 즉, StorePlayManager에 있는 StoryPlay() 함수를 실행하면 된다.
	/// 그러기 위해서는 우선 StoryPlay()함수 내에 Switch문에서
	/// Battle 부분을 손봐야된다.
	/// 왜냐하면 Battle의 역할이
	/// Story
	/// Story
	/// Story
	/// Battle 
	/// 이렇게 될때 한번에 묶어서 배열로 출력하게 하고
	/// 그다음에 Battle은 전투하기 버튼을 만들어주면 되기 때문이다.
	/// </summary>
	private StoryPlayManager storyPlayManager;
	private StackTextManager stackTextManager;
	private BattleManager battleManager;
	private EndManager endManager;

	private void Start()
	{
		storyPlayManager = FindObjectOfType<StoryPlayManager>();
		stackTextManager = FindObjectOfType<StackTextManager>();
		battleManager = FindObjectOfType<BattleManager>();
		endManager = FindObjectOfType<EndManager>();
	}

	public int count = 0;
	public string Content;
	public List<string> list = new List<string>();

	public void YesAction(string content)
	{
		Debug.Log($"{content} 버튼 클릭!");
		Content = content;
		// 예 버튼 클릭 시 동작

		// storyPlayManager.storyFlow = 스토리 순번
		Root1Counting();
	}

	public void Root1Counting()
	{
		if (DataManager.dictionary[storyPlayManager.storyFlow].Key == "Root1")
		{
			Debug.Log(DataManager.dictionary[storyPlayManager.storyFlow].Value);
			if (DataManager.dictionary[storyPlayManager.storyFlow].Value.Contains("&플레이어 직업&"))
			{
				DataManager.dictionary[storyPlayManager.storyFlow].Value 
					= DataManager.dictionary[storyPlayManager.storyFlow].Value.Replace("&플레이어 직업&", PlayerStatManager.Job);
			}

			list.Add(DataManager.dictionary[storyPlayManager.storyFlow].Value);
			count++;
			storyPlayManager.storyFlow++;
			Root1Counting();
		}
		else
		{
			// count  
			MakeBaeYeol();
		}
	}

	public void MakeBaeYeol()
	{
		string[] SendText = new string[count + 1]; // (Root1 내용들) + (1. 초대장을 조사한다.)
		SendText[0] = Content;
		for(int i = 1; i <= count; i++)
		{
			SendText[i] = list[i-1];
		}
        foreach (var item in SendText)
        {
			Debug.Log($"민수는 최고야: {item}");
        }
		list.Clear();

		StoryPlay(SendText);
	}

	public void StoryPlay(string[] SendText)
	{
		stackTextManager.Generate_Stack(0, SendText, "continue");
	}

	public void NoAction(string content)
	{
		Debug.Log($"{content} 버튼 클릭!");
		Content = content;
		// 아니오 버튼 클릭 시 동작

		// storyPlayManager.storyFlow = 스토리 순번
		Root2Counting();
	}

	public void Root2Counting()
	{
		if (DataManager.dictionary[storyPlayManager.storyFlow].Key == "Root2")
		{
			Debug.Log(DataManager.dictionary[storyPlayManager.storyFlow].Value);
			if (DataManager.dictionary[storyPlayManager.storyFlow].Value.Contains("&플레이어 직업&"))
			{
				DataManager.dictionary[storyPlayManager.storyFlow].Value
					= DataManager.dictionary[storyPlayManager.storyFlow].Value.Replace("&플레이어 직업&", PlayerStatManager.Job);
			}
			list.Add(DataManager.dictionary[storyPlayManager.storyFlow].Value);
			count++;
			storyPlayManager.storyFlow++;
			Root2Counting();
		}
		else if (DataManager.dictionary[storyPlayManager.storyFlow].Key == "Root1")
		{
			storyPlayManager.storyFlow++;
			Root2Counting();
		}
		else if (DataManager.dictionary[storyPlayManager.storyFlow].Key != "Root1" 
			&& DataManager.dictionary[storyPlayManager.storyFlow].Key != "Root2")
		{
			MakeBaeYeol();
		}
	}

	public void ContinueBtn()
	{
		// 계속하기 버튼 클릭 시 동작
		storyPlayManager.StoryPlay();
	}

	public void EndBtn()
	{
		// 계속하기 버튼 클릭 시 동작
		storyPlayManager.StoryPlay();
		endManager.SetJwangUI();
	}


	public void BattleBtn(string EnemyInfos)
	{
		// 버튼 클릭 시 동작
		Debug.Log("전투하기 버튼 클릭시 동작");
		battleManager.BattlePlay(EnemyInfos);
	}
}