using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameMgr : MonoBehaviour {

    private static GameMgr instance;

    public static GameMgr getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameMgr)) as GameMgr;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("GameMgr");
                instance = obj.AddComponent(typeof(GameMgr)) as GameMgr;
            }

            return instance;
        }
    }

    public int m_iMoney;
    public int m_iUnlockProject;
    public int m_iEmployee;
    public List<Project> m_ListProjectInProgress;

    public DateTime[] m_timeToEnd;
    public int[] m_iProjectValue;
    public int[] m_iStartMoney;
    public int[] m_iUnlockMoney;
    
 

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        m_timeToEnd = new DateTime[5] { new DateTime(2016, 11, 3, 0, 00, 15) ,
                                        new DateTime(2016, 11, 3, 0, 00, 30) ,
                                        new DateTime(2016, 11, 3, 0, 01, 00) ,
                                        new DateTime(2016, 11, 3, 0, 01, 30) ,
                                        new DateTime(2016, 11, 3, 0, 03, 00) };

        m_iProjectValue = new int[5] { 1, 5, 10, 50, 100 };
        m_iStartMoney = new int[5] { 0, 100, 500, 1000, 5000 };
        m_iUnlockMoney = new int[5] { 0, 1000, 5000, 10000, 100000 };
        m_ListProjectInProgress = new List<Project>();

        ObjFactory.getInstance.Setting();

        SaveMgr.getInstance.GameData_Load();
    }

    void OnDestroy()
    {
        instance = null;
    }

    // Use this for initialization
    void Start () {
        Localization.language = "Korean";
    }
}


public class Project {
    public int m_iEmployee;


    public DateTime m_startTime;
    public DateTime m_endTime;
}
