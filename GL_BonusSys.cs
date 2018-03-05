using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType { Rockets, Time, Bullet, Trajectory, Boom}

public class GL_BonusSys : MonoBehaviour {

    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Boom();
        }
    }


    #region MainMethods
    public void AddTime(float time)
    {
        GL_8TYMngr.S.AddTime(time);
    }
    public void AddBullet(int qty)
    {
        GL_CannonMngr.S.AddBullets(0, qty);
    }
    public IEnumerator SetCoefficient(float f_coef, float time)
    {
        GL_8TYMngr.S.SetMultiplicator(f_coef);
        yield return new WaitForSeconds(time);
        GL_8TYMngr.S.SetMultiplicator(1f);
    }
    public IEnumerator SwitchOnTrajectory(float time)
    {
        GL_CannonMngr.S.SwitchOnTrajectory();
        yield return new WaitForSeconds(time);
        GL_CannonMngr.S.SwitchOffTrajectory();
    }
    public void Boom()
    {
        TGT_8TY[] targets = FindObjectsOfType<TGT_8TY>();        
        foreach(TGT_8TY tgt in targets)
        {            
            tgt.BoomDestroy();
        }        
    }
    #endregion

    #region Assisst methods
    public void PerformCoroutine(IEnumerator cor)
    {
        StartCoroutine(cor);
    }   
    public void AssistOnTrajectory()
    {
        StartCoroutine(SwitchOnTrajectory(10));
    }
    #endregion

    #region Private Class
    public class BonusUnit
    {
        delegate void TopUp(float qty);
        public BonusType bonus_type;
        public  int i_qty = 0;

        public void AddQty(int i)
        {
            i_qty += i;
        }
        public void RemoveQty()
        {
            i_qty--;
            if (i_qty < 0)
                i_qty = 0;
        }
    }
    #endregion
}


