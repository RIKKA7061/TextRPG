using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class CharacterChoiceManager : MonoBehaviour
{
	// Button ������ (Unity �����Ϳ��� ����)
	public GameObject buttonPrefab;
	public Transform buttonContainer; // ��ư���� ���� �θ� ������Ʈ

	public StoryPlayManager storyPlayManager;

	public PlayerStatManager playerStatManager;

	public GameObject Character_Choice_Pannel;

	public DataManager dataManager;

	string[] Characters = { "����", "��������", "����", "���"};


	// ���丮 �Լ��� �� �Ű����� ��Ʈ
	private List<(int, int)> storyParams = new List<(int, int)>
	{
		(1, 2), // ����
        (3, 4), // ��������
        (5, 6), // ����
        (7, 8)  // ���
    };

	void Start()
	{
		CreateButtons();
	}

	void CreateButtons()
	{
		Vector2 startPosition = new Vector2(0, 0); // ���� ��ġ
		float buttonSpacing = 100f; // ��ư �� ����

		for (int i = 0; i < storyParams.Count; i++)
		{
			// ��ư ���� ����
			GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
			newButton.name = $"CharacterButton_{i + 1}";

			// ��ư �ؽ�Ʈ ����
			newButton.GetComponentInChildren<TextMeshProUGUI>().text = Characters[i];

			// ��ư ��ġ ����
			RectTransform rectTransform = newButton.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = startPosition - new Vector2(0, i * buttonSpacing);

			// �̺�Ʈ �Ҵ� (���� ǥ���� ���)
			int index = i; // ���� ĸó ���� ����
			newButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				StoryDownLoading(storyParams[index].Item1, storyParams[index].Item2);
			});
		}
	}

	// StoryDownLoading �Լ� (����)
	void StoryDownLoading(int param1, int param2)
	{
		Debug.Log($"�ش� ĳ������ ���丮 �Ľ� �۾� ���� - {param1}�� , {param2}��");
		int NowMode = param2 / 2 - 1; // 0,1,2,3 = ����,��������,����,���
		dataManager.StoryDownLoading(param1, param2);			// �ش� ĳ������ ���丮�� �Ľ��մϴ�.
		Debug.Log($"{NowMode} ���");						// ĳ���� ���
		playerStatManager.SetStat(NowMode);				// ĳ������ ������ ������ ���� �÷��̾��� �������� ����
		playerStatManager.SetUI();				// ĳ������ ���� �� ������ ui�� ���
		Character_Choice_Pannel.SetActive(false);				// ĳ���� ���� �Ƕ��� ����
		storyPlayManager.StoryPlay();   // ���丮 ���
		Debug.Log(Characters[NowMode] + "���丮�� ����մϴ�.");
	}
}
