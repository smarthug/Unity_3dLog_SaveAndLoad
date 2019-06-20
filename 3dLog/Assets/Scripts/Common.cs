using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common{

}

[System.Serializable]
public class ConfirmMemberData
{
    public int ConfirmMember;   // 대상자
    public bool ConfirmComplete;    // 완료여부
}

[System.Serializable]
public enum ConfirmKeyword { None, Color, Texture, Pos, Light, Bug, Table, Frame, etc }

interface IUserInterfaceRefresh {
    void Refresh();
};
