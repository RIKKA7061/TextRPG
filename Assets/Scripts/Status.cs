using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject
{
    // �� ��ũ���ͺ� ������Ʈ 
    // ��, �÷��̾� ����
    // ü��, ���ݷ�, ȸ���� �� ������

    public string Job;      // ����
    public Sprite icon;     // �̹���
    public int HP;          // ü��
    public int Atk;         // ���ݷ�
    public int AvoidRate;   // ȸ����
}
