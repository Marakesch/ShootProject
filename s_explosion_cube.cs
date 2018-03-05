using System.Collections;
using UnityEngine;

public class s_explosion_cube : MonoBehaviour {
    public float power = 5f;
    public float radius = 5f;
    public GameObject explosionVFX;
    Rigidbody rb_self;
    public float f_magnitude;

    // Use this for initialization
    void Start()
    {
        rb_self = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        f_magnitude = rb_self.velocity.magnitude;
    }
    void addExplosionNearby()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            if(hit.CompareTag("TargetInf"))
            {
                TGT_8TY tgt = hit.GetComponent<TGT_8TY>();
                tgt.BoomDestroy();                       
            }            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector3 normal = collision.contacts[0].normal;
            transform.rotation.SetLookRotation(normal);
            transform.rotation = Quaternion.Inverse(transform.rotation);
            Instantiate(explosionVFX, transform.position, transform.rotation);
            addExplosionNearby();
            //Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }
        else
        {
            float f_magnitude = rb_self.velocity.magnitude;
            if (f_magnitude > 0.5f)
            {
                Instantiate(explosionVFX, transform.position, transform.rotation);
                addExplosionNearby();
                Destroy(gameObject);
                return;
            }
            return;
        }
    }
}
