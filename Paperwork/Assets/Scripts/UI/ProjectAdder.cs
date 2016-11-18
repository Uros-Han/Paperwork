using UnityEngine;
using System.Collections;
using System;

public class ProjectAdder : MonoBehaviour {

    // Use this for initialization
    void Start () {
        

        transform.localPosition = new Vector3(0, 1143f);
        transform.localScale = Vector3.one;


        ProjectAdderSetting();
        iTween.MoveTo(gameObject, iTween.Hash("y", 0, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));
    }

    void ProjectAdderSetting()
    {
        //언락된 애들 프로젝트 정보 채우기
        for (int i = 0; i < GameMgr.getInstance.m_iUnlockProject; ++i)
        {
            InfoSetting(i);
        }

        //언락 안된 애들 세팅
        UnlockedClassSetting();
    }

    void InfoSetting(int iIdx)
    {
        Transform ClassTrans = transform.Find("Slots").GetChild(iIdx).transform;
        //프로젝트 칸 실선으로
        ClassTrans.GetChild(0).GetComponent<UISprite>().spriteName = "NewProjectFilled";

        //프로젝트 정보생성
        GameObject ProjectAdderInfo = ObjFactory.getInstance.ProjectAdderInfo(ClassTrans, iIdx);

        
    }

    void UnlockedClassSetting()
    {
        //돈 주고 뚫어야 할 차례인 프로젝트
        if (GameMgr.getInstance.m_iUnlockProject < 5)
        {
            Transform ClassNeedOpenTrans = transform.Find("Slots").GetChild(GameMgr.getInstance.m_iUnlockProject).transform;

            ObjFactory.getInstance.ProjectAdderNeedMoneyLabel(ClassNeedOpenTrans, GameMgr.getInstance.m_iUnlockMoney[GameMgr.getInstance.m_iUnlockProject]);
            
        }

        //아직 사지도 못하는애들 콜라이더 빼주기
        for (int i = GameMgr.getInstance.m_iUnlockProject + 1; i < 5; ++i)
        {
            transform.Find("Slots").GetChild(i).GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void CloseAdder()
    {
        StartCoroutine(CloseAdderCoroutine());
    }

    IEnumerator CloseAdderCoroutine()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 1143, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void DeleteError()
    {
        Destroy(transform.Find("ProjectAdderError(Clone)").gameObject);
    }

    public void TouchProject()
    {
        int iTouchedClassIdx = Convert.ToInt32(UICamera.selectedObject.name);

        if (UICamera.selectedObject.GetComponent<UISprite>().spriteName.Equals("NewProjectEmpty")) // 안열린 프로젝트 뚫기
        {
            if (GameMgr.getInstance.m_iMoney >= GameMgr.getInstance.m_iUnlockMoney[iTouchedClassIdx]) //충분한 돈
            {
				ObjFactory.getInstance.MoneyEffect (GameMgr.getInstance.m_iUnlockMoney [iTouchedClassIdx], false);

                //언락! 정보 보여주기
                InfoSetting(iTouchedClassIdx);

                //언락 최신화
                GameMgr.getInstance.m_iUnlockProject += 1;
                PlayerPrefs.SetInt("UnlockProject", GameMgr.getInstance.m_iUnlockProject);

                //언락되있는 애들 갱신
                Destroy(GameObject.Find("NeedMoneyLabel(Clone)").gameObject);
                UnlockedClassSetting();

                if (iTouchedClassIdx + 1 < 5)
                    transform.Find("Slots").GetChild(iTouchedClassIdx + 1).GetChild(0).GetComponent<BoxCollider>().enabled = true;
            }
            else //돈 모잘라
            {
                ObjFactory.getInstance.ProjectAdderError(UICamera.selectedObject.transform.parent);
            }
        }
        else // 이미 뚫린 프로젝트 시작하기
        {
            if (GameMgr.getInstance.m_iMoney >= GameMgr.getInstance.m_iStartMoney[iTouchedClassIdx]) //프로젝트 시작하기 충분한 돈
            {
				ObjFactory.getInstance.ProjectDetailSetter (iTouchedClassIdx);
            }else//프로젝트 시작할돈 모자람
            {
                ObjFactory.getInstance.ProjectAdderError(UICamera.selectedObject.transform.parent);
            }
        }
    }
}
