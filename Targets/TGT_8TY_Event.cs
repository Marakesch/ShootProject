using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class TGT_8TY_Event : TGT_8TY
{    
    void Start()
    {
        StartSequence();
    }
    bool dones = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {            
            if (!dones)
                EVNT_8TYMngr.S.AddScore(f_score);
            dones = true;
        }

    }
    void StartSequence()
    {
        type = TgtType.EVENT;
        transform.eulerAngles = new Vector3(90, 0, Random.Range(-30, 30));
        transform.DORotate(new Vector3(0, 180, 0), 2f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true).Play();
        f_score = EVNT_8TYMngr.S.GetDefaultScore();
        f_dist = Vector3.Distance(transform.position, GL_CannonMngr.S.transform.position);
    }
    private void OnDestroy()
    {
        EVNT_8TYMngr.S.RemoveTarget();
    }
}
