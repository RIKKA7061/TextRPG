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

	[Header("한글자 씩 타이핑하는 딜레이 초")]
	public float typingSpeed;

	[Header("한 스택 씩 생성시 딜레이 초")]
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

	public GameObject continueButtonPrefab; // "계속하기" 버튼 프리팹


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
		Debug.Log($"플레이어: {PlayerStatManager.AvoidRate}");
		StartCoroutine(DisplayText($"야생의 {EnemyStatManager.Job}(이/가) 나타났다."));
		StartCoroutine(DelayedPlayerTurn());
	}

	public void PlayerTurn()
	{
		StartCoroutine(DisplayText("어떻게 할까?"));
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
				StartCoroutine(DisplayText($"공격으로 {PlayerStatManager.Atk} 피해를 입혔다."));
				StartCoroutine(DelayedEnemyTurn());
			}
		}
		else
		{
			StartCoroutine(DisplayText($"적이 공격을 회피했다!"));
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
			StartCoroutine(DisplayText($"현재 회피율이 최대치입니다.(현재 회피율: {PlayerStatManager.AvoidRate})"));
			StartCoroutine(DelayedEnemyTurn());
		}
        else
        {
			PlayerStatManager.AvoidRate += randomNum;
			if (PlayerStatManager.AvoidRate >= 99)
			{
				PlayerStatManager.AvoidRate = 99;
			}
			StartCoroutine(DisplayText($"회피율이 {randomNum} 상승 되었습니다.(현재 회피율: {PlayerStatManager.AvoidRate})"));
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
				StartCoroutine(DisplayText($"적 공격에 {EnemyStatManager.Atk} 피해를 입었다."));
				StartCoroutine(DelayedPlayerTurn());
			}
		}
		else
		{
			StartCoroutine(DisplayText("적의 공격을 회피했다!"));
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
			StartCoroutine(DisplayText("체력이 없다."));

			// 엔딩화면 with 다시하기버튼 // 씬 재로드 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			endingText.text = "패배엔딩";
			Ending.SetActive(true);
		}
		else if (EnemyStatManager.nowHP <= 0)
		{
			// 전투 끝날시
			isBattleOver = true;
			EnemyStatManager.nowHP = 0;
			enemyStatManager.SetEnemeyBattleUI();
			playerStatManager.SetUI(); // UI 에 현재 전투 이후에 변경된 스탯 넣기
			StartCoroutine(DisplayText("적을 무찔렀다."));

			StartCoroutine(DelayedCreateContinueBtn());

			//// 전투창 끄고 다음 스토리 재생
			//Battle.SetActive(false);
			//storyPlayManager.StoryPlay();
			//// 배틀 로그 클리어
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
			// 프리팹 인스턴스화
			GameObject continueButton = Instantiate(continueButtonPrefab, scrollContent);

			// RectTransform 설정
			RectTransform buttonRect = continueButton.GetComponent<RectTransform>();
			if (buttonRect != null)
			{
				// 부모의 크기 가져오기
				float parentWidth = parentRect.rect.width;

				// 버튼 위치와 크기 설정
				buttonRect.sizeDelta = new Vector2(parentWidth - 50f, buttonRect.sizeDelta.y); // 부모 너비에 맞춤
				buttonRect.anchorMin = new Vector2(0.5f, 1f);
				buttonRect.anchorMax = new Vector2(0.5f, 1f);
				buttonRect.pivot = new Vector2(0.5f, 1f);
				buttonRect.anchoredPosition = new Vector2(0f, lastYPosition);

				// Y 위치 업데이트 (버튼 높이 + 간격)
				float buttonHeight = buttonRect.sizeDelta.y;
				lastYPosition -= buttonHeight + textSpacing;

				// 콘텐츠 높이 업데이트
				contentHeight = Mathf.Max(contentHeight, Mathf.Abs(lastYPosition));
				contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
			}

			// 버튼 이벤트 연결
			Button button = continueButton.GetComponent<Button>();
			if (button != null)
			{
				button.onClick.AddListener(() =>
				{
					// 버튼 클릭 시 실행할 작업
					Battle.SetActive(false);
					storyPlayManager.StoryPlay();
					ClearBattleLog();
					Destroy(continueButton); // 버튼 제거
				});
			}

			// 스크롤 위치 업데이트
			Canvas.ForceUpdateCanvases();
			StartCoroutine(SmoothScrollToBottom());
		}
		else
		{
			Debug.LogError("계속하기 버튼 프리팹 또는 스크롤 콘텐츠가 설정되지 않았습니다!");
		}
	}


	public IEnumerator DisplayText(string message)
	{
		// TMP 프리팹 인스턴스 생성
		TextMeshProUGUI newText = Instantiate(textPrefab, scrollContent).GetComponent<TextMeshProUGUI>();
		newText.text = ""; // 텍스트 초기화
		newText.enableWordWrapping = true; // 자동 줄바꿈 활성화

		// 부모의 크기 제한 가져오기
		float parentWidth = parentRect.rect.width;

		// 텍스트 RectTransform 설정
		RectTransform textRect = newText.rectTransform;

		// 가로 크기 조정 (매번 -50씩 줄어들게 설정)
		float adjustedWidth = Mathf.Max(parentWidth - 50f, 0f); // 0 이하로 줄어들지 않도록 제한
		newText.rectTransform.sizeDelta = new Vector2(adjustedWidth, 0);

		textRect.anchorMin = new Vector2(0.5f, 1f);
		textRect.anchorMax = new Vector2(0.5f, 1f);
		textRect.pivot = new Vector2(0.5f, 1f);

		// 첫 번째 텍스트의 위치는 0으로 설정
		textRect.anchoredPosition = new Vector2(0f, lastYPosition);

		if (message != null)
		{
			// 텍스트를 \n으로 나누어서 각 줄에 대해 타이핑 효과 적용
			string[] lines = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);

			// 글자 수 기반 예상 높이 계산
			int totalCharacters = message.Length;
			float estimatedHeight = Mathf.Ceil(totalCharacters / (adjustedWidth / newText.fontSize)) * newText.fontSize;

			// 콘텐츠 높이 미리 확보
			contentHeight += estimatedHeight + textSpacing;
			contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight); // 콘텐츠 높이 업데이트

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
						displayedText = line.Substring(0, i); // 한 글자씩 추가
						newText.text = displayedText;  // 타이핑 효과 적용
						yield return new WaitForSeconds(typingSpeed); // 타이핑 딜레이

						// 스크롤 업데이트
						StartCoroutine(SmoothScrollToBottom());
					}

					// 줄바꿈 추가
					newText.text += "\n";
					yield return new WaitForSeconds(term); // 줄바꿈 후 잠시 대기
				}
			}
		}

		// 텍스트 크기 업데이트
		newText.ForceMeshUpdate();
		Vector2 preferredSize = newText.GetPreferredValues(adjustedWidth, Mathf.Infinity);
		textRect.sizeDelta = new Vector2(adjustedWidth, preferredSize.y);

		// Y 위치 업데이트: 기존 위치에서 텍스트 높이만큼 내리기
		lastYPosition -= preferredSize.y + textSpacing; // Y 좌표를 텍스트 높이 + 간격만큼 아래로 이동

		// 콘텐츠 높이 갱신
		contentHeight = Mathf.Max(contentHeight, Mathf.Abs(lastYPosition)); // 최대값으로 설정
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

		// 강제로 레이아웃을 갱신
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

		// 스크롤 위치를 새로 생성된 텍스트로 이동
		Canvas.ForceUpdateCanvases();
		StartCoroutine(SmoothScrollToBottom());
	}

	// 부드럽게 스크롤하는 코루틴
	IEnumerator SmoothScrollToBottom()
	{
		// 스크롤 속도 조절
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
		lastYPosition = 0f; // 로그 초기화 후 Y 위치도 리셋
		contentHeight = 0f; // 콘텐츠 높이도 리셋
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 0f); // 콘텐츠 크기 초기화
	}


	public int GenerateRandomNum(int a, int b)
	{
		return Random.Range(a, b + 1);
	}

	IEnumerator DelayedEnemyTurn()
	{
		yield return new WaitForSeconds(2f);
		EnemyTurn();
	}
}
