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
		stackTextManager.AddButtonBelowLastText(4, $"{JwangNum} �� ����"); //BtnText
	}

	public void SetJwangUI()
	{
		Jwang.text = JwangNum.ToString() + "��"; // UI ����
	}
}
