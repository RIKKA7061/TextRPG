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
		string []AB = DataManager.dictionary[storyPlayManager.storyFlow - 1].Value.Split(':');
		string A = AB[0];
		string B = AB[1];

		stackTextManager.AddButtonBelowLastText(0, A);
		stackTextManager.AddButtonBelowLastText(1, B);
	}
}
