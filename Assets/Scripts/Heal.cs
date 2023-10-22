using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<FunctionController>().Heal();
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
