using UnityEngine;
using System.Collections;

public class MainPanel : MonoBehaviour {

	GameMgr gMgr;

	UILabel EmployeeLabel;
	UILabel FundLabel;


	// Use this for initialization
	void Start () {
		gMgr = GameMgr.getInstance;

		EmployeeLabel = transform.GetChild (1).Find ("EmployeeCount").GetComponent<UILabel> ();
		FundLabel = transform.GetChild (1).Find ("Fund").GetComponent<UILabel> ();

		StartCoroutine (Loop ());
	}

	void OnDestory()
	{
		StopAllCoroutines ();
	}
	
	// Update is called once per frame
	IEnumerator Loop () {
		do {
			FundLabel.text = gMgr.m_iMoney.ToString ();
			EmployeeLabel.text = gMgr.m_iEmployee.ToString ();

			yield return null;
		} while(true);
	}
}
