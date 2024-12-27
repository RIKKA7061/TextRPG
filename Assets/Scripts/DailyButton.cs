using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyButton : MonoBehaviour
{
	public Button button; // 버튼 컴포넌트
	public TextMeshProUGUI buttonText; // 버튼 텍스트
	public string activeText = "클릭하세요!"; // 활성화 상태 텍스트
	public string inactiveTextPrefix = "남은 시간: "; // 비활성화 상태 텍스트 접두사
	private DateTime nextAvailableTime; // 버튼 재사용 가능 시간
	private bool isButtonActive = true; // 버튼 활성화 여부

	private void Start()
	{
		button.onClick.AddListener(OnButtonClick);

		// 저장된 값 불러오기
		string savedTime = PlayerPrefs.GetString("NextAvailableTime", string.Empty);
		if (!string.IsNullOrEmpty(savedTime))
		{
			nextAvailableTime = DateTime.Parse(savedTime);
		}
		else
		{
			nextAvailableTime = DateTime.Now; // 초기값 설정
		}

		UpdateButtonState();
	}

	private void Update()
	{
		if (!isButtonActive)
		{
			TimeSpan timeRemaining = nextAvailableTime - DateTime.Now;

			if (timeRemaining <= TimeSpan.Zero)
			{
				ActivateButton();
			}
			else
			{
				buttonText.text = $"{inactiveTextPrefix}{FormatTime(timeRemaining)}";
			}
		}
	}

	private void OnButtonClick()
	{
		if (isButtonActive)
		{
			// 버튼 액션 실행 (예: Debug.Log 출력)
			Debug.Log("버튼 클릭!");

			// 버튼 비활성화 처리
			nextAvailableTime = DateTime.Now.AddDays(1);
			PlayerPrefs.SetString("NextAvailableTime", nextAvailableTime.ToString());
			PlayerPrefs.Save();
			DeactivateButton();
		}
	}

	private void ActivateButton()
	{
		isButtonActive = true;
		button.interactable = true;
		buttonText.text = activeText;
	}

	private void DeactivateButton()
	{
		isButtonActive = false;
		button.interactable = false;
	}

	private void UpdateButtonState()
	{
		if (DateTime.Now >= nextAvailableTime)
		{
			ActivateButton();
		}
		else
		{
			DeactivateButton();
		}
	}

	private string FormatTime(TimeSpan time)
	{
		return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
	}
}
