using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleDisplayManager : MonoBehaviour
{
	public Button AtkBtn;
	public Button AvoidBtn;
	public EnemyStatManager enemyStatManager;
	public PlayerStatManager playerStatManager;
	public GameObject textPrefab;
	public Transform scrollContent;
	public ScrollRect scrollRect;
	public StoryPlayManager storyPlayManager;
	public TextColorFade textColorFade;

	[Header("�ѱ��� �� Ÿ�����ϴ� ������ ��")]
	public float typingSpeed;

	[Header("�� ���� �� ������ ������ ��")]
	public float term;

	public GameObject Battle;
	public GameObject Ending;
	public TextMeshProUGUI endingText;

	public bool isBattleOver;
	public RectTransform parentRect; // Parent RectTransform to control text width

	private float lastYPosition = 0f; // Y position for positioning the next text
	private float contentHeight = 0f; // Total content height to manage scroll size
	private float textSpacing = 10f; // Spacing between text lines

	private RectTransform contentRect; // Declare contentRect

	public GameObject continueButtonPrefab; // "����ϱ�" ��ư ������


	void Start()
	{
		scrollRect.viewport = GetComponentInChildren<RectTransform>(); // Adjust the selector as needed
	}


	// Start or Awake to initialize contentRect
	private void Awake()
	{
		contentRect = scrollContent.GetComponent<RectTransform>();
		if (contentRect == null)
		{
			Debug.LogError("contentRect is not assigned!");
		}
	}


	public void BattleLogic()
	{
		isBattleOver = false;
		PlayerTurn_Btns(false);
		Debug.Log($"�÷��̾�: {PlayerStatManager.AvoidRate}");
		StartCoroutine(DisplayText($"{EnemyStatManager.Job}(��/��) ��Ÿ����."));
		StartCoroutine(DelayedPlayerTurn());
	}

	public void PlayerTurn()
	{
		StartCoroutine(DisplayText("��� �ұ�?"));
		StartCoroutine(DisplayText(" "));

		PlayerTurn_Btns(true);
	}

	IEnumerator DelayedPlayerTurn()
	{
		yield return new WaitForSeconds(1f);
		PlayerTurn();
	}

	public void PlayerTurn_Btns(bool isPlayerTurn)
	{
		if (AtkBtn != null)
		{
			AtkBtn.interactable = isPlayerTurn;
			AvoidBtn.interactable = isPlayerTurn;
		}
	}

	public void PlayerAtkBtnAction()
	{
		PlayerTurn_Btns(false);
		int randomNum = GenerateRandomNum(1, 100);
		if (randomNum > EnemyStatManager.AvoidRate)
		{
			EnemyStatManager.nowHP -= PlayerStatManager.Atk;
			enemyStatManager.SetEnemeyBattleUI();
			HPCheck();
			if (!isBattleOver)
			{
				StartCoroutine(DisplayText($"�������� {PlayerStatManager.Atk} ���ظ� ������."));
				StartCoroutine(DelayedEnemyTurn());
			}
		}
		else
		{
			StartCoroutine(DisplayText($"���� ������ ȸ���ߴ�!"));
			StartCoroutine(DelayedEnemyTurn());
		}
	}

	public void PlayerAvoidUpBtnAction()
	{
		PlayerTurn_Btns(false);
		int randomNum = GenerateRandomNum(1, 5);
		if(PlayerStatManager.AvoidRate >= 99)
		{
			PlayerStatManager.AvoidRate = 99;
			StartCoroutine(DisplayText($"���� ȸ������ �ִ�ġ�Դϴ�.(���� ȸ����: {PlayerStatManager.AvoidRate})"));
			StartCoroutine(DelayedEnemyTurn());
		}
        else
        {
			PlayerStatManager.AvoidRate += randomNum;
			if (PlayerStatManager.AvoidRate >= 99)
			{
				PlayerStatManager.AvoidRate = 99;
			}
			StartCoroutine(DisplayText($"ȸ������ {randomNum} ��� �Ǿ����ϴ�.(���� ȸ����: {PlayerStatManager.AvoidRate})"));
			StartCoroutine(DelayedEnemyTurn());
		}
	}

	public void EnemyTurn()
	{
		int randomNum = GenerateRandomNum(1, 100);
		if (randomNum > PlayerStatManager.AvoidRate)
		{
			PlayerStatManager.nowHP -= EnemyStatManager.Atk;
			playerStatManager.SetPlayerBattleUI();
			HPCheck();
			if (!isBattleOver)
			{
				StartCoroutine(DisplayText($"�� ���ݿ� {EnemyStatManager.Atk} ���ظ� �Ծ���."));
				StartCoroutine(DelayedPlayerTurn());
			}
		}
		else
		{
			StartCoroutine(DisplayText("���� ������ ȸ���ߴ�!"));
			StartCoroutine(DelayedPlayerTurn());
		}
	}

	public void HPCheck()
	{
		if (PlayerStatManager.nowHP <= 0)
		{
			isBattleOver = true;
			PlayerStatManager.nowHP = 0;
			playerStatManager.SetPlayerBattleUI();
			StartCoroutine(DisplayText("ü���� ����."));

			// ����ȭ�� with �ٽ��ϱ��ư // �� ��ε� SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			endingText.text = "�й迣��";
			Ending.SetActive(true);
		}
		else if (EnemyStatManager.nowHP <= 0)
		{
			// ���� ������
			isBattleOver = true;
			EnemyStatManager.nowHP = 0;
			enemyStatManager.SetEnemeyBattleUI();
			playerStatManager.SetUI(); // UI �� ���� ���� ���Ŀ� ����� ���� �ֱ�
			StartCoroutine(DisplayText("���� ���񷶴�."));

			StartCoroutine(DelayedCreateContinueBtn());

			//// ����â ���� ���� ���丮 ���
			//Battle.SetActive(false);
			//storyPlayManager.StoryPlay();
			//// ��Ʋ �α� Ŭ����
			//ClearBattleLog();
		}
	}

	IEnumerator DelayedCreateContinueBtn()
	{
		yield return new WaitForSeconds(2f);
		CreateContinueButton();
	}

	private void CreateContinueButton()
	{
		if (continueButtonPrefab != null && scrollContent != null)
		{
			// ������ �ν��Ͻ�ȭ
			GameObject continueButton = Instantiate(continueButtonPrefab, scrollContent);

			// RectTransform ����
			RectTransform buttonRect = continueButton.GetComponent<RectTransform>();
			if (buttonRect != null)
			{
				// �θ��� ũ�� ��������
				float parentWidth = parentRect.rect.width;

				// ��ư ��ġ�� ũ�� ����
				buttonRect.sizeDelta = new Vector2(parentWidth - 50f, buttonRect.sizeDelta.y); // �θ� �ʺ� ����
				buttonRect.anchorMin = new Vector2(0.5f, 1f);
				buttonRect.anchorMax = new Vector2(0.5f, 1f);
				buttonRect.pivot = new Vector2(0.5f, 1f);
				buttonRect.anchoredPosition = new Vector2(0f, lastYPosition);

				// Y ��ġ ������Ʈ (��ư ���� + ����)
				float buttonHeight = buttonRect.sizeDelta.y;
				lastYPosition -= buttonHeight + textSpacing;

				// ������ ���� ������Ʈ
				contentHeight = Mathf.Max(contentHeight, Mathf.Abs(lastYPosition));
				contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
			}

			// ��ư �̺�Ʈ ����
			Button button = continueButton.GetComponent<Button>();
			if (button != null)
			{
				button.onClick.AddListener(() =>
				{
					// ��ư Ŭ�� �� ������ �۾�
					Battle.SetActive(false);
					storyPlayManager.StoryPlay();
					ClearBattleLog();
					Destroy(continueButton); // ��ư ����
				});
			}

			// ��ũ�� ��ġ ������Ʈ
			Canvas.ForceUpdateCanvases();
			StartCoroutine(SmoothScrollToBottom());
		}
		else
		{
			Debug.LogError("����ϱ� ��ư ������ �Ǵ� ��ũ�� �������� �������� �ʾҽ��ϴ�!");
		}
	}


	public IEnumerator DisplayText(string message)
	{
		// TMP ������ �ν��Ͻ� ����
		TextMeshProUGUI newText = Instantiate(textPrefab, scrollContent).GetComponent<TextMeshProUGUI>();
		newText.text = ""; // �ؽ�Ʈ �ʱ�ȭ
		newText.enableWordWrapping = true; // �ڵ� �ٹٲ� Ȱ��ȭ

		// �θ��� ũ�� ���� ��������
		float parentWidth = parentRect.rect.width;

		// �ؽ�Ʈ RectTransform ����
		RectTransform textRect = newText.rectTransform;

		// ���� ũ�� ���� (�Ź� -50�� �پ��� ����)
		float adjustedWidth = Mathf.Max(parentWidth - 50f, 0f); // 0 ���Ϸ� �پ���� �ʵ��� ����
		newText.rectTransform.sizeDelta = new Vector2(adjustedWidth, 0);

		textRect.anchorMin = new Vector2(0.5f, 1f);
		textRect.anchorMax = new Vector2(0.5f, 1f);
		textRect.pivot = new Vector2(0.5f, 1f);

		// ù ��° �ؽ�Ʈ�� ��ġ�� 0���� ����
		textRect.anchoredPosition = new Vector2(0f, lastYPosition);

		if (message != null)
		{
			// �ؽ�Ʈ�� \n���� ����� �� �ٿ� ���� Ÿ���� ȿ�� ����
			string[] lines = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);

			// ���� �� ��� ���� ���� ���
			int totalCharacters = message.Length;
			float estimatedHeight = Mathf.Ceil(totalCharacters / (adjustedWidth / newText.fontSize)) * newText.fontSize;

			// ������ ���� �̸� Ȯ��
			contentHeight += estimatedHeight + textSpacing;
			contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight); // ������ ���� ������Ʈ

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
						StartCoroutine(SmoothScrollToBottom());
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

		// Y ��ġ ������Ʈ: ���� ��ġ���� �ؽ�Ʈ ���̸�ŭ ������
		lastYPosition -= preferredSize.y + textSpacing; // Y ��ǥ�� �ؽ�Ʈ ���� + ���ݸ�ŭ �Ʒ��� �̵�

		// ������ ���� ����
		contentHeight = Mathf.Max(contentHeight, Mathf.Abs(lastYPosition)); // �ִ밪���� ����
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

		// ������ ���̾ƿ��� ����
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

		// ��ũ�� ��ġ�� ���� ������ �ؽ�Ʈ�� �̵�
		Canvas.ForceUpdateCanvases();
		StartCoroutine(SmoothScrollToBottom());
	}

	// �ε巴�� ��ũ���ϴ� �ڷ�ƾ
	IEnumerator SmoothScrollToBottom()
	{
		// ��ũ�� �ӵ� ����
		float scrollSpeed = 5f;
		float elapsedTime = 0f;

		RectTransform viewportRect = scrollRect.viewport;

		if (viewportRect != null)
		{
			float targetPosition = -contentRect.anchoredPosition.y + viewportRect.rect.height - contentRect.rect.height;
		}


		while (scrollRect.verticalNormalizedPosition > 0f)
		{
			elapsedTime += Time.deltaTime;
			float normalizedTime = Mathf.Clamp01(elapsedTime / scrollSpeed);
			float newPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, 0f, normalizedTime);
			scrollRect.verticalNormalizedPosition = newPosition;
			yield return null;
		}
	}

	public void ClearBattleLog()
	{
		foreach (Transform child in scrollContent)
		{
			Destroy(child.gameObject);
		}
		lastYPosition = 0f; // �α� �ʱ�ȭ �� Y ��ġ�� ����
		contentHeight = 0f; // ������ ���̵� ����
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 0f); // ������ ũ�� �ʱ�ȭ
	}


	public int GenerateRandomNum(int a, int b)
	{
		return Random.Range(a, b + 1);
	}

	IEnumerator DelayedEnemyTurn()
	{
		yield return new WaitForSeconds(0.5f);
		EnemyTurn();
	}
}
