using UnityEngine;
using System.Collections;
using System;

public class ObjFactory : MonoBehaviour {
    private static ObjFactory instance;

    public static ObjFactory getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(ObjFactory)) as ObjFactory;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("ObjFactory");
                instance = obj.AddComponent(typeof(ObjFactory)) as ObjFactory;
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        instance = null;
    }


    GameObject _PopupMsg;
    GameObject _TeamPanel;

    GameObject _ProjectAdder;
    GameObject _ProjectAdderInfo;
    GameObject _ProjectAdderNeedMoneyLabel;
    GameObject _ProjectAdderError;
    GameObject _ProjectDetailSetter;

    public void Setting()
    {
        _PopupMsg = Resources.Load("Prefabs/Popup_YesNo") as GameObject;
        _TeamPanel = Resources.Load("Prefabs/TeamPanel/TeamPanel") as GameObject;

        _ProjectAdder = Resources.Load("Prefabs/ProjectAdder/ProjectAdder") as GameObject;
        _ProjectAdderInfo = Resources.Load("Prefabs/ProjectAdder/ProjectAdderInfo") as GameObject;
        _ProjectAdderNeedMoneyLabel = Resources.Load("Prefabs/ProjectAdder/NeedMoneyLabel") as GameObject;
        _ProjectAdderError = Resources.Load("Prefabs/ProjectAdder/ProjectAdderError") as GameObject;
        _ProjectDetailSetter = Resources.Load("Prefabs/ProjectAdder/ProjectDetailSetter") as GameObject;
    }

    public GameObject PopUpMsg(string strTitle, POPUP_TYPE type = POPUP_TYPE.YES_NO)
    {
        GameObject PopUpMsg = Instantiate(_PopupMsg) as GameObject;
        PopUpMsg.transform.Find("Title").GetComponent<UILabel>().text = strTitle;

        if (type.Equals(POPUP_TYPE.YES_NO))
            PopUpMsg.GetComponent<PopupMsg>().m_popUpType = POPUP_TYPE.YES_NO;
        else if (type.Equals(POPUP_TYPE.CONFIRM))
            PopUpMsg.GetComponent<PopupMsg>().m_popUpType = POPUP_TYPE.CONFIRM;

        return PopUpMsg;
    }

    public GameObject Tag(int iIdx)
    {
        GameObject Tag = Instantiate(_TeamPanel) as GameObject;
        Tag.GetComponent<Team>().m_iTagIdx = iIdx;
        Tag.GetComponent<Team>().m_bAddingTag = true;

        return Tag;
    }

    public GameObject ProjectAdder()
    {
        GameObject ProjectAdder = Instantiate(_ProjectAdder) as GameObject;
        ProjectAdder.transform.parent = GameObject.Find("UI Root").transform;


        return ProjectAdder;
    }

    public GameObject ProjectAdderInfo(Transform parent, int iIdx)
    {
        GameObject ProjectAdderInfo = Instantiate(_ProjectAdderInfo) as GameObject;
        ProjectAdderInfo.transform.parent = parent;
        ProjectAdderInfo.transform.localPosition = Vector3.zero;
        ProjectAdderInfo.transform.localScale = Vector3.one;

        //설정된 정보 프로젝트 정보에 넣어줌
        UILabel BaseInfoLabel = ProjectAdderInfo.transform.GetChild(0).GetComponent<UILabel>();
        UILabel MoneyInfoLabel = ProjectAdderInfo.transform.GetChild(1).GetComponent<UILabel>();
        GameMgr gMgr = GameMgr.getInstance;
        if (gMgr.m_timeToEnd[iIdx].Minute.Equals(0))
            BaseInfoLabel.text = Localization.Get("TimeToEnd") + gMgr.m_timeToEnd[iIdx].Second + " " + Localization.Get("Second") + "\n" + Localization.Get("ProjectValue") + gMgr.m_iProjectValue[iIdx] + " " + Localization.Get("Currency");
        else
            BaseInfoLabel.text = Localization.Get("TimeToEnd") + gMgr.m_timeToEnd[iIdx].Minute + " " + Localization.Get("Minute") + "  " + gMgr.m_timeToEnd[iIdx].Second + " " + Localization.Get("Second") + "\n" + Localization.Get("ProjectValue") + gMgr.m_iProjectValue[iIdx] + " " + Localization.Get("Currency");

        MoneyInfoLabel.text = Localization.Get("StartMoney") + gMgr.m_iStartMoney[iIdx] + " " + Localization.Get("Currency");

        return ProjectAdderInfo;
    }

    public GameObject ProjectAdderNeedMoneyLabel(Transform parent, int iNeedMoney)
    {
        GameObject ProjectAdderNeedMoneyLabel = Instantiate(_ProjectAdderNeedMoneyLabel) as GameObject;

        ProjectAdderNeedMoneyLabel.transform.parent = parent;
        ProjectAdderNeedMoneyLabel.transform.localPosition = new Vector3(0, -29f);
        ProjectAdderNeedMoneyLabel.transform.localScale = Vector3.one;

        ProjectAdderNeedMoneyLabel.GetComponent<UILabel>().text = string.Format(Localization.Get("ClassNeedMoney"), iNeedMoney, Localization.Get("Currency"));

        return ProjectAdderNeedMoneyLabel;
    }

    public GameObject ProjectAdderError(Transform parent)
    {
        GameObject ProjectAdderError = Instantiate(_ProjectAdderError) as GameObject;
        ProjectAdderError.transform.parent = parent;
        ProjectAdderError.transform.localPosition = Vector3.zero;
        ProjectAdderError.transform.localScale = Vector3.one;
        ProjectAdderError.GetComponent<TweenAlpha>().eventReceiver = ProjectAdderError;
        ProjectAdderError.GetComponent<TweenAlpha>().callWhenFinished = "_DestroyThis";

        return ProjectAdderError;
    }

    public GameObject ProjectDetailSetter(int iClassIdx)
    {
        GameObject ProjectDetailSetter = Instantiate(_ProjectDetailSetter) as GameObject;
        ProjectDetailSetter.GetComponent<ProjectDetailSetter>().m_iClassIdx = iClassIdx;

        return ProjectDetailSetter;
    }
}
