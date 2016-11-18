using UnityEngine;
using System.Collections;

public class MoneyEffect : MonoBehaviour {
	public int m_iMoney;
	public bool m_bAdd;

	// Use this for initialization
	void Start () {
		iTween.MoveTo(gameObject, iTween.Hash("y", -95f, "islocal", true, "time", 1f, "easetype", "easeInSine"));

		if (m_bAdd) {
			GetComponent<UILabel> ().text = "+ " +string.Format (Localization.Get ("Currency"), m_iMoney);
		}else{
			GetComponent<UILabel> ().text = "- " +string.Format (Localization.Get ("Currency"), m_iMoney);
			GetComponent<UILabel> ().color = new Color (225 / 255f, 50 / 255f, 0 / 255f);
		}

		StartCoroutine (Destroyer ());
	}
	
	IEnumerator Destroyer()
	{
		yield return new WaitForSeconds (1f);
		Destroy (gameObject);
	}

}
