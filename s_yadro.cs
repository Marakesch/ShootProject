using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_yadro : MonoBehaviour
{
    Rigidbody rb_var;
    public float f_force = 800;
    public float f_torque = 100;
    public float f_seconds_destroy = 5f;
    public GameObject go_collision;
    public GameObject go_VFX_target;
    public GameObject go_VFX_self;
    s_infinity_mode infinity_mode;
    bool done;
    // Use this for initialization
    void Start()
    {
        rb_var = gameObject.GetComponent<Rigidbody>();
        infinity_mode = FindObjectOfType<s_infinity_mode>();
        rb_var.AddRelativeForce(0, 0, f_force, ForceMode.Impulse);
        rb_var.AddRelativeTorque(0, 0, f_torque);
        //print(rb_var.velocity);
        Destroy(gameObject, f_seconds_destroy);
        done = false;
    }    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if(!done)
            {
                Instantiate(go_collision, transform.position, transform.rotation);
                Destroy(collision.gameObject);
                Destroy(gameObject);
                done = true;
            }
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
            return;
        if (collision.gameObject.CompareTag("Target"))
        {
            if (!done)
            {
                Instantiate(go_VFX_target, collision.transform.position, collision.transform.rotation);
                Destroy(collision.gameObject);
                Destroy(gameObject);
                done = true;
            }
            return;
        }
        if (collision.gameObject.CompareTag("TargetInf"))
        {
            if (!done)
            {
                Instantiate(go_VFX_target, collision.transform.position, collision.transform.rotation);
                Destroy(collision.gameObject);
                Destroy(gameObject);
                done = true;
            }
            return;
        }
        if (collision.gameObject.CompareTag("Deadzone"))
        {
            return;
        }
        else
        {
            Instantiate(go_VFX_self, transform.position, transform.rotation);
            Destroy(gameObject);            
            return;
        }
    }

}