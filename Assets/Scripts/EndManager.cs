using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    /// <summary>
    /// ���������� �Ѿ�� �Ŵ���
    /// �� ����ϱ� ��ư�̶� �Ȱ����� ���� �� ����� +1 �Ǵ� ���� �ٽ�
    /// ��� ���� (1,2,3)�� �Ŵ��� ���ҵ� �Ѵٰ� ������ ����
    /// </summary>
    /// 

    // ��
    public TextMeshProUGUI Jwang;
    public int JwangNum = 1;

	public StackTextManager stackTextManager;

	private void Start()
	{
		SetJwangUI();
	}

	public void EndBtnMaker()
	{
		JwangNum++; // �� �ѹ� UP
		SetJwangUI();
		stackTextManager.AddButtonBelowLastText(2, "���� �� ���丮");
	}

	public void SetJwangUI()
	{
		Jwang.text = JwangNum.ToString() + "��"; // UI ����
	}
}
