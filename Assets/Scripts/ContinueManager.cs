using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueManager : MonoBehaviour
{
	/// �б��� ���� ����
	/// ���� �����ϰԵ� ��� ���丮 ��� �� ����ϱ� ��ư ����
	/// ����ϱ� ��ư�̶�
	/// ���� ���丮 ����� ������ ��ư���� ����.
	/// ��, StorePlayManager�� �ִ� StoryPlay() �Լ��� �����ϸ� �ȴ�.
	/// �׷��� ���ؼ��� �켱 StoryPlay()�Լ� ���� Switch������
	/// Battle �κ��� �պ��ߵȴ�.
	/// �ֳ��ϸ� Battle�� ������
	/// Story
	/// Story
	/// Story
	/// Battle 
	/// �̷��� �ɶ� �ѹ��� ��� �迭�� ����ϰ� �ϰ�
	/// �״����� Battle�� �����ϱ� ��ư�� ������ָ� �Ǳ� �����̴�.
	/// ��
	/// Reward�� �ؾߵǴµ� ������.
	/// �ϴ� Battle�� �ý����� ������ �ʾ����ϱ�
	/// ��ư ���� Debug.Log()�ϴ� �� ������ ������ �ؾ߰ڴ�.
	/// 
	/// ����ϱ� ��ư��
	/// ������ �� Root2�� ���ü� �ִ�.
	/// �ֳ��ϸ� Root1�� ���ýÿ� Root2�� �ִ� �κ���
	/// �����ϱ� �����̴�.
	/// ������ �������� �ʾƵ� �ȴ�.
	/// switch������ Root1 �Ǵ� Root2 �Ͻÿ� 
	/// �׳� StoryFlow++�ϰ� ����Լ� �ݺ��ϸ� �׸��̴�.
	/// 

	public StackTextManager stackTextManager;

	public void ContinueBtnMaker()
	{
		stackTextManager.AddButtonBelowLastText(2, "����ϱ�");
	}
}
