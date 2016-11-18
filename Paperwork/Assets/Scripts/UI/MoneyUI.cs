using UnityEngine;
using System.Collections;

public class MoneyUI : MonoBehaviour {
    GameMgr gMgr;
    UILabel label;
	// Use this for initialization
	void Start () {
        gMgr = GameMgr.getInstance;
        label = GetComponent<UILabel>();
    }
	
	// Update is called once per frame
	void Update () {
		label.text = string.Format (Localization.Get ("Currency"), gMgr.m_iMoney);
    }
}
