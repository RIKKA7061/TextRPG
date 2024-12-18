using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	/// <summary>
	/// BattleManager란 배틀시에 일어나는 일을 관리하는 매니저를 말합니다.
	/// 예를 들어,
	/// 전투 시작 버튼 생성 -> 클릭 -> 전투 ->
	/// 일단 전투 하기 버튼을 생성하는 일에 중점을 둡시다.
	/// 그 다음, 클릭했을 때 전투 창을 True로 해서 보이게 한뒤
	/// 전투 시스템을 만들면 됩니다.
	/// 자, 버튼 만드는 일에 책임을 집니다. 
	/// 왜냐? 전투 시스템인 Battle과 관련이 있기 때문입니다.
	/// 
	/// 버튼액션에서 전투하기 버튼 클릭시
	/// 전투 창 보이게 하기 -> 적의 모습과 스테이터스, 플레이어의 모습과 스테이터스가 있겠죠?
	/// 전투는 당연히 철저하게 턴제 구성으로 이루어져 있다.
	/// 플레이어턴 -> 적턴 -> 플레이어턴
	/// 
	/// 
	/// UI
	/// [상단]
	/// 적 스테이터스 UI는
	/// 적의 이미지
	/// 적의 이름
	/// 그리고 적의 체력
	/// 
	/// [중단]
	/// 자동 스크롤 텍스트 창
	/// ~피해를 입혔다. (플레이어턴)
	/// 적이 공격해서 ~ 피해를 입었다. (적 턴)
	/// ~회피의 물약으로 ~ 회피율 상승을 하였다. (플레이어 턴)
	/// 적의 공격을 회피하였다. (적 턴)
	/// 적이 &플레이어 직업&의 공격을 회피하였다. (플레이어 턴)
	/// 
	/// [하단]
	/// 플레이어 스테이터스 UI는
	/// 플레이어 icon 이미지
	/// 플레이어 직업 이름
	/// 그리고 플레이어의 체력
	/// 밑에 왼쪽 오른쪽 버튼
	/// 왼쪽은 공격하기, 오른쪽은 회피율 상승
	/// 
	/// 플레이어 턴
	/// 일때는 두가지 선택권이 있다.
	/// 1. 공격하기 2. 회피율 상승
	/// 
	/// 적 턴
	/// 일때는 공격하기 가 주어진다.
	/// 
	/// 
	/// 시뮬레이션 
	/// 선 플레이어 턴시 질문을 한다.
	/// 전투가 시작되었다. (디스플레이)
	/// 어떤 선택을 할 것인가? (디스플레이)
	/// 플레이어 공격하기 버튼 클릭 (플레이어 턴)
	/// 적에게 공격으로 20 피해를 입혔다. or 적이 &플레이어 직업&의 공격을 피했다. (디스플레이)
	/// &플레이어 직업& 턴 종료 (디스플레이)
	/// (몬스터가 1초 있다가 공격 실행) (적 턴)
	/// &적 이름&의 공격으로 20 피해를 입었다. or &적 이름&의 공격을 회피했다. (디스플레이)
	/// 적 턴 종료 (디스플레이)
	/// 플레이어 회피율 상승 버튼 클릭 (플레이어 턴)
	/// 회피율이 상승되었다. (디스플레이)
	/// &플레이어 직업& 턴 종료 (디스플레이)
	/// (몬스터가 1초 있다가 공격 실행) (적 턴)
	/// &적 이름&의 공격으로 20 피해를 입었다. or &적 이름&의 공격을 회피했다. (디스플레이)
	/// 적 턴 종료 (디스플레이)
	/// 플레이어 공격하기 버튼 클릭 (플레이어 턴)
	/// 적에게 공격으로 20 피해를 입혔다. or 적이 &플레이어 직업&의 공격을 피했다. (디스플레이)
	/// 적 체력이 0 이하 일 경우 -> 전투 끝나고 다음 스토리 진행
	/// 플레이어 체력이 0 이하 일 경우 -> 패배 엔딩 재생
	/// 
	/// 일단 -> 전투 시스템 만드는 것이 목적이니
	/// 전투 도중 체력 0이하 조건에서 발생하는 스토리 재생, 패배 엔딩 부분은 
	/// Debug.Log()로 일단 대체합니다.
	/// 
	/// </summary>

	public StackTextManager stackTextManager;
	public PlayerStatManager playerStatManager;
	public EnemyStatManager enemyStatManager;
	public GameObject Battle_Chwang;
	public BattleDisplayManager battleDisplayManager;

	public void BattleBtnMaker(string EnemyInfos)
	{
		stackTextManager.AddButtonBelowLastText(3, EnemyInfos);
	}

	public void BattlePlay(string EnemyInfo)
	{
		// 플레이어 스탯 적용 전투 화면에 적용
		playerStatManager.SetPlayerBattleUI();

		// 적 스탯 적용 전투 화면에 적용
		string [] Infos = EnemyInfo.Split(':');
		Debug.Log(Infos[0]);
		Debug.Log(Infos[1]);

		int index = int.Parse(Infos[1]);
		enemyStatManager.SetEnemyStat(index);
		enemyStatManager.SetEnemeyBattleUI();

		Battle_Chwang.SetActive(true); // 전투창 키기

		// 전투 로직 실행
		battleDisplayManager.BattleLogic();
	}
}
