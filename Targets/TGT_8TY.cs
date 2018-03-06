using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TgtType { NORMAL, EVENT }
[System.Serializable]
public class TGT_8TY : MonoBehaviour
{
    protected TgtType type;
    protected float f_delay;
    protected float f_score;
    int i_num_target;
    protected float f_dist;
    public GameObject go_vfx;


    // Use this for initialization
    void Start()
    {
        StartLogic();
        StartEndAnimation();
    }
    void OnMouseDown()
    {        
        StartCoroutine(GL_UI.S.ShowTextByTransform(transform, f_dist.ToString("F0"), 1.2f));
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            float f_coef = (f_dist / 100) + 1;
            TGT_8TYMngr.S.AddPoints(i_num_target, f_coef, transform);
            return;
        }
        return;
    }
    private void OnDestroy()
    {
        if (TGT_8TYMngr.S != null)
            TGT_8TYMngr.S.CheckTargets();
    }
    public void StartLogic()
    {
        type = TgtType.NORMAL;
        f_delay = TGT_8TYMngr.S.GetCurrentDelay();
        i_num_target = TGT_8TYMngr.S.GetCurrentTarget();
        f_score = TGT_8TYMngr.S.GetCurrentScore();
        f_dist = Vector3.Distance(transform.position, GL_CannonMngr.S.transform.position);
        Destroy(gameObject, f_delay);
    }
    public virtual void StartEndAnimation()
    {
        if (transform.parent != null)
        {
            transform.parent.DOScale(1f, 1f).SetLoops(1, LoopType.Yoyo).Play();
            transform.parent.DOScale(0, 1f).SetLoops(1, LoopType.Yoyo).SetDelay(f_delay - 1).Play();
            return;
        }
        else
        {
            transform.DOScale(2f, 1f).SetLoops(1, LoopType.Yoyo).Play();
            transform.DOScale(0, 1f).SetLoops(1, LoopType.Yoyo).SetDelay(f_delay - 1).Play();
        }
    }
    bool done = false;
    public void BoomDestroy()
    {
        Instantiate(go_vfx, transform.position, transform.rotation);
        switch (type)
        {
            case (TgtType.NORMAL):
                {
                    if (!done)
                        TGT_8TYMngr.S.AddPoints(i_num_target, 1, transform);
                    done = true;
                }
                break;
            case (TgtType.EVENT):
                {
                    if (!done)
                        EVNT_8TYMngr.S.AddScore(f_score);
                    done = true;
                }
                break;
        }
        Destroy(gameObject);
    }
}
