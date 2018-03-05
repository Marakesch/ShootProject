using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_rubber_ball : MonoBehaviour {
    Rigidbody rb_var;
    public float f_force = 800;
    public float f_seconds_destroy = 5f;
    public GameObject go_collision;
    // Use this for initialization
    void Start()
    {
        rb_var = gameObject.GetComponent<Rigidbody>();
        rb_var.AddRelativeForce(0, 0, f_force);
        rb_var.AddRelativeTorque(600, 0, 0);
        StartCoroutine(SelfDestroy(f_seconds_destroy));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SelfDestroy(float f_seconds)
    {
        yield return new WaitForSeconds(f_seconds);
        GameObject go_explode = (GameObject)Instantiate(go_collision, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {            
            Destroy(gameObject);
        }        
        else
            return;
    }
}
