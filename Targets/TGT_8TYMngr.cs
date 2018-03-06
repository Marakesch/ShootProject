using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void StartLogic();
    void EndLogic();
    void MainLoop();
    void AddTime(float time);
    float GetTime();
    void MultiChange(float mult);
    float GetMultiplicator();
    void DestroyAll();
}

public class TGT_8TYMngr : MonoBehaviour, IState
{
    #region Variables
    public float f_time
    {
        get { return _f_time; }
        set
        {
            _f_time = Mathf.Clamp(value, 0, 500f);
        }
    }
    public float _f_time = 90f;
    public List<TGT_Data> targets = new List<TGT_Data>();
    public List<GL_3DNet> spaces = new List<GL_3DNet>();
    int i_current_target = 0;
    float f_lastYangle = 0;
    float f_multiplicator = 1;
    bool b_create_target = true;
    private bool b_time_finish = false;

    private IEnumerator inum_target_loop;
    ICannonMngr inter_cannon;
    //GameObject go_current_target;
    static public TGT_8TYMngr S;
    #endregion

    #region Routine Methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);
        inum_target_loop = TargetCreation();
    }
    void Start()
    {
        //go_current_target = targets[0].go_target;
        inter_cannon = GL_CannonMngr.S;
        foreach(TGT_Data data in targets)
        {
            data.i_level = 1;
        }
    }
    #endregion

    #region Creation Methods    
    void TimeCount()
    {
        if (f_time > 0)
        {
            f_time -= Time.deltaTime;
            return;
        }
        //b_isLose = true;
        b_time_finish = true;
        GL_8TYMngr.S.EndGame();
    }
    public IEnumerator TargetCreation()
    {
        ChooseAndCreateTarget();
        b_create_target = false;
        yield return new WaitForSeconds(targets[i_current_target].f_delay - 2f);
        b_create_target = true;
        inum_target_loop = TargetCreation();
    }
    /// <summary>
    /// Get spawn position
    /// </summary>
    /// <param name="i">Target index</param>
    /// <returns></returns>
    Vector3 ChoosePosition(int i)
    {
        float f_range_start;
        float f_range_end;
        switch (targets[i].i_level)
        {
            case 1:
                {
                    f_range_start = 25;
                    f_range_end = 30;
                }
                break;
            case 2:
                {
                    f_range_start = 30;
                    f_range_end = 40;
                }
                break;
            case 3:
                {
                    f_range_start = 40;
                    f_range_end = 50;
                }
                break;
            case 4:
                {
                    f_range_start = 50;
                    f_range_end = 65;
                }
                break;
            case 5:
                {
                    f_range_start = 65;
                    f_range_end = 80;
                }
                break;
            case 6:
                {
                    f_range_start = 80;
                    f_range_end = 100;
                }
                break;
            case 7:
                {
                    f_range_start = 100;
                    f_range_end = 120;
                }
                break;
            case 8:
                {
                    f_range_start = 120;
                    f_range_end = 140;
                }
                break;
            case 9:
                {
                    f_range_start = 140;
                    f_range_end = 160;
                }
                break;
            case 10:
                {
                    f_range_start = 160;
                    f_range_end = 180;
                }
                break;
            case 11:
                {
                    f_range_start = 180;
                    f_range_end = 185;
                }
                break;
            default:
                {
                    f_range_start = 25;
                    f_range_end = 30;
                }
                break;
        }
        float f_Y_angle = 0;
        bool b_minus = (Random.value > 0.5f);
        float f_minus = 1;
        if (b_minus)
            f_minus *= -1;
        f_Y_angle += ((Random.Range(5, 21) * f_minus) + f_lastYangle);
        if (f_Y_angle > 38)
            f_Y_angle -= 38;
        if (f_Y_angle < -38)
            f_Y_angle += 38;
        float f_range = Random.Range(f_range_start, f_range_end);
        float f_X_angle = (Random.Range(-35, 0) * -1);
        float f_x = f_range * Mathf.Cos(f_X_angle * Mathf.Deg2Rad) * Mathf.Sin(f_Y_angle * Mathf.Deg2Rad);
        float f_y = f_range * Mathf.Sin(f_X_angle * Mathf.Deg2Rad);
        float f_z = f_range * Mathf.Cos(f_X_angle * Mathf.Deg2Rad) * Mathf.Cos(f_Y_angle * Mathf.Deg2Rad);
        f_lastYangle = f_Y_angle;
        Vector3 v3_spawn_pos = new Vector3(f_x, f_y, f_z) + GL_CannonMngr.S.transform.position;
        return v3_spawn_pos;
    }
    /// <summary>
    /// Choose and create target
    /// </summary>
    void ChooseAndCreateTarget()
    {
        int k = Random.Range(0, GL_8TYMngr.S.GetCurrentLvl());
        if (k > 3)
            k = Random.Range(0, 3);
        i_current_target = k;
        //Instantiate(targets[i_current_target].go_target, ChoosePosition(i_current_target), transform.rotation);
        Instantiate(targets[i_current_target].go_target, spaces[targets[k].i_level - 1].GetFreePosition(), transform.rotation);
    }
    public void MainLoop()
    {
        if (b_create_target == true && b_time_finish == false)
        {
            StartCoroutine(inum_target_loop);
        }
        TimeCount();
    }
    #endregion

    #region Accessor methods
    public int GetCurrentTarget()
    {
        return i_current_target;
    }
    public float GetCurrentDelay()
    {
        return targets[i_current_target].f_delay;
    }
    public float GetTime()
    {
        return f_time;
    }
    public float GetMultiplicator()
    {
        return f_multiplicator;
    }
    public float GetCurrentScore()
    {
        return targets[i_current_target].f_add_score;
    }
    #endregion

    #region Setter methods
    public void AddTime(float time)
    {
        f_time += time;
    }    
    public void MultiChange(float mult)
    {
        f_multiplicator = mult;
    }
    #endregion

    #region Assist methods
    /// <summary>
    /// Add points to the current target
    /// </summary>
    /// <param name="k">Target index</param>
    /// <param name="coeficient">Multiply value</param>
    public void AddPoints(int k, float coeficient, Transform transform_l)
    {
        targets[k].i_destroyed++;
        targets[k].i_destroyed_total++;
        int i_score_add = Mathf.RoundToInt(targets[k].f_add_score * coeficient * f_multiplicator);
        GL_8TYMngr.S.AddScore(i_score_add);
        targets[k].i_score += i_score_add;
        f_time += targets[k].f_addTime;
        inter_cannon.AddBullets(0, targets[k].i_add_bullets);        
        StartCoroutine(GL_UI.S.ShowTextByTransform(transform_l,
            i_score_add.ToString() + " POINTS " + targets[k].f_addTime.ToString("F0") + " TIME", 2f));
        CheckTargetLvl(k);
    }
    /// <summary>
    /// Check and set level of the current target
    /// </summary>
    /// <param name="i">target index</param>
    public void CheckTargetLvl(int i)
    {
        if (targets[i].i_destroyed > 0 && targets[i].i_destroyed < 7)
        {
            targets[i].i_level = 1;
            return;
        }
        if (targets[i].i_destroyed >= 7 && targets[i].i_destroyed < 16)
        {
            targets[i].i_level = 2;
            return;
        }
        if (targets[i].i_destroyed >= 16 && targets[i].i_destroyed < 27)
        {
            targets[i].i_level = 3;
            return;
        }
        if (targets[i].i_destroyed >= 27 && targets[i].i_destroyed < 40)
        {
            targets[i].i_level = 4;
            return;
        }
        if (targets[i].i_destroyed >= 40 && targets[i].i_destroyed < 55)
        {
            targets[i].i_level = 5;
            return;
        }
        if (targets[i].i_destroyed >= 55 && targets[i].i_destroyed < 75)
        {
            targets[i].i_level = 6;
            return;
        }
        if (targets[i].i_destroyed >= 75 && targets[i].i_destroyed < 100)
        {
            targets[i].i_level = 7;
            return;
        }
        if (targets[i].i_destroyed >= 100 && targets[i].i_destroyed < 140)
        {
            targets[i].i_level = 8;
            return;
        }
        if (targets[i].i_destroyed >= 140 && targets[i].i_destroyed < 200)
        {
            targets[i].i_level = 9;
            return;
        }
        if (targets[i].i_destroyed >= 200 && targets[i].i_destroyed < 300)
        {
            targets[i].i_level = 10;
            return;
        }
        if (targets[i].i_destroyed >= 300)
        {
            targets[i].i_level = 11;
            return;
        }
    }
    public void CheckTargets()
    {
        GameObject go_target = GameObject.FindGameObjectWithTag("TargetInf");
        if (go_target != null)
        {
            return;
        }
        else
        {
            StopCoroutine(inum_target_loop);
            b_create_target = true;
        }
    }
    public void DestroyAll()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetInf");
        foreach (GameObject go in targets)
        {
            Destroy(go);
        }
    }
    public void StartLogic()
    {

    }
    public void EndLogic()
    {
        DestroyAll();
    }
    #endregion
}
