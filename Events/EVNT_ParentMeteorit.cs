using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVNT_ParentMeteorit : MonoBehaviour {

	
    private void OnDestroy()
    {
        EVNT_8TYMngr.S.RemoveTarget();
    }
}
