using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class GL_3DNet : MonoBehaviour {

    
    #region Variables
    public Transform t_start;    
    public GameObject go_segment;    
    public float xx, yy, zz, size;       
    Vector3 position;
    public List<GameObject> Net = new List<GameObject>();
    int i = 0;    
    #endregion

    #region Routine Methods
    private void Start()
    {
        
    }
    #endregion

    #region Main Methods
    public void CreateNet()
    {
        Initialize();
        for (int x = 0; x < xx; x++)
        {
            for(int y = 0; y < yy; y++)
            {
                for (int z = 0; z < zz; z++)
                {
                    Vector3 psn = new Vector3(x*size, y*size, z*size);
                    Vector3 offset = new Vector3(size / 2, size / 2, size / 2);
                    psn += offset;                 
                    Net.Add(Instantiate(go_segment, (transform.localPosition + psn), go_segment.transform.rotation));
                    Net[i].transform.parent = gameObject.transform;
                    Net[i].name = Net[i].name.Replace("(Clone)", psn.ToString());                    
                    i++;
                }
            }
        }
    }
    public Vector3 GetFreePosition()
    {
        Vector3 psn;
        List<Vector3> freePosition = new List<Vector3>();
        foreach(GameObject go in Net)
        {            
            GL_NetSegment segm = go.gameObject.GetComponent<GL_NetSegment>();
            if(!segm.IsOccupied())
            {
                freePosition.Add(go.transform.position);
            }
        }
        if (freePosition.Count > 0)
        {
            psn = freePosition[Random.Range(0, freePosition.Count)];
            psn += new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4));
            return psn;
        }
        foreach(GameObject go in Net)
        {
            freePosition.Add(go.transform.position);           
        }
        psn = freePosition[Random.Range(0, freePosition.Count)];
        psn += new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4));
        return psn;
    }
    #endregion

    #region Assist methods
    void Initialize()
    {
        go_segment = Resources.Load("Prefabs/Net/Segment", typeof(GameObject)) as GameObject;
        go_segment.GetComponent<BoxCollider>().size = Vector3.one;        
        go_segment.GetComponent<BoxCollider>().size *= size;        
    }    
    public void SetPosition()
    {
        position = GetFreePosition();
    }
    #endregion

    #region Accessor methods
    public float GetSize()
    {
        return size;
    }
    public void ClearList()
    {        
        for ( i = Net.Count; i > 0; i--)
        {
            GameObject go = Net[i-1];
            Net.RemoveAt(i-1);
            //Destroy(go);
            DestroyImmediate(go);
        }        
        i = 0;        
    }
    #endregion

   
}
