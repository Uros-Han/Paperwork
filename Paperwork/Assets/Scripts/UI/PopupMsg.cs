using UnityEngine;
using System.Collections;

public class PopupMsg : MonoBehaviour {

    public delegate void OnPressBtn(string str);
    public OnPressBtn onPressBtn;
    public POPUP_TYPE m_popUpType;

    // Use this for initialization
    void Start()
    {
        transform.parent = GameObject.Find("UI Root").transform;
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, 812f);
        iTween.MoveTo(gameObject, iTween.Hash("y", 0, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));

        if (m_popUpType.Equals(POPUP_TYPE.CONFIRM))
        {
            transform.Find("Button_No").gameObject.SetActive(false);
            transform.Find("Button_Yes").transform.localPosition = new Vector3(0, -59);
        }
    }
    
    public void clickYes()
    {
        onPressBtn("Yes");
    }

    public void clickNo()
    {
        onPressBtn("No");
    }

    IEnumerator DestroyMsg()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 812f, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void CreateTeam(string str)
    {
        if (str.Equals("Yes"))
        {
            if (GameMgr.getInstance.m_iMoney >= 1000) //돈은 충분하다! 팀 구입하자
            {
                GameMgr.getInstance.m_iMoney -= 1000;

                //+모양 태그찾기
                Transform Teams = GameObject.Find("Teams").transform;
                GameObject AddingTag = null;
                for (int i = 0; i < Teams.childCount; ++i)
                {
                    if (Teams.GetChild(i).GetComponent<Team>().m_bAddingTag.Equals(true))
                    {
                        AddingTag = Teams.GetChild(i).gameObject;
                        break;
                    }
                }

                AddingTag.GetComponent<UIPanel>().depth = AddingTag.GetComponent<Team>().m_iTagIdx + 1;

                //태그 색입혀주기
                AddingTag.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Tag";
                switch (AddingTag.GetComponent<Team>().m_iTagIdx)
                {
                    case 0:
                        AddingTag.transform.GetChild(0).GetComponent<UISprite>().color = new Color(229 / 255f, 77 / 255f, 77 / 255f);
                        AddingTag.transform.GetChild(1).GetComponent<UISprite>().color = new Color(229 / 255f, 77 / 255f, 77 / 255f);
                        break;

                    case 1:
                        AddingTag.transform.GetChild(0).GetComponent<UISprite>().color = new Color(80 / 255f, 120 / 255f, 233 / 255f);
                        AddingTag.transform.GetChild(1).GetComponent<UISprite>().color = new Color(80 / 255f, 120 / 255f, 233 / 255f);
                        break;

                    case 2:
                        AddingTag.transform.GetChild(0).GetComponent<UISprite>().color = new Color(233 / 255f, 197 / 255f, 80 / 255f);
                        AddingTag.transform.GetChild(1).GetComponent<UISprite>().color = new Color(233 / 255f, 197 / 255f, 80 / 255f);
                        break;
                }

                //새 태그만들기
                if(AddingTag.GetComponent<Team>().m_iTagIdx < 2)
                    ObjFactory.getInstance.Tag(AddingTag.GetComponent<Team>().m_iTagIdx + 1);

                //다했으니가 애딩태그 부울변수 거짓해주자
                AddingTag.GetComponent<Team>().m_bAddingTag = false;

            }
            else //돈이모자라서;; 팀을 못사요;
            {
                GameObject PopUpMsg = ObjFactory.getInstance.PopUpMsg(Localization.Get("NeedMoreMoney"), POPUP_TYPE.CONFIRM);
                PopUpMsg.GetComponent<PopupMsg>().onPressBtn += new PopupMsg.OnPressBtn(PopUpMsg.GetComponent<PopupMsg>().NeedMoreMoney);
            }

            StartCoroutine(DestroyMsg());
        }
        else
        {
            StartCoroutine(DestroyMsg());
        }
    }

    public void NeedMoreMoney(string str)
    {
        if (str.Equals("Yes"))
        {
            StartCoroutine(DestroyMsg());
        }
    }
}
