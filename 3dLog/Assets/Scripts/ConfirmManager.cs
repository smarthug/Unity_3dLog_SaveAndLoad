using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmManager : MonoBehaviour
{
    /// <summary>
    /// 현재 씬의 모든 메모를 저장합니다.
    /// </summary>
    public void SaveAll()
    {
        Confirm.instance.Close();
        SaveLoadManager.Save();
    }

    public void LoadAll()
    {
        Confirm.instance.Close();
        ConfirmData[] _data = SaveLoadManager.Load();

        GameObject[] _objs = GameObject.FindGameObjectsWithTag(ConstData.MEMO_TAG_NAME);
       
        for(int i=0; i<_objs.Length; i++)
        {
            Destroy(_objs[i].gameObject);
        }

        if(_data == null)
        {
            Debug.Log("로드 할 파일을 찾지 못했습니다.");
            return;
        }
        else
        {
            if(_data.Length == 0)
                Debug.Log("로드 할 파일은 찾았지만, 불러올 메모의 수가 0개 입니다.");

            else
                Debug.Log("로드 할 파일을 찾았습니다.\n" + _data.Length + "개의 메모를 불러옵니다.");
        }

        for(int i=0; i<_data.Length; i++)
        {
            GameObject _prefab = Instantiate(GetPrefab());
            ConfirmDataStruct _struct = _prefab.GetComponent<ConfirmDataStruct>();

            _struct.uniqueNumber = _data[i].uniqueNumber;
            _struct.GetComponent<RectTransform>().localPosition = new Vector3(_data[i].position_X, _data[i].position_Y, _data[i].position_Z);
            _struct.relativeImportanceValue = _data[i].relativeImportanceValue;
            _struct.feedbackText = _data[i].feedbackText;
            _struct.writeDate = _data[i].writeDate;
            _struct.confirmMember = _data[i].confirmMember;
            _struct.confirmKeyword = _data[i].confirmKeyword;
        }
    }

    /// <summary>
    /// 메모 오브젝트 생성
    /// </summary>
    public void Create()
    {
        int _NewUniqueNumber = CreateUniqueNumber();
        GameObject _obj = Instantiate(GetPrefab());
        //_obj.transform.position = new Vector3(0f, 0f, 0f);

        // 생성되어 있는 고유번호와 비교하여, 중복되지 않는 0번에 가까운 번호를 찾아 초기화 해준다.
        _obj.GetComponent<ConfirmDataStruct>().uniqueNumber = _NewUniqueNumber;

        ConfirmDataStruct _copy = _obj.GetComponent<ConfirmDataStruct>();

        // 메모의 생성 시간을 입력해준다.
        _copy.writeDate = System.DateTime.Now.ToString();

        // 우선순위 초기화
        _copy.relativeImportanceValue = 0;

        // 메모 작성자의 정보와 확인해야하는 대상자를 초기화 해준다.
        for(int i=0; i<3; i++)
        {
            _copy.confirmMember.Add(new ConfirmMemberData());

            if (i == 1)
            {
                _copy.confirmMember[1].ConfirmMember = 1;
            }
        }

        // 키워드 체크박스를 UnChecked로 초기화 한다.
        for(int i=0; i<Confirm.instance.GetKeywordLength(); i++)
        {
            _copy.confirmKeyword.Add(false);
        }
    }


    /// <summary>
    /// 프리팹 오브젝트를 리턴한다.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/MemoButton");
    }

    /// <summary>
    /// 기존의 고유번호와 비교하여, 0번에 가장 근접한 번호를 리턴한다.
    /// </summary>
    /// <returns></returns>
    private int CreateUniqueNumber()
    {
        // 0번에 가장 근접한 고유번호 생성을 위함.
        int _lowNum = 0;

        // 번호가 중복 여부에 따라 _lowNum의 증가와 루프가 필요한지 체크한다.
        bool _needIncreaseNumber = false;

        // 기존에 생성된 고유번호를 가져온다.
        int[] _copyNums = GetAllCreatedUniqueNumber();

        // 생성된 고유번호가 없으므로, 0을 반환한다.
        if (_copyNums.Length == 0)
            return 0;

        do
        {
            _needIncreaseNumber = false;

            for (int i = 0; i < _copyNums.Length; i++)
            {
                // 중복된 번호가 있다면, _lowNum을 증가시키고, 루프한다.
                if (_lowNum == _copyNums[i])
                {
                    _needIncreaseNumber = true;
                    _lowNum++;
                }
            }

        } while (_needIncreaseNumber);

        return _lowNum;
    }

    /// <summary>
    /// 기존에 생성된 고유번호를 반환한다.
    /// </summary>
    /// <returns></returns>
    private int[] GetAllCreatedUniqueNumber()
    {
        GameObject[] _nums = GameObject.FindGameObjectsWithTag(ConstData.MEMO_TAG_NAME);
        int[] _copyNums = new int[_nums.Length];

        // 메모 오브젝트의 고유번호를 리스트에 추가한다.
        for (int i = 0; i < _nums.Length; i++)
            _copyNums[i] = _nums[i].GetComponent<ConfirmDataStruct>().uniqueNumber;

        return _copyNums;
    }
}
