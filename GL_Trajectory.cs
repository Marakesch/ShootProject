using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBonus
{
    void SwitchOn();
    void SwitchOff();
}

public class GL_Trajectory : MonoBehaviour, IBonus {

    #region Variables    
    LineRenderer linerenderer;
    float f_mag = 75.5f;
    bool b_enable;
    #endregion

    #region Routine Methods
    private void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        b_enable = false;
    }
    private void Update()
    {
        if(b_enable)
        {
            MakeTrajectory(GL_CannonMngr.S.t_bullet_pos.position, 0.5f, f_mag);
        }
    }
    #endregion

    #region Main Logic
    void MakeTrajectory(Vector3 v3_start, float timeStep, float f_magnitude)
    {

        float f_alpha_angle = -GL_CannonMngr.S.t_dulo.localRotation.eulerAngles.x;
        float f_bravo_angle = GL_CannonMngr.S.t_platform.localRotation.eulerAngles.y;
        if (f_bravo_angle < 0)
            f_bravo_angle *= -1;
        float f_y = Mathf.Sin(Mathf.Deg2Rad * f_alpha_angle) * f_magnitude;
        float f_x = f_magnitude * Mathf.Cos(Mathf.Deg2Rad * f_alpha_angle) * Mathf.Sin(Mathf.Deg2Rad * (f_bravo_angle));
        float f_z = f_magnitude * Mathf.Cos(Mathf.Deg2Rad * f_alpha_angle) * Mathf.Cos(Mathf.Deg2Rad * (f_bravo_angle));
        Vector3 v3_velocity = new Vector3(f_x, f_y, f_z);
        List<Vector3> v3_paths = new List<Vector3>();
        for (float time = 0; time < 5f; time += timeStep)
        {
            Vector3 current_point = v3_start + (v3_velocity * time) + (Physics.gravity * time * time / 2);
            v3_paths.Add(current_point);
        }
        Vector3[] v3_traj = v3_paths.ToArray();
        linerenderer.positionCount = v3_traj.Length;
        linerenderer.SetPositions(v3_traj);
        linerenderer.numCapVertices = 1;
    }
    #endregion

    #region Interface Implementation
    public void SwitchOn()
    {
        linerenderer.enabled = true;
        b_enable = true;
    }
    public void SwitchOff()
    {
        linerenderer.enabled = false;
        b_enable = false;
    }
    #endregion
}
