using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GL_8TYData : MonoBehaviour {
    public int i_score
    {
        get { return _i_score; }
        set
        {
            _i_score = value;
            GL_8TYMngr.S.CheckLevel();
        }
    }
    public int i_current_lvl
    {
        get { return _i_current_lvl; }
        set
        {
            _i_current_lvl = Mathf.Clamp(value, 0, 10);
        }
    }

    private int _i_score;
    private int _i_current_lvl;

    public GL_8TYData(int score = 0, int lvl = 0)
    {
        i_score = score;
        i_current_lvl = lvl;
    }
}
