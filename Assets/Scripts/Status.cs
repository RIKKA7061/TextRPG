using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject
{
    // 응 스크립터블 오브젝트 
    // 적, 플레이어 공통
    // 체력, 공격력, 회피율 만 존재함

    public string Job;      // 직업
    public Sprite icon;     // 이미지
    public int HP;          // 체력
    public int Atk;         // 공격력
    public int AvoidRate;   // 회피율
}
