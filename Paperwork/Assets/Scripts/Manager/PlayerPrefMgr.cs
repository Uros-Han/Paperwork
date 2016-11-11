using UnityEngine;
using System.Collections;

public class PlayerPrefMgr : MonoBehaviour {
    private static PlayerPrefMgr instance;

    public static PlayerPrefMgr getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(PlayerPrefMgr)) as PlayerPrefMgr;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("PlayerPrefMgr");
                instance = obj.AddComponent(typeof(PlayerPrefMgr)) as PlayerPrefMgr;
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

        PlayerPrefs.DeleteAll();
        //CheckPref();
    }

    void OnDestroy()
    {
        instance = null;
    }

    void CheckPref()
    {
        if (PlayerPrefs.GetInt("GamePlayCount").Equals(0)) //최초실행
        {
            //PlayerPrefs.SetInt("Money", 1000000);
            //PlayerPrefs.SetInt("UnlockProject", 1);
            //PlayerPrefs.SetInt("Employee", 1);
            SaveMgr.getInstance.GameData_Save();
        }

        //GameMgr.getInstance.m_iMoney = PlayerPrefs.GetInt("Money");
        //GameMgr.getInstance.m_iUnlockProject = PlayerPrefs.GetInt("UnlockProject");

        //PlayerPrefs.SetInt("GamePlayCount", PlayerPrefs.GetInt("GamePlayCount") + 1);
    }
	
}
