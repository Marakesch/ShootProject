using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVNT_8TYMeteorit : MonoBehaviour {
    int i_qty_meteorits = 35;
    Bounds bounds_meteorit;
    public List<GameObject> list_go_meteorits = new List<GameObject>();
    // Use this for initialization
    void Start () {
        BoxCollider box = GetComponent<BoxCollider>();
        bounds_meteorit = box.bounds;
        StartLogic();
	}
    void CreateTargetBounds(Bounds bound_bound, GameObject go_target)
    {
        Vector3 v3_center = bound_bound.center;
        Vector3 v3_extends = bound_bound.extents;
        float f_rand_x = Random.Range(v3_center.x - v3_extends.x, v3_center.x + v3_extends.x);
        float f_rand_y = Random.Range(v3_center.y - v3_extends.y, v3_center.y + v3_extends.y);
        float f_rand_z = Random.Range(v3_center.z - v3_extends.z, v3_center.z + v3_extends.z);
        GameObject go_meteo = Instantiate(go_target, new Vector3(f_rand_x, f_rand_y, f_rand_z), transform.rotation);
        Rigidbody rb = go_meteo.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Random.Range(-550, -300), -500, Random.Range(-400,-150), ForceMode.Impulse);
        rb.AddRelativeTorque(Random.Range(1, 2), Random.Range(0, 2), Random.Range(0, 2), ForceMode.Impulse);
        
    }
    IEnumerator EventMeteoritCreate()
    {
        for (int i = 0; i < i_qty_meteorits; i++)
        {
            CreateTargetBounds(bounds_meteorit, list_go_meteorits[Random.Range(0, 1)]);
            yield return new WaitForSeconds(Random.Range(2, 4f));
        }
    }
    public void MainLoop()
    {

    }
    public float GetTime()
    {
        return 5f;
    }
    public void DestroyAll()
    {

    }
    public void StartLogic()
    {
        StartCoroutine(EventMeteoritCreate());
    }
    public void EndLogic()
    {

    }
    public void AddTime(float time)
    {
        
    }
    void CheckMeteorits()
    {
        
    }
}
