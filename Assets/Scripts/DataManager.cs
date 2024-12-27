using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
	/// <summary>
	/// ���α׷��Ӵ� ������ ���� �ؾ��Ѵ�.
	/// 
	/// DataManager = ���丮 ������ �����
	/// ���丮 ������ = ���丮��ȣ, ���丮 ���� or ���丮��ȣ, �б������� (A/B) : ����
	/// 
	/// ���丮 ������ �� = ĳ���� ��
	/// ĳ���� �� = 4���� (����, ��������, ����, ���)
	/// 
	/// [���� ���ϴ� ��]
	/// ��ȹ�ڰ� ���� ��Ʈ���� ���丮 �𵨸�(?)��ȣ �� �ֱ⸸ �ϸ� �˾Ƽ� ���丮�� ��������� ��
	/// EX.
	/// �� ���� ĳ���� ���丮�� �����.
	/// �켱 1���� ���� ���丮, 2���� ��������, 3���� ����, 4���� ���
	/// // ��, �߰��� �ϸ� �ڿ������� ������ ���������~ �̸���
	/// �̷��� �ϴ� ������ ������ Ȯ�强�� �����ϴ�.
	/// ����̰� 1, 2�� �����ϰ� �����̰� 3, 4�� ���� �ϸ� �ǰ���?
	/// ���� ���丮 ��ȣ �ϳ� �����Ѵ��ص� ������ ���� ����ߵ�
	/// 
	/// �̾Ƽ� ���� �����ؾߵ�
	/// �� ĳ���� ������ �Ѵ� -> �������ڸ� ������ -> �������� 2�� �׸� -> 2���� �־� 
	/// 
	/// �ణ �̷��� �Ҳ��� 
	/// 1�� ī�װ�
	/// 2�� ���
	/// 
	/// ��, story ������ ����ؼ� ����ϰ�
	/// Question�Ͻÿ� 
	/// ������ 
	/// Dictionary<int, string, string> �Ҽ� ��������?
	/// �׷��� 1, story, ���丮1
	/// �̷������� ���ٵ� 
	/// �ȴ�.
	/// StoryData��� Ŭ������ ����� �� Ŭ������ �ش��ϴ� Dictionary�� ������ָ� �ȴ�.
	/// 
	/// �鹮�� �ҿ��ϰ� ���� C# �������� �Ẹ�ڴ�.
	/// 
	/// 
	/// public class StoryData
	/// {
	///		public string Key {get; set;}
	///		public string Value {get; set;}
	/// 
	///		public StoryData(string key, string value)
	///		{
	///			key = key;
	///			Value = value
	///		}
	/// }
	/// 
	/// Dictionary<int, StoryData> dictionary = new Dictionary <int, StoryData>();
	/// 
	/// dictionary[1] = new StoryData("story", "���丮1");
	/// dictionary[2] = new StoryData("question", "�б��� ������");
	/// 
	/// Console.WriteLine(dictionary[1].Key);   // "story" ���
	/// Console.WriteLine(dictionary[1].Value); // "���丮1" ���
	/// 
	/// 
	/// ĳ���� ���ýÿ� ���丮�� ������.
	/// ��, �� ĳ���͸��� ��� ��ȣ�� �ο��Ѵ�.
	/// 
	/// ��� ���丮�� �ľ��ϴ� ���� �ƹ����� ��ȣ �ִ� ���� �������ϱ� �ƿ� ���� ��������
	/// ��..
	/// 
	/// ���� ���� ĳ���͸� ���� �߾�!
	/// �׷��� �� ��� excel �ľ��� �����ؼ� Dictionary ���Ÿ��� �ݺ��ؼ� ���� ���丮 1~3������� �Ľ���
	/// �Ľ��� �� �� ������ ��ٸ��� �Ľ��� �� �ȴٸ� ���丮 ��� ����
	/// 
	/// ���丮 ��� ���Ŀ��� �� ���� ����
	/// case�� �������� �ְ��� ��
	/// story    ���丮
	/// question �б��� ������ ���� 
	/// root1    �б��� 1�� �� ���丮
	/// root2    �б��� 2�� �� ���丮
	/// battle   ���� A�� or B�� or ������
	/// reward	 ���� ����
	/// end		 �� ����
	/// 
	/// ���� �����մϴ�.
	/// �Ʊ� ���ڸ�� �Ѵٸ� mode ���� = 1 �س���
	/// switch���� ���Ͽ� mode�� 1�Ͻÿ�
	/// 
	/// 2, 3 ���� �ִ� �͵��� Dictionary Add ���ѳ��´�.
	/// �׷���
	/// Dictionary ��ȣ 1 2 3 4 �̷��� �귯���� �ѵڿ�
	/// string1 (story or question or root1 or...) �� �Ǻ��ѵڿ�
	/// story��� �ϳ��� generate�ؼ� �������� ���丮 �����ֱ� -> ���� generate
	/// question��� ���ñ� ���� -> ':' �������� ������ 1��ư, 2��ư �����Ѵ�. (��, �̶� ��ư�� ������ ÷�εǾ���Ԥ���)
	/// 1��ư Ŭ���� -> root1 generate
	/// 2��ư Ŭ���� -> root2 generate
	/// root1 -> storyó�� ���� generate -> ���� generate
	/// root2 -> storyó�� ���� generate -> ���� generate
	/// end -> N �� ��ư ��� (1�� - 2������)
	/// battle -> ���� �� �̸��� ������ �����´�. -> ��������
	/// reward -> ���� ����
	/// 
	/// </summary>

	
	

	public class StoryData
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public StoryData(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}

	static public Dictionary<int, StoryData> dictionary = new Dictionary<int, StoryData>();

	string SHEET; 
	const string url = "https://docs.google.com/spreadsheets/d/1rBiPujtyM8tAufwLgTpsxa_berG4MC5jN0wqpmlpUvg/export?format=tsv&range=A2:I1000";
	
	IEnumerator Start()
	{
		using (UnityWebRequest online_sheet = UnityWebRequest.Get(url))
		{
			yield return online_sheet.SendWebRequest();

			if (online_sheet.isDone)
			{
				SHEET = online_sheet.downloadHandler.text;
			}
		}
		//StoryDownLoading(1,2); // ����
		//StoryDownLoading(3,4); // ��������
		//StoryDownLoading(5,6); // ����
		//StoryDownLoading(7,8); // ���
	}


	public void StoryDownLoading(int Category, int Text)
	{
		string[] sheet = SHEET.Split("\n");

		dictionary.Clear();

		for (int i = 0; i < 1000; i++)
        {
			// �迭 ���� ����
			if (i >= sheet.Length)
			{
				//Debug.Log($"�ε��� {i} ������ �Ѿ��. {sheet.Length}");
				continue; // ���� �ݺ��� �ǳʶ�
			}

			string[] hang = sheet[i].Split("\t");

			StoryData storyData = new StoryData(hang[Category], hang[Text]);

			//Debug.Log(i + 1);
			//Debug.Log(storyData.Key);
			//Debug.Log(storyData.Value);

			dictionary.Add(i + 1, storyData);
		}
	}
}
