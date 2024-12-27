using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
	public TMP_Text coinText; // coin Count UI
	public GameObject NoMoneyPannel; // no money
	private ShopManager shopManager;
	private CharacterChoiceManager characterChoiceManager; // cache

	void Awake()
	{
		shopManager = FindAnyObjectByType<ShopManager>();
		characterChoiceManager = FindAnyObjectByType<CharacterChoiceManager>();
	}

	private void Start()
	{
		ShopManager.isRelease = new bool[characterChoiceManager.storyParams.Count];
		shopManager.LoadIsRelease();
		shopManager.SyncMoney();
		UpdateCoinUI();
	}

	public void UpdateCoinUI()
	{
		coinText.text = $"Coin: {ShopManager.coins}";
	}

	public void ShowMoneyIssue()
	{
		NoMoneyPannel.SetActive(true);
	}
}
