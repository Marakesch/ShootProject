using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TGT_8TY_Covered : TGT_8TY {
    public GameObject go_parent;
    public GameObject go_VFX_box;
    public GameObject[] l_boxes;
    private void OnDestroy()
    {

        /*for (int i = 0; i < l_boxes.Length; i++)
        {
            if (l_boxes[i] != null)
            {
                Instantiate(go_VFX_box, l_boxes[i].transform.position, l_boxes[i].transform.rotation);
                Destroy(l_boxes[i]);
            }
        }*/
        TGT_8TYMngr.S.CheckTargets();        
        Destroy(go_parent);
    }
    
}
