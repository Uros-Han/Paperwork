using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

	private bool m_bPressed = false;

	public int m_iTagIdx;
    public bool m_bAddingTag;

	// Use this for initialization
	void Start () {
        gameObject.transform.parent = GameObject.Find("Teams").transform;
        transform.localPosition = new Vector3(360f, 0);
        transform.localScale = Vector3.one;

		transform.GetChild (0).localPosition = new Vector2 (-360f, 350f - (m_iTagIdx * 200));
	}
	
	// Update is called once per frame
	void Update () {
		if (m_bPressed) {
			Vector3 mousePos = UICamera.mainCamera.ScreenToWorldPoint (Input.mousePosition);
			Vector3 localMousePos = transform.InverseTransformPoint (mousePos);

			transform.position = new Vector2 (mousePos.x + (transform.position.x - transform.GetChild (0).position.x), transform.position.y);
			if (transform.localPosition.x > 360f)
				transform.localPosition = new Vector2 (360f, 0);
			else if(transform.localPosition.x < -360f)
				transform.localPosition = new Vector2 (-360f, 0);
			
			
			if(localMousePos.y < 350f && localMousePos.y > -350f)
				transform.GetChild (0).position = new Vector2 (transform.GetChild (0).position.x, mousePos.y);

			//태그위치 조정
			if (localMousePos.y > 350f - (m_iTagIdx * 200) + 100f) { //태그 위로보냄
				if (m_iTagIdx.Equals (0))
					return;
				else {
					Team AboveTagTeam = FindTeamByIdxTag (m_iTagIdx - 1);
					if (AboveTagTeam == null)
						return;
					
					AboveTagTeam.m_iTagIdx += 1;
					AboveTagTeam.MoveTagByIdx (AboveTagTeam.m_iTagIdx);

					m_iTagIdx -= 1;
				}
			}else if(localMousePos.y < 350f - (m_iTagIdx * 200) - 100f) { //태그 밑으로보냄
				if (m_iTagIdx.Equals (2))
					return;
				else {
					Team AboveTagTeam = FindTeamByIdxTag (m_iTagIdx + 1);
					if (AboveTagTeam == null)
						return;
					
					AboveTagTeam.m_iTagIdx -= 1;
					AboveTagTeam.MoveTagByIdx (AboveTagTeam.m_iTagIdx);

					m_iTagIdx += 1;
				}
			}

		} 
	}

	Team FindTeamByIdxTag(int idx)
	{
		for (int i = 0; i < transform.parent.childCount; ++i) {
			if (transform.parent.GetChild (i).GetComponent<Team> ().m_iTagIdx.Equals(idx))
            {
                if (transform.parent.GetChild(i).GetComponent<Team>().m_bAddingTag.Equals(false))
                    return transform.parent.GetChild(i).GetComponent<Team>();
                else
                    return null;
            }
		}

		return null;
	}

	public void MoveTagByIdx(int idx)
	{
		TweenPosition tw = transform.GetChild(0).GetComponent<TweenPosition> ();
		tw.from = transform.GetChild (0).localPosition;
		tw.to = new Vector2 (-360f, 350f - (idx * 200));
		tw.ResetToBeginning ();
		tw.PlayForward ();
	}

	public void TagPressed()
	{
        if (m_bAddingTag)
        {
            GameObject PopUpMsg = ObjFactory.getInstance.PopUpMsg(Localization.Get("CreateNewTeam"));
            PopUpMsg.GetComponent<PopupMsg>().onPressBtn += new PopupMsg.OnPressBtn(PopUpMsg.GetComponent<PopupMsg>().CreateTeam);

            return;
        }

		m_bPressed = true;
	}

	public void TagReleased()
	{
		m_bPressed = false;
		MoveTagByIdx (m_iTagIdx);

		Vector3 TagToScreen =  UICamera.mainCamera.WorldToScreenPoint(transform.GetChild(0).position);
		Debug.Log (TagToScreen);

		if (Screen.width * 0.3f > TagToScreen.x) {
			TweenPosition tw = GetComponent<TweenPosition> ();
			tw.from = transform.localPosition;
			tw.to = new Vector3(-310, 0);
			tw.ResetToBeginning ();
			tw.PlayForward ();
			Debug.Log ("Stick Left");
		}else if(Screen.width * 0.7f < TagToScreen.x)
		{
			TweenPosition tw = GetComponent<TweenPosition> ();
			tw.from = transform.localPosition;
			tw.to = new Vector3(360f, 0);
			tw.ResetToBeginning ();
			tw.PlayForward ();
			Debug.Log ("Stick Right");
		}
	}

    int m_iCurAddProject = -1;
    public void NewProject()
    {
        switch (UICamera.selectedObject.name)
        {
            case "Slot0":
                m_iCurAddProject = 0;
                break;

            case "Slot1":
                m_iCurAddProject = 1;
                break;

            case "Slot2":
                m_iCurAddProject = 2;
                break;
        }
        ObjFactory.getInstance.ProjectAdder();

    }

}
