using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StackTextManager : MonoBehaviour
{
	public RectTransform parentRect; // 부모 RectTransform
	public TextMeshProUGUI textPrefab; // TextMeshPro 프리팹
	static public string[] storyArray = new string[100]; // 스토리 배열
	public ScrollRect scrollRect; // ScrollRect 컴포넌트
	public RectTransform contentRect; // ScrollRect의 컨텐츠 영역
	public QuestionManager questionManager;
	public ButtonAction buttonActions;
	public ContinueManager continueManager;
	public BattleManager battleManager;
	public StoryPlayManager storyPlayManager;
	public RewardManager rewardManager;
	public EndManager endManager;
	public TextMeshProUGUI StoryTypingSpeed;
	public TextMeshProUGUI termSpeed;

	[Header("버튼 프리팹")]
	public GameObject[] buttonPrefabs; // 생성할 버튼 프리팹 0(예) 1(아니오)
	public float buttonSpacing = 20f; // 버튼과 마지막 텍스트 간 간격

	[Header("텍스트 간 개행 간격")]
	public float textSpacing = 10f; // 텍스트 간 간격

	public float term = 0.5f; // 텍스트 문장 출력 간 간격
	public float typingSpeed = 0.05f; // 타이핑 딜레이

	private float currentYPosition = 0f; // 텍스트 배치 위치


	private void Start()
	{
		Set_SettingUI();
	}

	public void Set_SettingUI()
	{
		StoryTypingSpeed.text = "스토리 타이핑 텀: " + typingSpeed.ToString();
		termSpeed.text = "줄간격 텀: " + term.ToString();

	}

	public void StoryTypingSpeedUP()
	{
		typingSpeed -= 0.01f;
		Set_SettingUI();
	}

	public void StoryTypingSpeedDown()
	{
		typingSpeed += 0.01f;
		Set_SettingUI();
	}

	public void TermUP()
	{
		term -= 0.01f;
		Set_SettingUI();
	}

	public void TermDown()
	{
		term += 0.01f;
		Set_SettingUI();
	}


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

	public void Generate_Stack(int colorNum, string[] SendText, string Case) // 0 하얀, 1 초록, 2 빨강
	{
		StartCoroutine(Stack(colorNum, SendText, Case));
	}

	public IEnumerator Make_Story_Btn(int colorNum)
	{
		//초기화(기존 텍스트 제거)
		foreach (Transform child in contentRect)
		{
			Destroy(child.gameObject);
		}
		currentYPosition = 0f;

		// 스토리 배열 순회
		foreach (string story in storyArray)
		{
			yield return StartCoroutine(AddTextWithTypingEffect(colorNum, story));
			yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
		}

		Debug.Log("스토리 끝");
	}

	public IEnumerator Make_Story_earnHP(int colorNum)
	{
		// 스토리 배열 순회
		foreach (string story in storyArray)
		{
			//yield return StartCoroutine(AddTextWithTypingEffect(story, colorNum));
			yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
		}
		//YesNoBtnManager.earnHP();
	}

	public IEnumerator Make_Story_loseHP(int colorNum)
	{
		// 스토리 배열 순회
		foreach (string story in storyArray)
		{
			//yield return StartCoroutine(AddTextWithTypingEffect(story, colorNum));
			yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
		}
		//YesNoBtnManager.loseHP();
	}

	// Make sure to reset contentRect height before adding any new elements
	public IEnumerator Stack(int colorNum, string[] SendTEXT, string Case)
	{
		// Y 위치 초기화 로그
		Debug.Log("Y 위치 초기화");

		// contentRect의 자식 객체를 모두 파괴
		foreach (Transform child in contentRect)
		{
			Destroy(child.gameObject);
		}

		// currentYPosition 초기화
		currentYPosition = 0f;
		lastYPosition = 0f;

		// Reset content height to 0
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 0f); // Set height to 0 initially

		// currentYPosition 초기화 후 상태 출력
		Debug.Log($"currentYPosition 초기화 후: {currentYPosition}");

		// 스토리 배열 순회
		foreach (string story in SendTEXT)
		{
			if (Case == "Reward")
			{
				if (story == SendTEXT[SendTEXT.Length - 1])
				{
					Debug.Log(story);
					string returnMessage = rewardManager.RewardAction(story); // 적용

					yield return StartCoroutine(AddTextWithTypingEffect(1, returnMessage));
					yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
					continueManager.ContinueBtnMaker();
				}
				else
				{
					yield return StartCoroutine(AddTextWithTypingEffect(colorNum, story));
					yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
				}
			}
			else
			{
				yield return StartCoroutine(AddTextWithTypingEffect(colorNum, story));
				yield return new WaitForSeconds(term); // 텍스트 출력 간 딜레이
			}
		}

		Debug.Log("출력 완료");

		// Case에 따른 동작 처리
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
			case "Reward":
				Debug.Log("보상");
				break;
			case "End":
				endManager.EndBtnMaker();
				Debug.Log("다음 장 스토리 버튼 생성");
				break;
			default:
				break;
		}
	}



	private float lastYPosition = 0f; // 텍스트의 마지막 Y 위치 추적
	private float contentHeight = 0f;  // 전체 콘텐츠 높이 추적


	// string 스토리한 문장을 한글자씩 타이핑 해주는 함수
	private IEnumerator AddTextWithTypingEffect(int colorNum, string story)
	{
		// TMP 프리팹 인스턴스 생성
		TextMeshProUGUI newText = Instantiate(textPrefab, contentRect);

		// 텍스트 기본 설정
		newText.text = ""; // 빈 텍스트로 초기화
		newText.enableWordWrapping = true; // 자동 줄바꿈 활성화

		// 부모의 크기 제한 가져오기
		float parentWidth = parentRect.rect.width;

		// 텍스트 RectTransform 설정
		RectTransform textRect = newText.rectTransform;

		// 가로 크기 조정 (매번 -5씩 줄어들게 설정)
		float adjustedWidth = Mathf.Max(parentWidth - 100f, 0f); // 0 이하로 줄어들지 않도록 제한
		newText.rectTransform.sizeDelta = new Vector2(adjustedWidth, 0);

		textRect.anchorMin = new Vector2(0.5f, 1f);
		textRect.anchorMax = new Vector2(0.5f, 1f);
		textRect.pivot = new Vector2(0.5f, 1f);

		// 텍스트 색상 설정
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

		// 텍스트 위치 설정 (기존 Y 좌표에 이어서 배치)
		textRect.anchoredPosition = new Vector2(0f, lastYPosition);

		if (story != null)
		{
			// 텍스트를 \n으로 나누어서 각 줄에 대해 타이핑 효과 적용
			string[] lines = story.Split(new string[] { "\n" }, System.StringSplitOptions.None);

			// 글자 수 기반 예상 높이 계산
			int totalCharacters = story.Length;
			float estimatedHeight = Mathf.Ceil(totalCharacters / (adjustedWidth / newText.fontSize)) * newText.fontSize;

			// 콘텐츠 높이 미리 확보
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
						displayedText = line.Substring(0, i); // 한 글자씩 추가
						newText.text = displayedText;  // 타이핑 효과 적용
						yield return new WaitForSeconds(typingSpeed); // 타이핑 딜레이

						// 스크롤 업데이트
						ForceScrollToBottom();
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

		// Y 위치 업데이트
		lastYPosition -= preferredSize.y + textSpacing; // Y 좌표를 텍스트 높이 + 간격만큼 아래로 이동

		// 콘텐츠 높이 갱신
		contentHeight = Mathf.Abs(lastYPosition); // contentHeight를 절대값으로 설정
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight); // 콘텐츠의 높이를 텍스트 높이에 맞추기

		// 강제로 레이아웃을 갱신
		LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect); // 강제 레이아웃 업데이트

		Debug.Log("현재 스크롤 Y(높이) 좌표 갱신 위치:" + contentRect.sizeDelta.y);

		// 강제로 스크롤을 맨 아래로 내리기
		ForceScrollToBottom();
	}


	// 스크롤을 강제로 맨 아래로 내리는 함수
	private void ForceScrollToBottom()
	{
		Canvas.ForceUpdateCanvases(); // UI 강제 업데이트
		scrollRect.verticalNormalizedPosition = 0f; // 스크롤 맨 아래로 이동
		Canvas.ForceUpdateCanvases(); // 다시 강제 업데이트
	}




	// 버튼 생성 함수
	public void AddButtonBelowLastText(int i, string BtnText)
	{
		// 버튼 생성
		GameObject newButton = Instantiate(buttonPrefabs[i], contentRect);

		// 버튼 RectTransform 설정
		RectTransform buttonRect = newButton.GetComponent<RectTransform>();
		buttonRect.anchorMin = new Vector2(0.5f, 1f);
		buttonRect.anchorMax = new Vector2(0.5f, 1f);
		buttonRect.pivot = new Vector2(0.5f, 1f);

		// 버튼 텍스트 설정
		TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
		if (buttonText != null && i != 3)
		{
			buttonText.text = BtnText;
		}
		else if (i == 3)
		{
			Debug.Log("전투용 버튼 클릭함");
		}
		else
		{
			Debug.LogWarning("Button prefab does not contain a TextMeshProUGUI component.");
		}

		// 버튼 동작 설정
		Button button = newButton.GetComponent<Button>();
		ButtonAction buttonAction = newButton.GetComponent<ButtonAction>();  // 버튼에 연결된 ButtonAction 컴포넌트 가져오기
		if (button != null && buttonAction != null)
		{
			switch (i)
			{
				case 0:
					button.onClick.AddListener(() => buttonAction.YesAction(BtnText));  // 람다식으로 인자를 전달
					break;
				case 1:
					button.onClick.AddListener(() => buttonAction.NoAction(BtnText));  // 람다식으로 인자를 전달
					break;
				case 2:
					button.onClick.AddListener(() => buttonAction.ContinueBtn());  // 람다식으로 인자를 전달
					break;
				case 3:
					button.onClick.AddListener(() => buttonAction.BattleBtn(BtnText));  // 람다식으로 인자를 전달
					break;
			}
		}

		// 버튼 위치 설정
		float buttonHeight = buttonRect.sizeDelta.y;
		buttonRect.anchoredPosition = new Vector2(0f, lastYPosition - buttonSpacing);

		// 크기 강제 적용
		LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRect);  // 강제로 레이아웃을 갱신

		// Y 위치 업데이트
		lastYPosition -= buttonHeight + buttonSpacing;
		currentYPosition = lastYPosition;

		// 콘텐츠 높이 업데이트
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, Mathf.Abs(lastYPosition));

		// 스크롤 강제 업데이트
		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}
