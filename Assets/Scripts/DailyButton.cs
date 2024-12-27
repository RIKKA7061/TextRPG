using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyButton : MonoBehaviour
{
	public Button button; // ��ư ������Ʈ
	public TextMeshProUGUI buttonText; // ��ư �ؽ�Ʈ
	public string activeText = "Ŭ���ϼ���!"; // Ȱ��ȭ ���� �ؽ�Ʈ
	public string inactiveTextPrefix = "���� �ð�: "; // ��Ȱ��ȭ ���� �ؽ�Ʈ ���λ�
	private DateTime nextAvailableTime; // ��ư ���� ���� �ð�
	private bool isButtonActive = true; // ��ư Ȱ��ȭ ����

	private void Start()
	{
		button.onClick.AddListener(OnButtonClick);

		// ����� �� �ҷ�����
		string savedTime = PlayerPrefs.GetString("NextAvailableTime", string.Empty);
		if (!string.IsNullOrEmpty(savedTime))
		{
			nextAvailableTime = DateTime.Parse(savedTime);
		}
		else
		{
			nextAvailableTime = DateTime.Now; // �ʱⰪ ����
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
			// ��ư �׼� ���� (��: Debug.Log ���)
			Debug.Log("��ư Ŭ��!");

			// ��ư ��Ȱ��ȭ ó��
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
