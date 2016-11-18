using UnityEngine;
using System.Collections;

public class ProjectDetailSetter : MonoBehaviour {

    public int m_iClassIdx;
	GameObject m_InfoContent;

    int m_iEmployeeCount;
    int m_iCurAvailableEmployee;
    int m_iMaxAvailableEmployee;

	System.TimeSpan m_TimeToEnd;
	int m_iEstimatedRevenue;
	int m_iProjectValue;

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, 1143f);
        transform.localScale = Vector3.one;

		m_InfoContent = ObjFactory.getInstance.ProjectAdderInfo(transform.GetChild(3).GetChild(0), m_iClassIdx);

        iTween.MoveTo(gameObject, iTween.Hash("y", 0, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));

        //클래스 정보
        UILabel titleLabel = transform.Find("CreateNew").GetChild(0).GetChild(1).GetComponent<UILabel>();
        UISprite borderSprite = transform.Find("CreateNew").GetChild(0).GetChild(2).GetComponent<UISprite>();
        switch (m_iClassIdx)
        {
            case 0:
                titleLabel.text = string.Format(Localization.Get("ClassType"), "E");
                break;

            case 1:
                titleLabel.text = string.Format(Localization.Get("ClassType"), "C");
                borderSprite.color = new Color(0/255f, 112/255f, 178/255f);
                break;

            case 2:
                titleLabel.text = string.Format(Localization.Get("ClassType"), "B");
                borderSprite.color = new Color(233 / 255f, 168 / 255f, 0 / 255f);
                break;

            case 3:
                titleLabel.text = string.Format(Localization.Get("ClassType"), "A");
                borderSprite.color = new Color(203 / 255f, 0 / 255f, 0 / 255f);
                break;

            case 4:
                titleLabel.text = string.Format(Localization.Get("ClassType"), "S");
                borderSprite.color = new Color(165 / 255f, 0 / 255f, 160 / 255f);
                break;
        }

        //배치 가능한 인원 
		EmployeeSetting();

		RefreshInfo ();
    }

	public void EmployeeSetting()
	{
		m_iCurAvailableEmployee = GameMgr.getInstance.m_iEmployee;
		for (int i = 0; i < GameMgr.getInstance.m_ListProjectInProgress.Count; ++i)
		{
			m_iCurAvailableEmployee -= GameMgr.getInstance.m_ListProjectInProgress[i].m_iEmployee;
		}
		m_iMaxAvailableEmployee = m_iCurAvailableEmployee;
		m_iEmployeeCount = m_iCurAvailableEmployee;
		transform.Find("Deploying").Find("Employee").GetChild(0).GetComponent<UILabel>().text = Localization.Get("DeployableEmployee") + " : " + m_iMaxAvailableEmployee.ToString();
		transform.Find("Deploying").Find("Employee").GetChild(1).GetComponent<UILabel>().text = m_iCurAvailableEmployee.ToString();
	
		RefreshInfo ();
	}
		
	void RefreshInfo()
	{
		GameMgr gMgr = GameMgr.getInstance;
		UILabel BaseInfoLabel = m_InfoContent.transform.GetChild(0).GetComponent<UILabel>();
		UILabel MoneyInfoLabel = m_InfoContent.transform.GetChild(1).GetComponent<UILabel>();

		//공식 사용
		if (m_iCurAvailableEmployee != 0) {
			m_TimeToEnd = gMgr.m_timeToEnd [m_iClassIdx].Subtract (new System.TimeSpan (0, 0, m_iCurAvailableEmployee - 1));
			m_iEstimatedRevenue = gMgr.m_iStartMoney [m_iClassIdx] + ((m_iCurAvailableEmployee - 1) * gMgr.m_iProjectValue [m_iClassIdx] * 10);
			m_iProjectValue = m_iCurAvailableEmployee * gMgr.m_iProjectValue [m_iClassIdx];
		} else {
			m_TimeToEnd = gMgr.m_timeToEnd [m_iClassIdx];
			m_iEstimatedRevenue = gMgr.m_iStartMoney [m_iClassIdx];
			m_iProjectValue = gMgr.m_iProjectValue [m_iClassIdx];
		}

		if (m_iCurAvailableEmployee <= 1) {
			if (m_TimeToEnd.Minutes.Equals (0))
				BaseInfoLabel.text = "[000000]" + Localization.Get ("TimeToEnd") + m_TimeToEnd.Seconds + " " + Localization.Get ("Second") + "\n" + Localization.Get ("ProjectValue") + string.Format (Localization.Get ("Currency"), m_iProjectValue) + "\n" + Localization.Get ("EstimatedRevenue") + string.Format(Localization.Get("Currency"),m_iEstimatedRevenue);
			else
				BaseInfoLabel.text = "[000000]" + Localization.Get ("TimeToEnd") + m_TimeToEnd.Minutes + " " + Localization.Get ("Minute") + "  " + m_TimeToEnd.Seconds + " " + Localization.Get ("Second") + "\n" + Localization.Get ("ProjectValue") + string.Format (Localization.Get ("Currency"), m_iProjectValue) + "\n" + Localization.Get ("EstimatedRevenue") + string.Format(Localization.Get("Currency"),m_iEstimatedRevenue);
		} else {
			if (m_TimeToEnd.Minutes.Equals (0))
				BaseInfoLabel.text = "[000000]" + Localization.Get ("TimeToEnd") + "[007D32FF]" + m_TimeToEnd.Seconds + " " + Localization.Get ("Second") + "[-]" + "\n" + Localization.Get ("ProjectValue") + "[007D32FF]" + string.Format (Localization.Get ("Currency"), m_iProjectValue) + "[-]" + "\n" + Localization.Get ("EstimatedRevenue") + "[007D32FF]" + string.Format(Localization.Get("Currency"),m_iEstimatedRevenue) + "[-]";
			else
				BaseInfoLabel.text = "[000000]" + Localization.Get ("TimeToEnd") + "[007D32FF]" + m_TimeToEnd.Minutes + " " + Localization.Get ("Minute") + "  " + m_TimeToEnd.Seconds + " " + Localization.Get ("Second") + "[-]" + "\n" + Localization.Get ("ProjectValue") + "[007D32FF]" + string.Format (Localization.Get ("Currency"), m_iProjectValue) + "[-]" + "\n" + Localization.Get ("EstimatedRevenue") + "[007D32FF]" + string.Format(Localization.Get("Currency"),m_iEstimatedRevenue) + "[-]";
		}
	}
	
    public void Close()
    {
        StartCoroutine(CloseCoroutine());
    }

    IEnumerator CloseCoroutine()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 1143, "islocal", true, "time", 0.5f, "easetype", "easeOutBack"));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void EmployeeRightArrow()
    {
        if (m_iCurAvailableEmployee < m_iMaxAvailableEmployee)
        {
            m_iCurAvailableEmployee += 1;
            transform.Find("Deploying").Find("Employee").GetChild(1).GetComponent<UILabel>().text = m_iCurAvailableEmployee.ToString();

			RefreshInfo ();
        }
    }

    public void EmployeeLeftArrow()
    {
        if (m_iCurAvailableEmployee > 0)
        {
            m_iCurAvailableEmployee -= 1;
            transform.Find("Deploying").Find("Employee").GetChild(1).GetComponent<UILabel>().text = m_iCurAvailableEmployee.ToString();

			RefreshInfo ();
        }
    }

	public void HireBtn()
	{
		GameObject PopUpMsg = ObjFactory.getInstance.PopUpMsg (Localization.Get("HireApply"));
		PopUpMsg.GetComponent<PopupMsg>().onPressBtn += new PopupMsg.OnPressBtn(PopUpMsg.GetComponent<PopupMsg>().HireApply);
	}

	public void Confirm()
	{
		//배치 0명 하고 확인눌럿을 때
		if (m_iCurAvailableEmployee.Equals (0)) {
			GameObject PopUpMsg = ObjFactory.getInstance.PopUpMsg (Localization.Get("NeedEmployee"), POPUP_TYPE.CONFIRM);
			PopUpMsg.GetComponent<PopupMsg>().onPressBtn += new PopupMsg.OnPressBtn(PopUpMsg.GetComponent<PopupMsg>().DestroyThisYesBtn);
			return;
		}

		StartCoroutine(CloseCoroutine());
		GameObject.Find ("ProjectAdder(Clone)").GetComponent<ProjectAdder> ().CloseAdder ();

		//새 프로젝트 생성
		Project_value newProject = new Project_value();
		newProject.m_startTime = System.DateTime.Now;
		newProject.m_endTime = System.DateTime.Now + m_TimeToEnd;
		newProject.m_iEmployee = m_iCurAvailableEmployee;
		newProject.m_iClass = m_iClassIdx;
		newProject.m_iCompleteRevenue = m_iEstimatedRevenue;
		newProject.m_iProjectValue = m_iProjectValue;
		newProject.m_strName = GameObject.Find("nameSlot").transform.GetChild(2).GetComponent<UILabel>().text;

		ObjFactory.getInstance.Project (newProject);

		GameMgr.getInstance.m_ListProjectInProgress.Add (newProject);
	}
}
