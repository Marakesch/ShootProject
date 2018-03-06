using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GL_Environment : MonoBehaviour {

    #region Variables    
    Vector3 sourcePsn;
    Vector3 destPsn;
    Vector3 cannonPsn;
    public EnviromentalData Decors = new EnviromentalData();
    public EnviromentalData BoomDrums = new EnviromentalData();
    public EnviromentalData Bonuses = new EnviromentalData();
    [HideInInspector]
    public static GL_Environment S;
    bool b_create = true;
    #endregion

    #region Routine methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);
    }
    void Start () {
        cannonPsn = GL_CannonMngr.S.transform.position;
        sourcePsn = Vector3.zero;
        destPsn = cannonPsn;
        Decors.type = EnvironmentType.DECORATION;
        BoomDrums.type = EnvironmentType.BOOMDRUM;
        Bonuses.type = EnvironmentType.BONUS;
	}
	
	
	void Update ()
    {
        MainLoop();
	}
    #endregion

    #region Assist methods
    void CreateTargetBounds(Bounds bound_bound, GameObject go_target)
    {
        Vector3 v3_center = bound_bound.center;
        Vector3 v3_extends = bound_bound.extents;
        float f_rand_x = Random.Range(v3_center.x - v3_extends.x, v3_center.x + v3_extends.x);
        float f_rand_y = Random.Range(v3_center.y - v3_extends.y, v3_center.y + v3_extends.y);
        float f_rand_z = Random.Range(v3_center.z - v3_extends.z, v3_center.z + v3_extends.z);
        GameObject go_decor = Instantiate(go_target, new Vector3(f_rand_x, f_rand_y, f_rand_z), transform.rotation);
        Rigidbody rb = go_decor.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Random.Range(-550, -300), -500, Random.Range(-400, -150), ForceMode.Impulse);
        rb.AddRelativeTorque(Random.Range(1, 2), Random.Range(0, 2), Random.Range(0, 2), ForceMode.Impulse);

    }
    void ChooseTypeInstanciate()
    {
        SetStartEndPoints();
        int j = Random.Range(0, 3);
        switch(j)
        {
            case (0):
                {
                    CreateEnvironment(Decors);
                }
                break;
            case (1):
                {
                    CreateEnvironment(BoomDrums);
                }
                break;
            case (2):
                {
                    CreateEnvironment(Bonuses);
                }
                break;
            default:
                {
                    CreateEnvironment(Bonuses);
                }
                break;
        }
    }
    void SetStartEndPoints()
    {
        destPsn = cannonPsn;
        Vector3 offset = new Vector3(0, 0, Random.Range(25,95));
        int j = Random.Range(0, 4);
        switch(j)
        {
            case (0):
                {
                    sourcePsn = Vector3.left*100;                               
                }
                break;
            case (1):
                {
                    sourcePsn = Vector3.right*100;
                }
                break;
            case (2):
                {
                    sourcePsn = Vector3.up*50;
                }
                break;
            case (3):
                {
                    sourcePsn = Vector3.down*10;
                }
                break;
            default:
                {
                    sourcePsn = Vector3.left;
                }
                break;
        }
        
        sourcePsn += offset;
        sourcePsn += cannonPsn;
        destPsn += new Vector3(Random.Range(-10, 10), Random.Range(0, 15), Random.Range(25, 95));
    }
    void CreateEnvironment(EnviromentalData env)
    {
        if(env.i_pool_limit < 21)
        {
            GameObject go_new = Instantiate(env.ObjectList[Random.Range(0, env.ObjectList.Count)], sourcePsn, transform.rotation, transform);
            ENV_Unit unit = go_new.GetComponent<ENV_Unit>();
            unit.SetType(env.type);
            Rigidbody rb = go_new.GetComponent<Rigidbody>();
            rb.AddForce((destPsn - sourcePsn)*30);
            rb.AddRelativeTorque(Random.Range(1, 2), Random.Range(0, 2), Random.Range(0, 2), ForceMode.Impulse);
            env.i_pool_limit++;
        }
    }
    IEnumerator Delays(float time)
    {
        b_create = false;
        yield return new WaitForSeconds(time);
        b_create = true;
    }
    void MainLoop()
    {
        if (b_create == true)
        {
            StartCoroutine(Delays(2));
            ChooseTypeInstanciate();
        }
    }
    #endregion

    #region private Classes
    [System.Serializable]
    public class EnviromentalData
    {
        public EnvironmentType type;
        public List<GameObject> ObjectList = new List<GameObject>();
        
        public int i_pool_limit = 0;
    }
    #endregion
}
