using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour {
	void Start () {
        // 4k 해상도 기준으로 제작
        float S_h = Screen.height / 2160f;
        GetComponent<CanvasScaler>().scaleFactor = S_h;
    }
}
