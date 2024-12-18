using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class CharacterChoiceManager : MonoBehaviour
{
	// Button 프리팹 (Unity 에디터에서 연결)
	public GameObject buttonPrefab;
	public Transform buttonContainer; // 버튼들을 넣을 부모 오브젝트

	public StoryPlayManager storyPlayManager;

	public PlayerStatManager playerStatManager;

	public GameObject Character_Choice_Pannel;

	public DataManager dataManager;

	string[] Characters = { "기자", "역사학자", "형사", "배우"};


	// 스토리 함수에 들어갈 매개변수 세트
	private List<(int, int)> storyParams = new List<(int, int)>
	{
		(1, 2), // 기자
        (3, 4), // 역사학자
        (5, 6), // 형사
        (7, 8)  // 배우
    };

	void Start()
	{
		CreateButtons();
	}

	void CreateButtons()
	{
		Vector2 startPosition = new Vector2(0, 0); // 시작 위치
		float buttonSpacing = 100f; // 버튼 간 간격

		for (int i = 0; i < storyParams.Count; i++)
		{
			// 버튼 동적 생성
			GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
			newButton.name = $"CharacterButton_{i + 1}";

			// 버튼 텍스트 설정
			newButton.GetComponentInChildren<TextMeshProUGUI>().text = Characters[i];

			// 버튼 위치 설정
			RectTransform rectTransform = newButton.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = startPosition - new Vector2(0, i * buttonSpacing);

			// 이벤트 할당 (람다 표현식 사용)
			int index = i; // 람다 캡처 문제 방지
			newButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				StoryDownLoading(storyParams[index].Item1, storyParams[index].Item2);
			});
		}
	}

	// StoryDownLoading 함수 (예시)
	void StoryDownLoading(int param1, int param2)
	{
		Debug.Log($"해당 캐릭터의 스토리 파싱 작업 실행 - {param1}열 , {param2}열");
		int NowMode = param2 / 2 - 1; // 0,1,2,3 = 기자,역사학자,형사,배우
		dataManager.StoryDownLoading(param1, param2);			// 해당 캐릭터의 스토리를 파싱합니다.
		Debug.Log($"{NowMode} 모드");						// 캐릭터 모드
		playerStatManager.SetStat(NowMode);				// 캐릭터의 직업의 스탯을 현재 플레이어의 스탯으로 만듦
		playerStatManager.SetUI();				// 캐릭터의 직업 및 스텟을 ui에 등록
		Character_Choice_Pannel.SetActive(false);				// 캐릭터 선택 판때기 끄기
		storyPlayManager.StoryPlay();   // 스토리 재생
		Debug.Log(Characters[NowMode] + "스토리를 재생합니다.");
	}
}
