using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ConfirmData{

    public int uniqueNumber;

    public float position_X;
    public float position_Y;
    public float position_Z;

    public int relativeImportanceValue; // 작업 우선순위
    public string feedbackText; // 피드백 원문
    public string writeDate; // 피드백 작성일

    // 피드백관련 출처, 피드백 대상자 관련
    public List<ConfirmMemberData> confirmMember;

    // 피드백 키워드
    public List<bool> confirmKeyword;

    public ConfirmData(int uniqueNumber, float position_X, float position_Y, float position_Z, int relativeImportanceValue, string feedbackText, string writeDate, List<ConfirmMemberData> confirmMember, List<bool> confirmKeyword)
    {
        this.uniqueNumber = uniqueNumber;
        this.position_X = position_X;
        this.position_Y = position_Y;
        this.position_Z = position_Z;
        this.relativeImportanceValue = relativeImportanceValue;
        this.feedbackText = feedbackText;
        this.writeDate = writeDate;
        this.confirmMember = confirmMember;
        this.confirmKeyword = confirmKeyword;
    }

    public ConfirmData(ConfirmDataStruct _copy, float position_X, float position_Y, float position_Z)
    {
        this.uniqueNumber = _copy.uniqueNumber;
        this.position_X = position_X;
        this.position_Y = position_Y;
        this.position_Z = position_Z;
        this.relativeImportanceValue = _copy.relativeImportanceValue;
        this.feedbackText = _copy.feedbackText;
        this.writeDate = _copy.writeDate;
        this.confirmMember = _copy.confirmMember;
        this.confirmKeyword = _copy.confirmKeyword;
    }

}
