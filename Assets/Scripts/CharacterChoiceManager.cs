using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoiceManager : MonoBehaviour
{
	public GameObject buttonPrefab;
	public Transform buttonContainer;

	public StoryPlayManager storyPlayManager;

	public PlayerStatManager playerStatManager;

	public GameObject Character_Choice_Pannel;

	public DataManager dataManager;
	private ShopManager shopManager; // cache
	private ShopUIManager shopUIManager;

	// JOB name
	string[] Characters = { "기자", "역사학자", "형사", "배우" };

	public List<(int, int)> storyParams = new List<(int, int)>
	{
		(1, 2), // reporter story
        (3, 4), // historian story
        (5, 6), // detector story
        (7, 8)  // actor story
    };

	void Start()
	{
		shopManager = FindAnyObjectByType<ShopManager>();
		shopUIManager = FindAnyObjectByType<ShopUIManager>();
		CreateButtons();
	}

	void CreateButtons()
	{
		Vector2 startPosition = new Vector2(0, 0); // startPos
		float buttonSpacing = 120f; // button between space

		RectTransform containerRectTransform = buttonContainer.GetComponent<RectTransform>();// container Rect
		float containerWidth = containerRectTransform.rect.width;// container Rect width

		for (int i = 0; i < storyParams.Count; i++)
		{
			// generate button
			GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
			newButton.name = $"CharacterButton_{i + 1}";

			// button text
			newButton.GetComponentInChildren<TextMeshProUGUI>().text = Characters[i];

			// button Pos
			RectTransform rectTransform = newButton.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = startPosition - new Vector2(0, i * buttonSpacing);

			// button width
			rectTransform.sizeDelta = new Vector2(containerWidth, rectTransform.sizeDelta.y);

			// get ACTION to button
			int index = i; // 0 1 2... storyParams.Count
			newButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				StoryDownLoading(storyParams[index].Item1, storyParams[index].Item2);
			});
		}
	}

	void StoryDownLoading(int param1, int param2)
	{
		int index = param2 / 2 - 1;
		Debug.Log($"{Characters[index]} story");

		if (ShopManager.isRelease[index] == true)
		{
			StartCoroutine(StoryPlayCoroutine(param1, param2));
		}
		else
		{
			if (ShopManager.coins >= 1)
			{
				shopManager.SubtractMoney();
				ShopManager.isRelease[index] = true;
				shopManager.SaveIsRelease();
				Debug.Log("Buy a new STORY!!!");

				StartCoroutine(StoryPlayCoroutine(param1, param2));
			}
			else
			{
				Debug.Log("UnReleased!");
				Debug.Log($"Left Money: {ShopManager.coins}");
				shopUIManager.ShowMoneyIssue();
			}
		}
	}

	//void StoryPlay(int param1, int param2)
	//{
	//	//Debug.Log($"Parsing: {param1} , {param2}");
	//	int NowMode = param2 / 2 - 1; // 0,1,2,3 EACH JOB
	//	dataManager.StoryDownLoading(param1, param2); // Start Parsing 
	//	//Debug.Log($"{NowMode} Mode"); // choiced Character
	//	playerStatManager.SetStat(NowMode); // PlayerStat Update
	//	playerStatManager.SetUI(); // UI Update
	//	Character_Choice_Pannel.SetActive(false); // OFF Character Choice Window
	//	storyPlayManager.StoryPlay();   // Story Play
	//	//Debug.Log(Characters[NowMode] + "Story Play");
	//}
	// Change the method to return IEnumerator for coroutine
	IEnumerator StoryPlayCoroutine(int param1, int param2)
	{
		// Wait for 1 second before continuing
		yield return new WaitForSeconds(1f);

		// Now execute the rest of the logic
		int NowMode = param2 / 2 - 1; // 0,1,2,3 EACH JOB
		dataManager.StoryDownLoading(param1, param2); // Start Parsing 
													  //Debug.Log($"{NowMode} Mode"); // choiced Character
		playerStatManager.SetStat(NowMode); // PlayerStat Update
		playerStatManager.SetUI(); // UI Update
		Character_Choice_Pannel.SetActive(false); // OFF Character Choice Window
		storyPlayManager.StoryPlay();   // Story Play
										//Debug.Log(Characters[NowMode] + "Story Play");
	}

	// Call this method to start the coroutine
	void StartStoryPlay(int param1, int param2)
	{
		StartCoroutine(StoryPlayCoroutine(param1, param2));
	}

}