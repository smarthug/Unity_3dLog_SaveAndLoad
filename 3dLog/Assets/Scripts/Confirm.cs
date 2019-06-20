using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Confirm : MonoBehaviour, IUserInterfaceRefresh {

    public static Confirm instance;

    private ConfirmKeyword m_EKeyword;
    private List<Toggle> m_KeywordToggles = new List<Toggle>();
    private ConfirmDataStruct copy;
    private int m_RelativeImportanceValue;

    public RectTransform KeywordPanel;

    [Header("Text")]
    public Text RelativeImportanceText;
    public Text WriteDate;
    public InputField FeedbackText;

    [Header("Dropdown and Toggle")]
    public RectTransform ConfirmUI;
    private Dropdown[] m_ConfirmDropdowns;
    private Toggle[] m_ConfirmToggls;

    [Header("Button")]
    public Button BtnClose;
    public Button BtnSave;
    public Button BtnDelete;

    private void Awake()
    {
        if (!instance)
            instance = this;

        FeedbackText = GetComponentInChildren<InputField>();
        m_ConfirmDropdowns = ConfirmUI.GetComponentsInChildren<Dropdown>();
        m_ConfirmToggls = ConfirmUI.GetComponentsInChildren<Toggle>();
    }

    private void Start()
    {
        Init();
        InitWrite();
    }

    public void Init()
    {
        m_ConfirmDropdowns[0].onValueChanged.AddListener(CheckSecondConfirmIsValid);
        m_ConfirmDropdowns[1].onValueChanged.AddListener(CheckSecondConfirmIsValid);

        BtnSave.onClick.AddListener(Save);
        BtnDelete.onClick.AddListener(Delete);
        BtnClose.onClick.AddListener(Close);

        CreateKeyword();

        DrawUI(false);
    }

    public void InitWrite()
    {
        SetImportanceValue(0);
        RelativeImportanceText.text = GetImportanceValue().ToString();
        FeedbackText.text = "";

        m_ConfirmDropdowns[0].value = 0;
        m_ConfirmDropdowns[1].value = 1;
        m_ConfirmDropdowns[2].value = 0;

        m_ConfirmToggls[0].isOn = false;
        m_ConfirmToggls[1].isOn = false;

        WriteDate.text = System.DateTime.Now.ToString();
    }

    #region Getter/Setter
    public int GetImportanceValue()
    {
        return m_RelativeImportanceValue;
    }
    public int SetImportanceValue(int _NewInt)
    {
        m_RelativeImportanceValue = _NewInt;
        return m_RelativeImportanceValue;
    }

    public Dropdown[] GetDropdowns()
    {
        return m_ConfirmDropdowns;
    }

    #endregion

    /// <summary>
    /// 작성한 메모를 포함하여 모든 메모를 저장합니다.
    /// </summary>
    public void Save()
    {
        GameObject[] _objs = GameObject.FindGameObjectsWithTag(ConstData.MEMO_TAG_NAME);

        for(int i=0; i<_objs.Length; i++)
        {
            copy.relativeImportanceValue = GetImportanceValue();
            copy.feedbackText = FeedbackText.text;
            copy.writeDate = WriteDate.text;
            copy.confirmMember = GetConfirmMember();
            copy.confirmKeyword = ConfirmKeyword();
        }

        SaveLoadManager.Save();
        Close();
    }

    /// <summary>
    /// 사용자가 클릭한 메모를 로드
    /// </summary>
    /// <param name="_data"></param>
    public void Load(ConfirmDataStruct _data)
    {
        DrawUI(true);
        copy = _data;

        RelativeImportanceText.text = SetImportanceValue(_data.relativeImportanceValue).ToString();
        FeedbackText.text = _data.feedbackText; // 피드백 텍스트 로드

        // 피드백 출처와 대상자 데이터 로드
        for (int i = 0; i < _data.confirmMember.Count; i++)
        {
            m_ConfirmDropdowns[i].value = _data.confirmMember[i].ConfirmMember;

            if (i != 0)
            {
                m_ConfirmToggls[i - 1].isOn = _data.confirmMember[i].ConfirmComplete;
            }
        }

        // 컨펌 대상자가 2차까지 있는지 확인
        CheckSecondConfirmIsValid(0);

        // 작성일 로드
        WriteDate.text = _data.writeDate;

        // 키워드 관련 로드
        for (int i = 0; i < m_KeywordToggles.Count; i++)
        {
            m_KeywordToggles[i].isOn = _data.confirmKeyword[i];
        }
    }

    /// <summary>
    /// 열람중인 메모 데이터를 삭제
    /// </summary>
    public void Delete()
    {
        Destroy(copy.gameObject);
        Close();
    }

    /// <summary>
    /// 컨펌 ui를 닫습니다. CloseButton에 사용
    /// </summary>
    public void Close()
    {
        DrawUI(false);
    }

    /// <summary>
    /// 컨펌 ui의 보이는 상태를 조작합니다.
    /// </summary>
    /// <param name="Visible"></param>
    public void DrawUI(bool Visible)
    {
        GetComponent<Canvas>().enabled = Visible;
        GetComponent<GraphicRaycaster>().enabled = Visible;
    }

    public int GetKeywordLength()
    {
        return Enum.GetValues(typeof(ConfirmKeyword)).Length;
    }

    /// <summary>
    /// 컨펌 시 사용되는 일부 키워드를 생성
    /// </summary>
    private void CreateKeyword()
    {
        // 키워드 관련 항목 생성이 되어 있지 않다면, 
        if (KeywordPanel.transform.childCount == 0)
        {
            int i = 0;
            int j = 0;
            GameObject _prefabs = Resources.Load<GameObject>("Prefabs/KeywordToggle");

            foreach (int data in Enum.GetValues(typeof(ConfirmKeyword)))
            {
                if (i == 0)
                {
                    i++;
                    continue;
                }

                if(i > ConstData.KEYWORD_MAX_LENGTH)
                {
                    break;
                }

                j = (i % 2 == 1 ? 0 : 1);

                // 생성 및 위치조절
                GameObject _obj = Instantiate(_prefabs, KeywordPanel.GetComponent<RectTransform>());
                RectTransform _rect = _obj.GetComponent<RectTransform>();
                _rect.localPosition = new Vector3((145f * (i - j)) + 80f, (-70f * j) - 10f, 0f);

                // 텍스트 할당
                ConfirmKeyword _key = (ConfirmKeyword)data;
                _obj.GetComponentInChildren<Text>().text = _key.ToString();

                i++;

                // 토글 초기화
                m_KeywordToggles.Add(_rect.GetComponent<Toggle>());
                _rect.GetComponent<Toggle>().isOn = false;
            }
        }
    }

    /// <summary>
    /// 컨펌 대상자가 다수여야하는지 체크
    /// </summary>
    /// <param name="i"></param>
    public void CheckSecondConfirmIsValid(int i)
    {
        /// 피드백 발행자와 1차 컨펌대상 모두 하청이면, 2차 컨펌자는 발행되지 않도록 함.
        /// 단, 피드백 발행자가 원청이고, 1차 컨펌대상이 하청이면, 2차 컨펌자 활성화.
        if (m_ConfirmDropdowns[0].value == 0 && m_ConfirmDropdowns[1].value != 0)
        {
            m_ConfirmDropdowns[2].gameObject.SetActive(true);
        }
        else
        {
            m_ConfirmDropdowns[2].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 작성되어진 컨펌 멤버 데이터를 반환
    /// </summary>
    /// <returns></returns>
    public List<ConfirmMemberData> GetConfirmMember()
    {
        List<ConfirmMemberData> _cMember = new List<ConfirmMemberData>();
        List<Dropdown> _drop = new List<Dropdown>();

        for(int i=0; i<m_ConfirmDropdowns.Length; i++)
        {
            _drop.Add(ConfirmUI.transform.GetChild(i).GetComponent<Dropdown>());
        }

        for (int i=0; i<_drop.Count; i++)
        {
            ConfirmMemberData _data = new ConfirmMemberData();

            _data.ConfirmMember = _drop[i].value;

            if (i != 0)
            {
                _data.ConfirmComplete = _drop[i].transform.GetChild(3).GetComponent<Toggle>().isOn;
            }
            else
            {
                _data.ConfirmComplete = false;
            }

            _cMember.Add(_data);
        }

        return _cMember;
    }

    /// <summary>
    /// 작성되어진 컨펌 키워드 데이터를 반환
    /// </summary>
    /// <returns></returns>
    public List<bool> ConfirmKeyword()
    {
        List<bool> _bool = new List<bool>();
        for(int i=0; i< m_KeywordToggles.Count; i++)
        {
            _bool.Add(KeywordPanel.transform.GetChild(i).GetComponent<Toggle>().isOn);
        }

        return _bool;
    }

    /// <summary>
    /// 씬의 모든 메모 데이터를 삭제
    /// </summary>
    public void Clear()
    {
        Close();
        DestroyPopup();
    }

    /// <summary>
    /// 모든 메모 데이터 삭제
    /// </summary>
    public void DestroyPopup()
    {
        GameObject[] _objs = GameObject.FindGameObjectsWithTag("Memo");
        for (int i = 0; i < _objs.Length; i++)
        {
            Destroy(_objs[i].gameObject);
        }
    }

    /// <summary>
    /// 우선순위의 증감
    /// </summary>
    public void Plus()
    {
        m_RelativeImportanceValue++;
        Refresh();
    }

    /// <summary>
    /// 우선순위의 증감
    /// </summary>
    public void Minus()
    {
        m_RelativeImportanceValue--;
        m_RelativeImportanceValue = (int)Mathf.Clamp(m_RelativeImportanceValue, 0, int.MaxValue);
        Refresh();
    }

    public void Refresh()
    {
        copy.relativeImportanceValue = GetImportanceValue();
        RelativeImportanceText.text = GetImportanceValue().ToString();
        copy.Refresh();
    }
}
