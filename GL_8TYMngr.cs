using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface InfinityMngr
{
    void ChangeLoops(IState newstate);
    float GetTime();
    void CheckLevel();
    void CheckCondition();
    int GetScore();
}
public delegate void Loop();
delegate float TimeCount();

public class GL_8TYMngr : MonoBehaviour, InfinityMngr {

    #region Variables
    
    IState currentState;    
    Dictionary<string, IState> states = new Dictionary<string, IState>();
    List<bool> b_lvls = new List<bool>();
    //bool b_isLose;
    public GL_8TYData Data;
    public static GL_8TYMngr S;    

    IState nextState;
    Loop CurrentLoop;
    #endregion

    #region Routine Methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);           
    }
    void Start()
    {
        StatesInitialization();
        currentState = states["Main"];            
        BoolLevelsInitialize();                
    }
    private void Update()
    {        
        currentState.MainLoop();
    }
    #endregion

    #region Management Methods
    public void EndGame()
    {
        //b_isLose = true;
        GL_CannonMngr.S.b_canControl = false;           
        GL_UI.S.LoseGame();
    }        
    IEnumerator StartPreEventTimer()
    {
        float time = Random.Range(10, 15);
        yield return new WaitForSeconds(time - 5);
        GL_UI.S.PreStart();
        yield return new WaitForSeconds(5f);
        StartEvent();
    }
    public void StartEvent()
    {
        ChangeLoops(states["Pattern"]);       
        GL_CannonMngr.S.SetCurrentWeapon(2);
        GL_UI.S.StartEvent();       
    }
    public void EndEvent()
    {
        GL_CannonMngr.S.SetCurrentWeapon(0);
        GL_UI.S.EndEvent();
        ChangeLoops(states["Main"]);
    }
    void SetNewLevel(int level)
    {
        if (b_lvls[level - 1] == false)
        {
            b_lvls[level - 1] = true;            
            StartCoroutine(GL_UI.S.ShowText("YOU ACHIEVED LEVEL " + level + "!", 2f, GL_UI.S.MAIN));
            Data.i_current_lvl = level;
            StartCoroutine(StartPreEventTimer());
        }
    }
    #endregion

    #region Assist Methods
    /// <summary>
    /// Initialize level progress status, in the begining only 1st is true
    /// </summary>
    void BoolLevelsInitialize()
    {
        for (int i = 0; i < 5; i++)
        {
            b_lvls.Add(false);
        }
        b_lvls[0] = true;
    }    
    public void ChangeLoops(IState newstate)
    {
        currentState.EndLogic();
        currentState = newstate;        
        currentState.StartLogic();
    }
    public void CheckLevel()
    {
        if (Data.i_score > 0 && Data.i_score < 50)
        {            
            return;
        }
        if (Data.i_score >= 50 && Data.i_score < 700)
        {            
            SetNewLevel(2);
        }
        if (Data.i_score >= 700 && Data.i_score < 1200)
        {            
            SetNewLevel(3);
        }
        if (Data.i_score >= 1200 && Data.i_score < 2500)
        {
            SetNewLevel(4);
        }
        if (Data.i_score >= 2500)
        {
            SetNewLevel(5);
        }
    }
    
    #endregion

    #region Accessor Methods
    public int GetCurrentLvl()
    {
        CheckLevel();
        return Data.i_current_lvl;
    }
    public int GetScore()
    {
        return Data.i_score;
    }
    public float GetTime()
    {
        return currentState.GetTime();        
    }
    public float GetMulti()
    {
        return currentState.GetMultiplicator();
    }
    #endregion

    #region Setter Methods
    public void AddScore(int score)
    {
        Data.i_score += score;
    }
    public void AddTime(float time)
    {
        currentState.AddTime(time);
    }
    public void SetMultiplicator(float mult)
    {
        foreach(KeyValuePair<string, IState> state in states)
        {
            state.Value.MultiChange(mult);
        }
    }
    #endregion

    #region UnderWork
    void StatesInitialization()
    {
        states.Add("Main", FindObjectOfType<TGT_8TYMngr>());
        states.Add("Pattern", FindObjectOfType<EVNT_8TYMngr>());
        //states.Add("Meteorit", FindObjectOfType<EVNT_8TYMeteorit>());
    }
    IState ChooseEvent()
    {
        int i = Random.Range(0, 2);
        if(i == 0)
        {
            return states["Pattern"];
        }
        if (i == 1)
            return states["Meteorit"];

        return states["Pattern"];
    }
    public void CheckCondition()
    {

    }
    public void OnGameStart()
    {

    }
    void EmptyMethod()
    { }
    #endregion
}


