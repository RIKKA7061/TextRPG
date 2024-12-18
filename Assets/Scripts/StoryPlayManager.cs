using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPlayManager : MonoBehaviour
{
	/// <summary>
	/// (처음시 or 스택 끝날시)신호를 받아서 i(dictionary int) 번때 -> category를 판별한다. -> 판별하여 generate 한다.
	/// 
	/// 현재 스토리 진행도 변수 i
	/// end 일시에 다음 장 버튼을 클릭할 경우 장 수를 높여요. 1장 -> 2장
	/// 
	/// dicitionary 1 Story 스토리내용
	/// 
	/// 
	/// </summary>
	/// 
	public StackTextManager stackTextManager;
	public QuestionManager questionManager;
	public int storyFlow = 1;
	public int storyCount = 0;
	public List<string> SendTEXT = new List<string>();

	public void StoryPlay()
    {
		string Category = DataManager.dictionary[storyFlow].Key;
		string StoryText = DataManager.dictionary[storyFlow].Value;

		switch (Category)
		{
			case "Story":
				Debug.Log("이 내용은 스토리입니다.");
				Debug.Log(StoryText);
				
				if(StoryText.Contains("&플레이어 직업&"))
				{
					DataManager.dictionary[storyFlow].Value = StoryText.Replace("&플레이어 직업&", PlayerStatManager.Job);
				}

				storyCount++;
				storyFlow++;
				StoryPlay();
				break;
			case "Question":
				Debug.Log("이 내용은 분기점 제시입니다.");
				storyCount++;
				storyFlow++;

				SendTEXT.Clear(); // 리스트 초기화
				for (int i = 1; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}

				// 양갈래 선택지
				string SeonTaekJee = DataManager.dictionary[storyFlow - 1].Value;
				Debug.Log(SeonTaekJee);

				// 스택 생성
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Question");
				SendTEXT.Clear(); // 리스트 초기화
				storyCount = 0;
				break;
			case "Root1":
				storyFlow++;
				StoryPlay();
				break;
			case "Root2":
				storyFlow++;
				StoryPlay();
				break;
			case "Battle":
				// Story 출력
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // 리스트 초기화
				for (int i = 0; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount +i].Value);
				}
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Battle");
				SendTEXT.Clear(); // 리스트 초기화
				storyCount = 0;
				break;
			case "Reward":
				// Story 출력
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // 리스트 초기화
				for (int i = 0; i < storyCount; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}
				Debug.Log(DataManager.dictionary[storyFlow - 1].Value);
				

				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Reward");
				SendTEXT.Clear(); // 리스트 초기화
				storyCount = 0;
				break;
			case "End":
				// Story 출력
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // 리스트 초기화
				for (int i = 0; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "End");
				SendTEXT.Clear(); // 리스트 초기화
				storyCount = 0;
				break;




			// 필요한 경우 다른 카테고리 추가
			default:
				Debug.Log($"알 수 없는 카테고리: {Category}");
				break;
		}
	}
}
