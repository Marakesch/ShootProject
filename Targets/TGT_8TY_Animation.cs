using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGT_8TY_Animation : TGT_8TY
{
    public GameObject go_parent;
    public string[] triggers;
    string s_trigger;

    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        int i_ran_trig = Random.Range(0, triggers.Length);
        s_trigger = triggers[i_ran_trig];
        animator.SetTrigger(s_trigger);
        StartLogic();
        StartEndAnimation();
    }

    private void OnDestroy()
    {
        TGT_8TYMngr.S.CheckTargets();
        Destroy(go_parent);
    }
}
