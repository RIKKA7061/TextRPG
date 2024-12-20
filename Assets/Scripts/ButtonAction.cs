using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
	private StoryPlayManager storyPlayManager;
	private StackTextManager stackTextManager;
	private BattleManager battleManager;
	private EndManager endManager;
	private RestartManager restartManager;
	private void Start()
	{
		storyPlayManager = FindObjectOfType<StoryPlayManager>();
		stackTextManager = FindObjectOfType<StackTextManager>();
		battleManager = FindObjectOfType<BattleManager>();
		endManager = FindObjectOfType<EndManager>();
		restartManager = FindObjectOfType<RestartManager>();
	}

	public int count = 0;
	public string Content;
	public List<string> list = new List<string>();

	public void QuestionChoiceAction(string content, int i)
	{
		Debug.Log($"{content} btn Clicked!");
		Content = content;

		QuestionAction(i);
	}

	//public void NoAction(string content)
	//{
	//	Debug.Log($"{content} 버튼 클릭!");
	//	Content = content;
	//	// 아니오 버튼 클릭 시 동작

	//	// storyPlayManager.storyFlow = 스토리 순번
	//	Root2Counting();
	//}

	public void QuestionAction(int btnType) // 5 6 7 8 = root1, root2, root3, root4
	{
		string Case = DataManager.dictionary[storyPlayManager.storyFlow - 1].Key;

		string nowRoot = "Minsu is the best";

		switch (btnType)
		{
			case 5:
				nowRoot = "Root1";
				break;
			case 6:
				nowRoot = "Root2";
				break;
			case 7:
				nowRoot = "Root3";
				break;
			case 8:
				nowRoot = "Root4";
				break;
		}
		RootCounting(nowRoot);
	}

	public void RootCounting(string nowRoot)
	{
		Debug.Log(DataManager.dictionary[storyPlayManager.storyFlow].Key);

		string Case = DataManager.dictionary[storyPlayManager.storyFlow].Key; // Root1, Root2, Root3, Rooot4

		if (Case == nowRoot) // Root1 == Root1
		{
			if (DataManager.dictionary[storyPlayManager.storyFlow].Value.Contains("&플레이어 직업&"))
			{
				DataManager.dictionary[storyPlayManager.storyFlow].Value
					= DataManager.dictionary[storyPlayManager.storyFlow].Value.Replace("&플레이어 직업&", PlayerStatManager.Job);
			}
			list.Add(DataManager.dictionary[storyPlayManager.storyFlow].Value);
			count++;
			storyPlayManager.storyFlow++;
			RootCounting(nowRoot);
		}
		else if (Case != nowRoot && Case != "Story")
		{
			storyPlayManager.storyFlow++;
			RootCounting(nowRoot);
		}
		else if (Case == "Story")
		{
			MakeBaeYeol();
		}
	}

	//public void Root1Counting()
	//{
	//	if (DataManager.dictionary[storyPlayManager.storyFlow].Key == "Root1")
	//	{
	//		if (DataManager.dictionary[storyPlayManager.storyFlow].Value.Contains("&플레이어 직업&"))
	//		{
	//			DataManager.dictionary[storyPlayManager.storyFlow].Value 
	//				= DataManager.dictionary[storyPlayManager.storyFlow].Value.Replace("&플레이어 직업&", PlayerStatManager.Job);
	//		}

	//		list.Add(DataManager.dictionary[storyPlayManager.storyFlow].Value);
	//		count++;
	//		storyPlayManager.storyFlow++;
	//		Root1Counting();
	//	}
	//	else
	//	{
	//		// count  
	//		MakeBaeYeol();
	//	}
	//}

	//public void Root2Counting()
	//{
	//	string Case = DataManager.dictionary[storyPlayManager.storyFlow].Key;
	//	if (Case == "Root2")
	//	{
	//		if (DataManager.dictionary[storyPlayManager.storyFlow].Value.Contains("&플레이어 직업&"))
	//		{
	//			DataManager.dictionary[storyPlayManager.storyFlow].Value
	//				= DataManager.dictionary[storyPlayManager.storyFlow].Value.Replace("&플레이어 직업&", PlayerStatManager.Job);
	//		}
	//		list.Add(DataManager.dictionary[storyPlayManager.storyFlow].Value);
	//		count++;
	//		storyPlayManager.storyFlow++;
	//		Root2Counting();
	//	}
	//	else if (Case == "Root1")
	//	{
	//		storyPlayManager.storyFlow++;
	//		Root2Counting();
	//	}
	//	else if (Case != "Root1" && Case != "Root2")
	//	{
	//		MakeBaeYeol();
	//	}
	//}

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

	public void RestartBtn()
	{
		restartManager.RestartBtnAction();
	}
}