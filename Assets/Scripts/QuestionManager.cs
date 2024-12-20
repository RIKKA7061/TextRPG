using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
	/// 선택지 내용 (1. 초대장을 조사한다.:2. 대극장을 조사한다.) 이런식으로
	/// 올시에 우선 ':' 기준으로 나눠서 Split을 하고 1번과 2번을
	/// 버튼을 생성한다. (여기까지가 목표)
	/// 
	/// 
	/// 버튼 클릭시 -> 다른 매니저 작동하게
	/// 
	public StoryPlayManager storyPlayManager;
	public StackTextManager stackTextManager;

	public void QuestionBtnMaker()
	{
		Debug.Log(DataManager.dictionary[storyPlayManager.storyFlow- 1].Value);
		string []questions = DataManager.dictionary[storyPlayManager.storyFlow - 1].Value.Split(':');

		Debug.Log($"Question Count: {questions.Length}");

		for(int i = 0;  i < questions.Length; i++)
		{
			stackTextManager.AddButtonBelowLastText(i+5, questions[i]);
		}

		//stackTextManager.AddButtonBelowLastText(0, questions[0]);
		//stackTextManager.AddButtonBelowLastText(1, questions[1]);
	}
}
