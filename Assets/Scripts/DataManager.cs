using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
	/// <summary>
	/// 프로그래머는 생각을 많이 해야한다.
	/// 
	/// DataManager = 스토리 데이터 저장소
	/// 스토리 데이터 = 스토리번호, 스토리 내용 or 스토리번호, 분기점선택 (A/B) : 보상
	/// 
	/// 스토리 나누는 것 = 캐릭터 별
	/// 캐릭터 수 = 4마리 (기자, 역사학자, 형사, 배우)
	/// 
	/// [내가 원하는 것]
	/// 기획자가 엑셀 시트에서 스토리 모델명(?)번호 를 넣기만 하면 알아서 스토리가 만들어지는 것
	/// EX.
	/// 열 별로 캐릭터 스토리를 만든다.
	/// 우선 1열은 기자 스토리, 2열은 역사학자, 3열은 형사, 4열은 배우
	/// // 즉, 추가만 하면 자연스럽게 게임이 만들어진다~ 이말씀
	/// 이렇게 하는 이유는 굉장히 확장성에 유연하다.
	/// 재욱이가 1, 2열 관리하고 재훈이가 3, 4열 관리 하면 되겠지?
	/// 만약 스토리 번호 하나 수정한다해도 지정이 별로 없어야됨
	/// 
	/// 뽑아서 쓸때 생각해야됨
	/// 자 캐릭터 선택을 한다 -> 역사학자를 선택함 -> 역사학자 2번 테마 -> 2열에 있어 
	/// 
	/// 약간 이렇게 할꺼야 
	/// 1열 카테고리
	/// 2열 대사
	/// 
	/// 즉, story 형식을 계속해서 출력하고
	/// Question일시에 
	/// 저장을 
	/// Dictionary<int, string, string> 할수 있으려나?
	/// 그러면 1, story, 스토리1
	/// 이런식으로 될텐데 
	/// 된다.
	/// StoryData라는 클래스를 만들고 그 클래스에 해당하는 Dictionary를 만들어주면 된다.
	/// 
	/// 백문이 불여일견 직접 C# 형식으로 써보겠다.
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
	/// dictionary[1] = new StoryData("story", "스토리1");
	/// dictionary[2] = new StoryData("question", "분기점 선택지");
	/// 
	/// Console.WriteLine(dictionary[1].Key);   // "story" 출력
	/// Console.WriteLine(dictionary[1].Value); // "스토리1" 출력
	/// 
	/// 
	/// 캐릭터 선택시에 스토리가 갈린다.
	/// 즉, 각 캐릭터마다 모드 번호를 부여한다.
	/// 
	/// 모든 스토리를 파씽하는 것은 아무래도 번호 넣는 것이 귀찮으니깐 아예 쉽게 구비하자
	/// 자..
	/// 
	/// 나는 기자 캐릭터를 선택 했어!
	/// 그러면 그 즉시 excel 파씽을 시작해서 Dictionary 짓거리를 반복해서 기자 스토리 1~3장까지만 파싱해
	/// 파싱이 다 될 때까지 기다리고 파싱이 다 된다면 스토리 재생 시작
	/// 
	/// 스토리 재생 이후에는 뭐 뻔히 알지
	/// case는 여기정도 있겠죠 뭐
	/// story    스토리
	/// question 분기점 선택지 제공 
	/// root1    분기점 1일 시 스토리
	/// root2    분기점 2일 시 스토리
	/// battle   전투 A몹 or B몹 or 보스몹
	/// reward	 보상 제공
	/// end		 장 종료
	/// 
	/// 모드는 간단합니다.
	/// 아까 기자모드 한다면 mode 변수 = 1 해놓고
	/// switch문을 통하여 mode가 1일시에
	/// 
	/// 2, 3 열에 있는 것들을 Dictionary Add 시켜놓는다.
	/// 그러고
	/// Dictionary 번호 1 2 3 4 이렇게 흘러가게 한뒤에
	/// string1 (story or question or root1 or...) 을 판별한뒤에
	/// story라면 하나씩 generate해서 스택으로 스토리 보여주기 -> 다음 generate
	/// question라면 선택권 제시 -> ':' 기준으로 나눠서 1버튼, 2버튼 생성한다. (단, 이때 버튼에 내용이 첨부되어야함ㅋㅋ)
	/// 1버튼 클릭시 -> root1 generate
	/// 2버튼 클릭시 -> root2 generate
	/// root1 -> story처럼 내용 generate -> 다음 generate
	/// root2 -> story처럼 내용 generate -> 다음 generate
	/// end -> N 장 버튼 출력 (1장 - 2장으로)
	/// battle -> 적의 몹 이름을 꺼내서 가져온다. -> 전투개시
	/// reward -> 보상 지급
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
		//StoryDownLoading(1,2); // 기자
		//StoryDownLoading(3,4); // 역사학자
		//StoryDownLoading(5,6); // 형사
		//StoryDownLoading(7,8); // 배우
	}


	public void StoryDownLoading(int Category, int Text)
	{
		string[] sheet = SHEET.Split("\n");
		dictionary.Clear();

		for (int i = 0; i < 1000; i++)
        {
			// 배열 길이 검증
			if (i >= sheet.Length)
			{
				//Debug.Log($"인덱스 {i} 범위를 넘어섰다. {sheet.Length}");
				continue; // 현재 반복을 건너뜀
			}

			string[] hang = sheet[i].Split("\t");

			StoryData storyData = new StoryData(hang[Category], hang[Text]);

			Debug.Log(i + 1);
			Debug.Log(storyData.Key);
			Debug.Log(storyData.Value);

			dictionary.Add(i + 1, storyData);
		}
	}
}
