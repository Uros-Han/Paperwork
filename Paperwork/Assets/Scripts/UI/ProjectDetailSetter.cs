using UnityEngine;
using System.Collections;

public class ProjectDetailSetter : MonoBehaviour {

    public int m_iClassIdx;

    int m_iEmployeeCount;
    int m_iCurAvailableEmployee;
    int m_iMaxAvailableEmployee;

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, 1143f);
        transform.localScale = Vector3.one;

        GameObject infoContent = ObjFactory.getInstance.ProjectAdderInfo(transform.GetChild(3).GetChild(0), m_iClassIdx);

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
        m_iCurAvailableEmployee = GameMgr.getInstance.m_iEmployee;
        for (int i = 0; i < GameMgr.getInstance.m_ListProjectInProgress.Count; ++i)
        {
            m_iCurAvailableEmployee -= GameMgr.getInstance.m_ListProjectInProgress[i].m_iEmployee;
        }
        m_iMaxAvailableEmployee = m_iCurAvailableEmployee;
        m_iEmployeeCount = m_iCurAvailableEmployee;
        transform.Find("Deploying").Find("Employee").GetChild(0).GetComponent<UILabel>().text = Localization.Get("DeployableEmployee") + " : " + m_iMaxAvailableEmployee.ToString();
        transform.Find("Deploying").Find("Employee").GetChild(1).GetComponent<UILabel>().text = m_iCurAvailableEmployee.ToString();
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
        }
    }

    public void EmployeeLeftArrow()
    {
        if (m_iCurAvailableEmployee > 0)
        {
            m_iCurAvailableEmployee -= 1;
            transform.Find("Deploying").Find("Employee").GetChild(1).GetComponent<UILabel>().text = m_iCurAvailableEmployee.ToString();
        }
    }
}
