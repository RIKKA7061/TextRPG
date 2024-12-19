using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPlayManager : MonoBehaviour
{
	public StackTextManager stackTextManager;
	public QuestionManager questionManager;
	public int storyFlow = 1;
	public int storyCount = 0;
	public List<string> SendTEXT = new List<string>();

	public void StoryPlay()
	{
		if (!DataManager.dictionary.ContainsKey(storyFlow))
		{
			Debug.Log($"Key '{storyFlow}' not found in the dictionary.");
			Debug.Log("There is no more story");
			return;//End
		}

		string Category = DataManager.dictionary[storyFlow].Key;
		string StoryText = DataManager.dictionary[storyFlow].Value;

		switch (Category)
		{
			case "Story":
				if (StoryText.Contains("&�÷��̾� ����&"))
				{
					DataManager.dictionary[storyFlow].Value = StoryText.Replace("&�÷��̾� ����&", PlayerStatManager.Job);
				}
				storyCount++;
				storyFlow++;
				StoryPlay();
				break;
			case "Question":
				storyCount++;
				storyFlow++;
				SendTEXT.Clear();
				for (int i = 1; i < storyCount - 1; i++)
				{
					SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
				}
				stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), "Question");
				SendTEXT.Clear();
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
				SendTextToStack("Battle", 1);
				break;
			case "Reward":
				SendTextToStack("Reward",0);
				break;
			case "End":
				SendTextToStack("End", 1);
				break;
			default:
				// Exceptions
				Debug.Log($"Unknown category: {Category}");
				break;
		}
	}

	public void SendTextToStack(string Case, int Subtraction)
	{
		storyCount++;
		storyFlow++;
		SendTEXT.Clear();
		for (int i = 0; i < storyCount - Subtraction; i++)
		{
			SendTEXT.Add(DataManager.dictionary[storyFlow - storyCount + i].Value);
		}
		stackTextManager.Generate_Stack(0, SendTEXT.ToArray(), Case);
		SendTEXT.Clear();
		storyCount = 0;
	}
}
