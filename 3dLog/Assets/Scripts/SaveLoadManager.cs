using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public static class SaveLoadManager
{
    public static string folderName = "UnityLog";
    public static string fileName = "LogData";
    public static string fileExtension = ".sav";

    public static void Save()
    {
        Directory.CreateDirectory(GetBinaryFilePath());
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(GetBinaryFilePath() + fileName + fileExtension, FileMode.Create);

        GameObject[] _all = GameObject.FindGameObjectsWithTag(ConstData.MEMO_TAG_NAME);
        ConfirmData[] _data = new ConfirmData[_all.Length];

        for (int i = 0; i < _all.Length; i++)
        {
            ConfirmDataStruct _copy = _all[i].GetComponent<ConfirmDataStruct>();
            float _x = _all[i].transform.position.x;
            float _y = _all[i].transform.position.y;
            float _z = _all[i].transform.position.z;
            //int _uniqueNum = _copy.uniqueNumber;
            //int _value = _copy.relativeImportanceValue;
            //string _text = _copy.feedbackText;
            //string _date = _copy.writeDate;
            //List<ConfirmMemberData> _member = _copy.confirmMember;
            //List<bool> _keyword = _copy.confirmKeyword;

            //ConfirmData _newData = new ConfirmData(_uniqueNum, _x, _y, _z, _value, _text, _date, _member, _keyword);
            ConfirmData _newData = new ConfirmData(_copy, _x, _y, _z);

            _data[i] = _newData;
        }

        bf.Serialize(stream, _data);
        stream.Close();
    }

    public static void DataLoad()
    {
        Load();
    }

    public static ConfirmData[] Load()
    {
        string _path = GetBinaryFilePath() + fileName + fileExtension;

        if (File.Exists(_path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(GetBinaryFilePath() + fileName + fileExtension, FileMode.Open);
            ConfirmData[] data = bf.Deserialize(stream) as ConfirmData[];
            stream.Close();
            return data;
        }
        else
        {
            Save();
            return null;
        }
    }

    public static string GetBinaryFilePath()
    {
        return "C:/Users/" + ((Application.persistentDataPath).Split('/')[2]) + "/Desktop/" + folderName + "/";
    }
}
