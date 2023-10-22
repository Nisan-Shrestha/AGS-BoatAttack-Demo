using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    public float AmmoLife;
    public float damage;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("selfDestruct");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     PlayerAmmo = 9
     Enemy = 6
     EnemyAmmo = 10
     Boat = 8   
     */

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.layer == 9 && other.gameObject.layer == 6)  
        {
            other.GetComponent<EnemyController>().Hurt(damage);
            GameObject.Destroy(this.gameObject);
        }
        else if (this.gameObject.layer == 10 && other.gameObject.layer == 8)
        {
            
            other.GetComponent<FunctionController>().Hurt(damage);
            GameObject.Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == 7)
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    IEnumerator  selfDestruct()
    {
        yield return new WaitForSeconds(AmmoLife);
        if (this.gameObject!=null)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
