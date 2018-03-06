using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVNT_8TYMngr : MonoBehaviour, IState {

    #region Variables
    public List<EVNT_Data> Datas = new List<EVNT_Data>();
    EVNT_Data Data;   
    GameObject go_event;
    float f_multiplicator = 1f;
    public static EVNT_8TYMngr S;
    ICannonMngr infs_cannon;
    Loop CheckCondition;
    #endregion

    #region Routine Methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);        
    }
    private void Start()
    {
        infs_cannon = GL_CannonMngr.S;
    }
    #endregion    

    #region Interface implementation    
    public void StartLogic()
    {
        ChooseEvent();
        Data.SetDefault();
        go_event = Instantiate(Data.go_pattern, SetPosition(Data.i_level), transform.rotation);
    }
    public void EndLogic()
    {
        Data.i_finpoints = Mathf.RoundToInt(Data.f_score);
        GL_8TYMngr.S.AddScore(Data.i_finpoints);
        Data.i_level++;
        DestroyAll();
        Destroy(go_event);
    }
    public void MainLoop()
    {
        TimeCount();        
        CheckCondition();
        infs_cannon.AutoFire();
    }    
    public void DestroyAll()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetInf");
        foreach (GameObject go in targets)
        {
            Destroy(go);
        }
    }
    #endregion

    #region Assisst methods
    void ChooseEvent()
    {
        int i = Random.Range(0, Datas.Count );
        Data = Datas[i];
        CheckCondition = null;
        if (i == 0)
        {            
            CheckCondition += PatternCheckCondition;
            return;
        }
        if (i == 1)
        {
            CheckCondition += MeteoritCheckCondition;
            return;
        }
    }
    Vector3 SetPosition(int lvl)
    {
        Vector3 psn;
        switch (lvl)
        {
            case 1:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 30);
                }
                break;
            case 2:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 50);
                }
                break;
            case 3:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 70);
                }
                break;
            case 4:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 100);
                }
                break;
            case 5:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 140);
                }
                break;
            default:
                {
                    psn = GL_CannonMngr.S.transform.position + new Vector3(0, 2, 30);
                }
                break;
        }
        return psn;
    }
    void TimeCount()
    {
        Data.f_time -= Time.deltaTime;
    }
    void PatternCheckCondition()
    {
        if (Data.f_time <= 0)
        {
            Data.f_time = 0;
            GL_8TYMngr.S.EndEvent();
            return;
        }
        if (Data.i_quant_targets == 0)
        {
            if (Data.f_time <= 1)
                Data.f_score *= 1.1f;
            else
                Data.f_score *= (1.1f + (Data.f_time * 0.04f));
            GL_8TYMngr.S.EndEvent();
            return;
        }
    }
    void MeteoritCheckCondition()
    {
        EVNT_8TYMeteorit meteorit = go_event.GetComponent<EVNT_8TYMeteorit>();
        if (Data.f_time <= 0)
        {
            Data.f_time = 0;
            meteorit.StopAllCoroutines();
            GL_8TYMngr.S.EndEvent();
            return;
        }
        if (Data.i_quant_targets == 0)
        {
            meteorit.StopAllCoroutines();
            GL_8TYMngr.S.EndEvent();
        }
    }
    #endregion

    #region Setter Methods
    public void AddScore(float points)
    {
        Data.f_score += (points*f_multiplicator);        
    } 
    public void RemoveTarget()
    {
        Data.i_quant_targets--;
    }
    public void AddTime(float time)
    {
        Data.f_time += time;
    }
    public void MultiChange(float mult)
    {
        f_multiplicator = mult;
    }
    #endregion

    #region Accessor Methods
    public float GetDefaultTime()
    {
        return Data.default_time;
    }
    public float GetDefaultScore()
    {
        return Data.default_score;
    }
    public int GetScore()
    {
        return Mathf.RoundToInt(Data.f_score);
    }
    public float GetTime()
    {
        return Data.f_time;
    }
    public float GetMultiplicator()
    {
        return f_multiplicator;
    }
    #endregion

    #region private Class
    [System.Serializable]
    public class EVNT_Data
    {
        public float default_time = 20f;
        public int default_tgts = 35;
        public float default_score = 5f;
        public float f_score = 0f;
        public float f_time;
        public int i_quant_targets;
        public int i_finpoints;
        public int i_level = 1;
        public int i_ttl_score;
        public int i_ttl_tgts_destroyed;
        public GameObject go_pattern;

        public EVNT_Data(float time, int quantity, float score)
        {
            f_time = time;
            i_quant_targets = quantity;
            f_score = score;
        }
        public EVNT_Data()
        {
            f_time = 10f;
            i_quant_targets = 35;
            f_score = 0;
        }
        public void SetDefault()
        {
            f_score = 0;
            f_time = default_time;
            i_quant_targets = default_tgts;
        }
    }
    #endregion

}
