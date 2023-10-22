using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementController : MonoBehaviour
{
    public float speed;
    public float RotationSpeed;
    public VariableJoystick varJS;
    public Rigidbody rb;

    private GameObject body;
    private FunctionController fc;
    private float speedtimer = 0;
    private void Start()
    {
        fc = this.GetComponent<FunctionController>();
        Transform childTrans = this.transform.Find("Body");
        if (childTrans != null)
            body = childTrans.gameObject;
        else
            Debug.LogError("Body of ship not found!");
        
    }
    public void FixedUpdate()
    {
        if (!fc.dead)
            handleMotion();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == 7) 
            rb.velocity /= 8f;
    }

    void handleMotion()
    {

        Vector3 rot = this.transform.rotation.eulerAngles;
        //dirn rotation
        Vector3 targetDirection = Vector3.forward * varJS.Vertical + Vector3.right * varJS.Horizontal;
        float singleStep = RotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        rot.y = Quaternion.LookRotation(newDirection).eulerAngles.y; 
        this.transform.eulerAngles = rot;

        //move ahead
        rot = body.transform.rotation.eulerAngles;
        float r = Mathf.Sqrt(varJS.Horizontal * varJS.Horizontal + varJS.Vertical * varJS.Vertical);
        rb.velocity = transform.forward * speed;      

        //bob up down
        float val = Mathf.Sin(Time.timeSinceLevelLoad * 1.3f);
        rot.x = val * 7f;

        //tilt turn
        float dirRot = rot.y;
        while (dirRot < 0.0f)
            dirRot += 360f;
        if (dirRot > 360f)
            dirRot = dirRot % 360f;

        float targRot = 180f + Mathf.Rad2Deg * (MathF.Atan2(-varJS.Horizontal, -varJS.Vertical));
        if (r <= .5)
            targRot = dirRot;

        float diff = dirRot - targRot;
        if (Mathf.Abs(diff) < 0.5f && (Mathf.Abs(rot.z) >= 0 || Mathf.Abs(rot.z) <= 360))
        {
            //Debug.Log("going 0");
            if (rot.z > 1 && rot.z < 25.0f)
                rot.z -= 1;
            else if (rot.z < -1 || (rot.z < 359.0f && rot.z >335.0f))
                rot.z += 1;
            else
                rot.z = 0;
        }
        else if (    (diff < -180f || (diff < 180 && diff > 0))     &&      (rot.z<20.0f|| rot.z > 340.0f)       )
            rot.z += 1;
        else if (   (   diff > 180f || (diff > -180 && diff < 0)  ) &&  (    (rot.z > -22.0f && rot.z < 22.0f)  || (rot.z > (360 - 20.0f) && rot.z > 338.0f)    ) )
            rot.z -= 1;
 /*
        if ((rot.z > 30.0f && rot.z < 180.0f ) || (rot.z < 330 && rot.z > 180.0f))
        {
            //Debug.Log("OVERSHOT!!:   "  + diff);
        }
*/        body.transform.eulerAngles = rot;
        
    }

    private void Update()
    {
        if (fc.dead)
        {
            var pos = transform.position;
            pos.y -= .2f;
            var rot = transform.eulerAngles;
            if (rot.x < 85.0f)
                rot.x += 2.00f;
            transform.position = pos;
            transform.eulerAngles = rot;
            return;
        }
        if (speedtimer > 0)
            speedtimer -= Time.deltaTime;
    }

    public void SpeedUp()
    {
        StartCoroutine("Slowdown");
        return;
    }

    IEnumerator  Slowdown()
    {
        if (speedtimer>0)
            yield return new WaitForSeconds(speedtimer);
        speed = 110f;
        RotationSpeed = 3f;
        speedtimer += 10;
        yield return new WaitForSeconds(speedtimer);
        speed = 70f;
        RotationSpeed = 1;
    }
}