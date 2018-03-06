using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TGT_Data : MonoBehaviour {
    public GameObject go_target;
    public int i_destroyed;
    public int i_destroyed_total;
    public int i_level;
    public float f_add_score;
    public int i_score;
    public int i_ttl_score;
    public int i_add_bullets;
    public float f_delay;
    public float f_addTime;


    public TGT_Data(int dest, int lvl, float pts, int bullets, float delay, float time)
    {
        i_destroyed = dest;
        i_level = lvl;
        f_add_score = pts;
        i_add_bullets = bullets;
        f_delay = delay;
        f_addTime = time;
    }
    public void SetDestroyed(int a)
    {
        i_destroyed = a;
    }
    public void SetDestroyedTotal(int a)
    {
        i_destroyed_total = a;
    }
    public void SetCurrentLvl(int a)
    {
        i_level = a;
    }
    public void Destroyed()
    {
        i_destroyed++;
    }
}
