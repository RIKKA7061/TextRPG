using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance { get; private set; }
	public static bool[] isRelease;
	public static int coins; // NOW coin
	private const string coinKEY = "Coins"; // PlayePrefs Key
	private CharacterChoiceManager characterChoiceManager; // cache
	private ShopUIManager shopUIManager; // cache
	private void Awake()
	{
		// 싱글턴 패턴: ShopManager 인스턴스가 이미 존재하면 파괴
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		characterChoiceManager = FindAnyObjectByType<CharacterChoiceManager>();
		shopUIManager = FindAnyObjectByType<ShopUIManager>();
		SyncMoney();
		isRelease = new bool[characterChoiceManager.storyParams.Count];
		isRelease[0] = true; // reporter
		LoadIsRelease();
	}

	public void AddMoney()
	{
		coins++;
		PlayerPrefs.SetInt(coinKEY, coins);
		PlayerPrefs.Save();
		shopUIManager.UpdateCoinUI();
	}

	public void SubtractMoney()
	{
		coins--;
		PlayerPrefs.SetInt(coinKEY, coins);
		shopUIManager.UpdateCoinUI();
	}

	public void SyncMoney()
	{
		coins = PlayerPrefs.GetInt(coinKEY, 0);
	}

	public void SaveIsRelease()
	{
		for (int i = 0; i < isRelease.Length; i++)
		{
			// 각 bool 값을 1 또는 0으로 저장
			PlayerPrefs.SetInt("isRelease_" + i, isRelease[i] ? 1 : 0);
		}
		PlayerPrefs.Save();  // 변경 사항을 즉시 저장
	}

	public void LoadIsRelease()
	{
		for (int i = 0; i < isRelease.Length; i++)
		{
			// 저장된 int 값을 불러와서 bool로 변환
			isRelease[i] = PlayerPrefs.GetInt("isRelease_" + i, 0) == 1;
		}
	}


	public void OnClick_ResetAction()
	{
		PlayerPrefs.SetInt(coinKEY, 0);
		for (int i = 0; i < isRelease.Length; i++)
		{
			// 각 bool 값을 0으로 저장
			PlayerPrefs.SetInt("isRelease_" + i, 0);
		}
		PlayerPrefs.Save();  // 변경 사항을 즉시 저장
		LoadIsRelease();
		coins = 0;
		shopUIManager.UpdateCoinUI();
	}
}