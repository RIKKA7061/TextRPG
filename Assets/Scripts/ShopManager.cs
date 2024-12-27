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
		// �̱��� ����: ShopManager �ν��Ͻ��� �̹� �����ϸ� �ı�
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
			// �� bool ���� 1 �Ǵ� 0���� ����
			PlayerPrefs.SetInt("isRelease_" + i, isRelease[i] ? 1 : 0);
		}
		PlayerPrefs.Save();  // ���� ������ ��� ����
	}

	public void LoadIsRelease()
	{
		for (int i = 0; i < isRelease.Length; i++)
		{
			// ����� int ���� �ҷ��ͼ� bool�� ��ȯ
			isRelease[i] = PlayerPrefs.GetInt("isRelease_" + i, 0) == 1;
		}
	}


	public void OnClick_ResetAction()
	{
		PlayerPrefs.SetInt(coinKEY, 0);
		for (int i = 0; i < isRelease.Length; i++)
		{
			// �� bool ���� 0���� ����
			PlayerPrefs.SetInt("isRelease_" + i, 0);
		}
		PlayerPrefs.Save();  // ���� ������ ��� ����
		LoadIsRelease();
		coins = 0;
		shopUIManager.UpdateCoinUI();
	}
}