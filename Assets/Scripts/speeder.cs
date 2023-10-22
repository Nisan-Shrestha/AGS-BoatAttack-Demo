using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speeder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<MovementController>().SpeedUp();
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
