using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StackTextManager : MonoBehaviour
{
	public RectTransform parentRect; // �θ� RectTransform
	public TextMeshProUGUI textPrefab; // TextMeshPro ������
	static public string[] storyArray = new string[100]; // ���丮 �迭
	public ScrollRect scrollRect; // ScrollRect ������Ʈ
	public RectTransform contentRect; // ScrollRect�� ������ ����
	public QuestionManager questionManager;
	public ButtonAction buttonActions;
	public ContinueManager continueManager;
	public BattleManager battleManager;
	public StoryPlayManager storyPlayManager;
	

	[Header("��ư ������")]
	public GameObject[] buttonPrefabs; // ������ ��ư ������ 0(��) 1(�ƴϿ�)
	public float buttonSpacing = 20f; // ��ư�� ������ �ؽ�Ʈ �� ����

	[Header("�ؽ�Ʈ �� ���� ����")]
	public float textSpacing = 10f; // �ؽ�Ʈ �� ����

	[Header("�ؽ�Ʈ ���� ��� �� ����")]
	public float term = 0.5f;

	[Header("Ÿ���� ȿ�� ������")]
	public float typingSpeed = 0.05f; // Ÿ���� ������

	private float currentYPosition = 0f; // �ؽ�Ʈ ��ġ ��ġ

	public void StoryGenerate_AfterMakeBtns(int colorNum)
	{
		StartCoroutine(Make_Story_Btn(colorNum));
	}
	public void MakeStory_earnHP(int colorNum)
	{
		StartCoroutine(Make_Story_earnHP(colorNum));
	}

	public void MakeStory_loseHP(int colorNum)
	{
		StartCoroutine(Make_Story_loseHP(colorNum));
	}

	public void Generate_Stack(int colorNum, string[] SendText, string Case) // 0 �Ͼ�, 1 �ʷ�, 2 ����
	{
		StartCoroutine(Stack(colorNum, SendText, Case));
	}

	public IEnumerator Make_Story_Btn(int colorNum)
	{
		//�ʱ�ȭ(���� �ؽ�Ʈ ����)
		foreach (Transform child in contentRect)
		{
			Destroy(child.gameObject);
		}
		currentYPosition = 0f;

		// ���丮 �迭 ��ȸ
		foreach (string story in storyArray)
		{
			yield return StartCoroutine(AddTextWithTypingEffect(colorNum, story));
			yield return new WaitForSeconds(term); // �ؽ�Ʈ ��� �� ������
		}

		Debug.Log("���丮 ��");
	}

	public IEnumerator Make_Story_earnHP(int colorNum)
	{
		// ���丮 �迭 ��ȸ
		foreach (string story in storyArray)
		{
			//yield return StartCoroutine(AddTextWithTypingEffect(story, colorNum));
			yield return new WaitForSeconds(term); // �ؽ�Ʈ ��� �� ������
		}
		//YesNoBtnManager.earnHP();
	}

	public IEnumerator Make_Story_loseHP(int colorNum)
	{
		// ���丮 �迭 ��ȸ
		foreach (string story in storyArray)
		{
			//yield return StartCoroutine(AddTextWithTypingEffect(story, colorNum));
			yield return new WaitForSeconds(term); // �ؽ�Ʈ ��� �� ������
		}
		//YesNoBtnManager.loseHP();
	}

	// Make sure to reset contentRect height before adding any new elements
	public IEnumerator Stack(int colorNum, string[] SendTEXT, string Case)
	{
		// Y ��ġ �ʱ�ȭ �α�
		Debug.Log("Y ��ġ �ʱ�ȭ");

		// contentRect�� �ڽ� ��ü�� ��� �ı�
		foreach (Transform child in contentRect)
		{
			Destroy(child.gameObject);
		}

		// currentYPosition �ʱ�ȭ
		currentYPosition = 0f;
		lastYPosition = 0f;

		// Reset content height to 0
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 0f); // Set height to 0 initially

		// currentYPosition �ʱ�ȭ �� ���� ���
		Debug.Log($"currentYPosition �ʱ�ȭ ��: {currentYPosition}");

		// ���丮 �迭 ��ȸ
		foreach (string story in SendTEXT)
		{
			yield return StartCoroutine(AddTextWithTypingEffect(colorNum, story));
			yield return new WaitForSeconds(term); // �ؽ�Ʈ ��� �� ������
		}

		Debug.Log("��� �Ϸ�");

		// Case�� ���� ���� ó��
		switch (Case)
		{
			case "Question":
				questionManager.QuestionBtnMaker();
				break;
			case "continue":
				continueManager.ContinueBtnMaker();
				break;
			case "Battle":
				battleManager.BattleBtnMaker(DataManager.dictionary[storyPlayManager.storyFlow - 1].Value);
				break;
			default:
				break;
		}
	}



	private float lastYPosition = 0f; // �ؽ�Ʈ�� ������ Y ��ġ ����
	private float contentHeight = 0f;  // ��ü ������ ���� ����


	// string ���丮�� ������ �ѱ��ھ� Ÿ���� ���ִ� �Լ�
	private IEnumerator AddTextWithTypingEffect(int colorNum, string story)
	{
		// TMP ������ �ν��Ͻ� ����
		TextMeshProUGUI newText = Instantiate(textPrefab, contentRect);

		// �ؽ�Ʈ �⺻ ����
		newText.text = ""; // �� �ؽ�Ʈ�� �ʱ�ȭ
		newText.enableWordWrapping = true; // �ڵ� �ٹٲ� Ȱ��ȭ

		// �θ��� ũ�� ���� ��������
		float parentWidth = parentRect.rect.width;

		// �ؽ�Ʈ RectTransform ����
		RectTransform textRect = newText.rectTransform;

		// ���� ũ�� ���� (�Ź� -5�� �پ��� ����)
		float adjustedWidth = Mathf.Max(parentWidth - 100f, 0f); // 0 ���Ϸ� �پ���� �ʵ��� ����
		newText.rectTransform.sizeDelta = new Vector2(adjustedWidth, 0);

		textRect.anchorMin = new Vector2(0.5f, 1f);
		textRect.anchorMax = new Vector2(0.5f, 1f);
		textRect.pivot = new Vector2(0.5f, 1f);

		// �ؽ�Ʈ ���� ����
		switch (colorNum)
		{
			case 0:
				newText.color = Color.white;
				break;
			case 1:
				newText.color = Color.green;
				break;
			case 2:
				newText.color = Color.red;
				break;
		}

		// �ؽ�Ʈ ��ġ ���� (���� Y ��ǥ�� �̾ ��ġ)
		textRect.anchoredPosition = new Vector2(0f, lastYPosition);

		if (story != null)
		{
			// �ؽ�Ʈ�� \n���� ����� �� �ٿ� ���� Ÿ���� ȿ�� ����
			string[] lines = story.Split(new string[] { "\n" }, System.StringSplitOptions.None);

			// ���� �� ��� ���� ���� ���
			int totalCharacters = story.Length;
			float estimatedHeight = Mathf.Ceil(totalCharacters / (adjustedWidth / newText.fontSize)) * newText.fontSize;

			// ������ ���� �̸� Ȯ��
			contentHeight += estimatedHeight + textSpacing;
			contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					Debug.LogWarning("Empty or null line detected in story.");
					continue;
				}
				else
				{
					string displayedText = "";
					for (int i = 0; i <= line.Length; i++)
					{
						displayedText = line.Substring(0, i); // �� ���ھ� �߰�
						newText.text = displayedText;  // Ÿ���� ȿ�� ����
						yield return new WaitForSeconds(typingSpeed); // Ÿ���� ������

						// ��ũ�� ������Ʈ
						ForceScrollToBottom();
					}

					// �ٹٲ� �߰�
					newText.text += "\n";
					yield return new WaitForSeconds(term); // �ٹٲ� �� ��� ���
				}
			}
		}

		// �ؽ�Ʈ ũ�� ������Ʈ
		newText.ForceMeshUpdate();
		Vector2 preferredSize = newText.GetPreferredValues(adjustedWidth, Mathf.Infinity);
		textRect.sizeDelta = new Vector2(adjustedWidth, preferredSize.y);

		// Y ��ġ ������Ʈ
		lastYPosition -= preferredSize.y + textSpacing; // Y ��ǥ�� �ؽ�Ʈ ���� + ���ݸ�ŭ �Ʒ��� �̵�

		// ������ ���� ����
		contentHeight = Mathf.Abs(lastYPosition); // contentHeight�� ���밪���� ����
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight); // �������� ���̸� �ؽ�Ʈ ���̿� ���߱�

		// ������ ���̾ƿ��� ����
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect); // ���� ���̾ƿ� ������Ʈ

		Debug.Log("���� ��ũ�� Y(����) ��ǥ ���� ��ġ:" + contentRect.sizeDelta.y);

		// ������ ��ũ���� �� �Ʒ��� ������
		ForceScrollToBottom();
	}


	// ��ũ���� ������ �� �Ʒ��� ������ �Լ�
	private void ForceScrollToBottom()
	{
		Canvas.ForceUpdateCanvases(); // UI ���� ������Ʈ
		scrollRect.verticalNormalizedPosition = 0f; // ��ũ�� �� �Ʒ��� �̵�
		Canvas.ForceUpdateCanvases(); // �ٽ� ���� ������Ʈ
	}




	// ��ư ���� �Լ�
	public void AddButtonBelowLastText(int i, string BtnText)
	{
		// ��ư ����
		GameObject newButton = Instantiate(buttonPrefabs[i], contentRect);

		// ��ư RectTransform ����
		RectTransform buttonRect = newButton.GetComponent<RectTransform>();
		buttonRect.anchorMin = new Vector2(0.5f, 1f);
		buttonRect.anchorMax = new Vector2(0.5f, 1f);
		buttonRect.pivot = new Vector2(0.5f, 1f);

		// ��ư �ؽ�Ʈ ����
		TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
		if (buttonText != null && i != 3)
		{
			buttonText.text = BtnText;
		}
		else if (i == 3)
		{
			Debug.Log("������ ��ư Ŭ����");
		}
		else
		{
			Debug.LogWarning("Button prefab does not contain a TextMeshProUGUI component.");
		}

		// ��ư ���� ����
		Button button = newButton.GetComponent<Button>();
		ButtonAction buttonAction = newButton.GetComponent<ButtonAction>();  // ��ư�� ����� ButtonAction ������Ʈ ��������
		if (button != null && buttonAction != null)
		{
			switch (i)
			{
				case 0:
					button.onClick.AddListener(() => buttonAction.YesAction(BtnText));  // ���ٽ����� ���ڸ� ����
					break;
				case 1:
					button.onClick.AddListener(() => buttonAction.NoAction(BtnText));  // ���ٽ����� ���ڸ� ����
					break;
				case 2:
					button.onClick.AddListener(() => buttonAction.ContinueBtn());  // ���ٽ����� ���ڸ� ����
					break;
				case 3:
					button.onClick.AddListener(() => buttonAction.BattleBtn(BtnText));  // ���ٽ����� ���ڸ� ����
					break;
			}
		}

		// ��ư ��ġ ����
		float buttonHeight = buttonRect.sizeDelta.y;
		buttonRect.anchoredPosition = new Vector2(0f, lastYPosition - buttonSpacing);

		// ũ�� ���� ����
		LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRect);  // ������ ���̾ƿ��� ����

		// Y ��ġ ������Ʈ
		lastYPosition -= buttonHeight + buttonSpacing;
		currentYPosition = lastYPosition;

		// ������ ���� ������Ʈ
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, Mathf.Abs(lastYPosition));

		// ��ũ�� ���� ������Ʈ
		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}