using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISegment
{
    bool IsOccupied();
}

public class GL_NetSegment : MonoBehaviour, ISegment
{    
    bool b_occupied;
    GameObject occupant;
    private void FixedUpdate()
    {
        if (occupant == null)
            b_occupied = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        occupant = other.gameObject;
        b_occupied = true;
    }
    private void OnTriggerExit(Collider other)
    {
        occupant = null;
    }
    private void OnTriggerStay(Collider other)
    {
        occupant = other.gameObject;
        b_occupied = true;
    }
    public bool IsOccupied()
    {
        return b_occupied;
    }
    private void OnDrawGizmos()
    {
        if (b_occupied)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position, gameObject.GetComponent<BoxCollider>().size);
        }
        else
        {
            Gizmos.color = new Color(0, 1, 0, 0.5F);
            Gizmos.DrawWireCube(transform.position, gameObject.GetComponent<BoxCollider>().size);
        }
        
        
    }
}
