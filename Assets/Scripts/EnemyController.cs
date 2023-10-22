using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    //stats
    public float speed = 40.0f;
    public float RotationSpeed = 1;
    public float health = 20.0f;
    public int level = 0;
   
    //controls
    bool right = false;
    bool dead = false;

    //references
    private GameObject[] GunLevelParents = new GameObject[3];
    public GameObject[] Gun1Pos = new GameObject[2];
    public GameObject[] Gun2Pos = new GameObject[4];
    public GameObject[] Gun3Pos = new GameObject[6];
    private GameObject[][] GunPos = new GameObject[3][]
    {
        new GameObject[2], new GameObject[4], new GameObject[6]
    };
    public GameObject[] ProjectilePrefab = new GameObject[3];
    private float timeBetweenFire = 5f;
    private float shootTimer = 0f;

    public LevelController ES;

    private GameObject Player;
    private Rigidbody rb;
    private GameObject body;
    [SerializeField]
    private Image HealthBar;    
    [SerializeField]
    private GameObject[] powerupPrefab;

    private void Awake()
    {
        GunLevelParents[0] = transform.Find("Body/lvl1/u1").gameObject;
        GunLevelParents[1] = transform.Find("Body/lvl1/u2").gameObject;
        GunLevelParents[2] = transform.Find("Body/lvl1/u3").gameObject;
        GunPos[0] = Gun1Pos;
        GunPos[1] = Gun2Pos;
        GunPos[2] = Gun3Pos;
        
    }
    void Start()
    {
        ES = FindObjectOfType<LevelController>();
        right = Random.Range(0, 1.0f)>.5f;
        Player = GameObject.Find("Player");
        Transform childTrans = this.transform.Find("Body");
        if (childTrans != null)
            body = childTrans.gameObject;
        else
            Debug.LogError("Body of enemy ship not found!");
        rb = GetComponent<Rigidbody>();
    }

    public void init(int lvl, Canvas c)
    {
        level = lvl;
        for (int i = 1; i < 3; i++)
            GunLevelParents[i].SetActive(false);
        GunLevelParents[lvl].SetActive(true);
        SetupHealthBar(c);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;
        
        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;
        else
            shoot();
        if (health <= 0)
        {
            GameObject.Destroy(this.gameObject,1.0f);
            dead = true;
        }

    }
    private void shoot()
    {

        if (shootTimer <= 0)
        {
            shootTimer = timeBetweenFire;
            EjectProjectile();
            right = Random.Range(0, 1.0f) > .5f;
        }
    }

    private void EjectProjectile()
    {
        GameObject ammo = ProjectilePrefab[level];
        switch (level)
        {
            case 0:
                for (int i = 0; i < 2; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[0][i].transform.position, GunPos[0][i].transform.rotation);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[0][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[1][i].transform.position, GunPos[1][i].transform.rotation);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[1][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            case 2:
                for (int i = 0; i < 6; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[2][i].transform.position, GunPos[2][i].transform.rotation);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[2][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            default:
                break;
        }
        return;
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            var pos = transform.position;
            pos.y -= .2f;
            var rot = transform.eulerAngles;
            if (rot.x<85.0f)
                rot.x += 2.00f;
            transform.position= pos;
            transform.eulerAngles= rot;
            return;
        }
        handleMotion();
    }

    void handleMotion()
    {
        float r = Vector3.Distance(Player.transform.position, this.gameObject.transform.position);
        Vector3 rot = this.transform.rotation.eulerAngles;

        //dirn rotation
        Vector3 targetDirection = Player.transform.position - this.transform.position;
        float singleStep = RotationSpeed * Time.deltaTime;
        if (r <= 120.0f)
        {
            if (right)
                targetDirection = transform.right;
            else
                targetDirection = -transform.right;
        }

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        rot.y = Quaternion.LookRotation(newDirection).eulerAngles.y;
        this.transform.eulerAngles = rot;

        //move ahead
        rot = body.transform.rotation.eulerAngles;
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
        
        targetDirection =  Vector3.Normalize(targetDirection);
        float targRot = 180f + Mathf.Rad2Deg * (MathF.Atan2(-targetDirection.x, -targetDirection.z));
        float diff = dirRot - targRot;
        if (Mathf.Abs(diff) < 0.5f && (Mathf.Abs(rot.z) >= 0 || Mathf.Abs(rot.z) <= 360))
        {
            if (rot.z > 1 && rot.z < 25.0f)
                rot.z -= 1;
            else if (rot.z < -1 || (rot.z < 359.0f && rot.z > 335.0f))
                rot.z += 1;
            else
                rot.z = 0;
        }
        else if ((diff < -180f || (diff < 180 && diff > 0)) && (rot.z < 20.0f || rot.z > 340.0f))
            rot.z += 1;
        else if ((diff > 180f || (diff > -180 && diff < 0)) && ((rot.z > -22.0f && rot.z < 22.0f) || (rot.z > (360 - 20.0f) && rot.z > 338.0f)))
            rot.z -= 1;
        body.transform.eulerAngles = rot;
    }

    public void Hurt(float damage) {
        health -= damage;
        HealthBar.fillAmount= health/20.0f;
        if (health <=0 && !dead)
        {
            dead= true;
            ES.enemyCount -= 1;
            ES.killCount += 1;
            var pu = GameObject.Instantiate(powerupPrefab[Random.Range(0, powerupPrefab.Length)], this.transform.position  ,Quaternion.identity);
            var t = pu.transform.position;
            t.y = 38;
            pu.transform.position = t;
            Destroy(HealthBar.gameObject, .9f);
            GameObject.Destroy(this.gameObject,1.0f);
        }
        return;
    }

    public void SetupHealthBar(Canvas Canvas)
    {
       
        HealthBar.transform.SetParent(Canvas.transform);
        /*if (HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.Camera = Camera.current;
        }*/
    }
}
