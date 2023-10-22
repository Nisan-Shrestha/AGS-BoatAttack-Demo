using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shielder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<FunctionController>().ShieldUp();
            Debug.Log("public method called");
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
