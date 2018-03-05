using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP_MeshGraphCreate : MonoBehaviour
{
    public Mesh meshObj;
    public GameObject mesh;
    public List<Vector3> vertxs = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    // Use this for initialization
    void Start()
    {
        //meshObj = mesh.GetComponent<Mesh>();
        AssignVertexes();
        AssignNormals();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void AssignVertexes()
    {
        for (int i = 0; i < meshObj.vertexCount; i++)
        {
            vertxs.Add(meshObj.vertices[i]);
        }

    }
    void AssignNormals()
    {
        for (int i = 0; i < meshObj.vertexCount; i++)
        {
            normals.Add(meshObj.normals[i]);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(Vector3 psn in vertxs)
        {
            Vector3 offset = mesh.transform.position;
            Gizmos.DrawSphere((psn*50) + offset, 1);
        }
        foreach(Vector3 nrml in normals)
        {
            Vector3 offset = mesh.transform.position;
            Gizmos.DrawWireSphere((nrml * 30) + offset, 1);
        }

        /*for (int k = 0; k < vertxs.Count; k++)
        {
            Gizmos.DrawSphere(vertxs[k] * 30, 1);
            if (k != 0)
                Gizmos.DrawLine(vertxs[k] * 30, vertxs[k - 1] * 30);
        }*/
    }
}
