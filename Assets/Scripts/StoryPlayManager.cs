using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPlayManager : MonoBehaviour
{
	/// <summary>
	/// (ó���� or ���� ������)��ȣ�� �޾Ƽ� i(dictionary int) ���� -> category�� �Ǻ��Ѵ�. -> �Ǻ��Ͽ� generate �Ѵ�.
	/// 
	/// ���� ���丮 ���൵ ���� i
	/// end �Ͻÿ� ���� �� ��ư�� Ŭ���� ��� �� ���� ������. 1�� -> 2��
	/// 
	/// dicitionary 1 Story ���丮����
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
				Debug.Log("�� ������ ���丮�Դϴ�.");
				Debug.Log(StoryText);
				
				if(StoryText.Contains("&�÷��̾� ����&"))
				{
					DataManager.dictionary[storyFlow].Value = StoryText.Replace("&�÷��̾� ����&", PlayerStatManager.Job);
				}

				storyCount++;
				storyFlow++;
				StoryPlay();
				break;
			case "Question":
				Debug.Log("�� ������ �б��� �����Դϴ�.");
				storyCount++;
				storyFlow++;

				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				for (int i = 1; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}

				// �簥�� ������
				string SeonTaekJee = DataManager.dictionary[storyFlow - 1].Value;
				Debug.Log(SeonTaekJee);

				// ���� ����
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Question");
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
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
				// Story ���
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				for (int i = 0; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount +i].Value);
				}
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Battle");
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				storyCount = 0;
				break;
			case "Reward":
				// Story ���
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				for (int i = 0; i < storyCount; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}
				Debug.Log(DataManager.dictionary[storyFlow - 1].Value);
				

				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Reward");
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				storyCount = 0;
				break;
			case "End":
				// Story ���
				storyCount++;
				storyFlow++;
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				for (int i = 0; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "End");
				SendTEXT.Clear(); // ����Ʈ �ʱ�ȭ
				storyCount = 0;
				break;




			// �ʿ��� ��� �ٸ� ī�װ� �߰�
			default:
				Debug.Log($"�� �� ���� ī�װ�: {Category}");
				break;
		}
	}
}
