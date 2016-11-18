using UnityEngine;
using System.Collections;
using System;

public class Project : MonoBehaviour {
	
	public Project_value m_project;

	UILabel m_percentLabel;
	UISprite m_progressBar;
	int m_iProgress;

	// Use this for initialization
	void Start () {
		transform.GetChild (1).GetComponent<UILabel> ().text = m_project.m_strName;

		UILabel classLabel = transform.GetChild (2).GetComponent<UILabel> ();
		switch (m_project.m_iClass)
		{
		case 0:
			classLabel.text = string.Format(Localization.Get("ClassType"), "E");
			break;

		case 1:
			classLabel.text = string.Format(Localization.Get("ClassType"), "C");
			classLabel.color = new Color(0/255f, 112/255f, 178/255f);
			break;

		case 2:
			classLabel.text = string.Format(Localization.Get("ClassType"), "B");
			classLabel.color = new Color(233 / 255f, 168 / 255f, 0 / 255f);
			break;

		case 3:
			classLabel.text = string.Format(Localization.Get("ClassType"), "A");
			classLabel.color = new Color(203 / 255f, 0 / 255f, 0 / 255f);
			break;

		case 4:
			classLabel.text = string.Format(Localization.Get("ClassType"), "S");
			classLabel.color = new Color(165 / 255f, 0 / 255f, 160 / 255f);
			break;
		}

		transform.GetChild (3).GetChild (1).GetComponent<UILabel> ().text = ": " + m_project.m_iEmployee + " " + Localization.Get("EmployeeUnit") + "\n" + ": " + string.Format(Localization.Get("Currency"), m_project.m_iProjectValue)+ "\n";

		m_percentLabel = transform.GetChild (3).GetChild (2).GetComponent<UILabel> ();
		m_percentLabel.text = ": " + m_iProgress + " %";

		m_progressBar = transform.GetChild (0).GetComponent<UISprite> ();

		StartCoroutine (PercentCalculate ());
	}

	IEnumerator PercentCalculate () {
		while (DateTime.Now < m_project.m_endTime) {
			TimeSpan originSpan = m_project.m_endTime - m_project.m_startTime;
			TimeSpan curSpan = DateTime.Now - m_project.m_startTime;
			TimeSpan remainSpan = m_project.m_endTime - DateTime.Now;

			float fillAmout = (float)(curSpan.TotalSeconds / originSpan.TotalSeconds);
			m_iProgress = (int)(fillAmout * 100);

			if (remainSpan.Minutes.Equals (0))
				m_percentLabel.text = ": " + m_iProgress + " %" + "\n" + " ( " + Localization.Get ("TimeRemaining") + " : " + remainSpan.Seconds + Localization.Get ("Second") + " )";
			else
				m_percentLabel.text = ": " + m_iProgress + " %" + "\n" + " ( " + Localization.Get ("TimeRemaining") + " : " + remainSpan.Minutes + Localization.Get ("Minute") + remainSpan.Seconds + Localization.Get ("Second") + " )";

			m_progressBar.fillAmount = fillAmout;

			yield return null;
		}

		//시간 다 되었다.
		ObjFactory.getInstance.MoneyEffect (m_project.m_iCompleteRevenue, true);
		GameMgr.getInstance.m_ListProjectInProgress.Remove(m_project);
		Destroy (gameObject);
	}

	void OnClick()
	{
		ObjFactory.getInstance.MoneyEffect (m_project.m_iProjectValue, true);
	}
}
