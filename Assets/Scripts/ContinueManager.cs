using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueManager : MonoBehaviour
{
	/// 분기점 선택 이후
	/// 내가 선택하게된 결과 스토리 출력 후 계속하기 버튼 생성
	/// 계속하기 버튼이란
	/// 다음 스토리 재생을 누르는 버튼과도 같다.
	/// 즉, StorePlayManager에 있는 StoryPlay() 함수를 실행하면 된다.
	/// 그러기 위해서는 우선 StoryPlay()함수 내에 Switch문에서
	/// Battle 부분을 손봐야된다.
	/// 왜냐하면 Battle의 역할이
	/// Story
	/// Story
	/// Story
	/// Battle 
	/// 이렇게 될때 한번에 묶어서 배열로 출력하게 하고
	/// 그다음에 Battle은 전투하기 버튼을 만들어주면 되기 때문이다.
	/// ㄱ
	/// Reward도 해야되는데 귀찮다.
	/// 일단 Battle은 시스템을 만들지 않았으니깐
	/// 버튼 생성 Debug.Log()하는 것 까지만 마무리 해야겠다.
	/// 
	/// 계속하기 버튼이
	/// 눌렸을 때 Root2가 나올수 있다.
	/// 왜냐하면 Root1을 선택시에 Root2에 있는 부분을
	/// 무시하기 때문이다.
	/// 하지만 걱정하지 않아도 된다.
	/// switch문에서 Root1 또는 Root2 일시에 
	/// 그냥 StoryFlow++하고 재귀함수 반복하면 그만이다.
	/// 

	public StackTextManager stackTextManager;

	public void ContinueBtnMaker()
	{
		stackTextManager.AddButtonBelowLastText(2, "계속하기");
	}
}
