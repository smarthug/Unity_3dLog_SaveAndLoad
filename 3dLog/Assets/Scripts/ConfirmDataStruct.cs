using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ConfirmDataStruct : MonoBehaviour, IUserInterfaceRefresh {

    public int uniqueNumber;

    public int relativeImportanceValue; // 작업 우선순위
    public string feedbackText; // 피드백 원문
    public string writeDate; // 피드백 작성일

    // 피드백관련 출처, 피드백 대상자 관련
    public List<ConfirmMemberData> confirmMember;

    // 피드백 키워드
    public List<bool> confirmKeyword;

    public void ReadData()
    {
        Confirm.instance.Load(this);
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        GetComponentInChildren<Text>().text = relativeImportanceValue.ToString();
    }
}
