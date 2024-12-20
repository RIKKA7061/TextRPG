using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
	/// ������ ���� (1. �ʴ����� �����Ѵ�.:2. ������� �����Ѵ�.) �̷�������
	/// �ýÿ� �켱 ':' �������� ������ Split�� �ϰ� 1���� 2����
	/// ��ư�� �����Ѵ�. (��������� ��ǥ)
	/// 
	/// 
	/// ��ư Ŭ���� -> �ٸ� �Ŵ��� �۵��ϰ�
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
