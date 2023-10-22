using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGizmo : MonoBehaviour
{
    public float size = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, size);
    }
}