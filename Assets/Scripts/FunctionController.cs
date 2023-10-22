using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FunctionController : MonoBehaviour
{
    [SerializeField]
    public GameObject ammoParent;
    [SerializeField]
    public float health = 100.0f;
    [SerializeField]
    private Image healthFillImage;
    public GameObject[] ProjectilePrefab = new GameObject[3];
    private float shieldtimer = 0;
    private bool immune = false;

    [SerializeField]
    public GameObject[] BoatLevelParents= new GameObject[3];
    private GameObject[] GunLevelParents= new GameObject[3];
    public GameObject[] Gun1Pos = new GameObject[2];
    public GameObject[] Gun2Pos = new GameObject[4];
    public GameObject[] Gun3Pos = new GameObject[6];

    private GameObject[][] GunPos = new GameObject[3][]
    {
        new GameObject[2], new GameObject[4], new GameObject[6]
    };
    [SerializeField]
    private Animator[] Animators;

    public int BoatLevel = 0;
    public int GunLevel = 0;
    public bool shooting = false;

    [SerializeField]
    private float timeBetweenFire = 5f;
    private float shootTimer = 0f;
    public bool dead = false;

    public Image gameOverImage;
    // Start is called before the first frame update
    void Start()
    {
        GunLevelParents[0] = BoatLevelParents[BoatLevel].transform.Find("u1").gameObject;
        GunLevelParents[1] = BoatLevelParents[BoatLevel].transform.Find("u2").gameObject;
        GunLevelParents[2] = BoatLevelParents[BoatLevel].transform.Find("u3").gameObject;
        GunPos[0] = Gun1Pos;
        GunPos[1] = Gun2Pos;
        GunPos[2] = Gun3Pos;
        for (int i = 1; i < 3; i++)
        {
            BoatLevelParents[i].SetActive(false);
            GunLevelParents[i].SetActive(false);
        }
        BoatLevelParents[0].SetActive(true);
        GunLevelParents[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (health<=0)
        {
            if (!dead)
            {
                StartCoroutine("GameOver");
                dead = true;
            }
            return;
        }
        if (shieldtimer > 0)
            shieldtimer -= Time.deltaTime;
        if (shootTimer > 0) 
            shootTimer -= Time.deltaTime;
        if (shootTimer < (timeBetweenFire - 1))
        {
            foreach (var a in Animators)
                if (a.isActiveAndEnabled)
                    a.SetBool("shoot", false);
        }
        healthFillImage.fillAmount= health/100.0f;
    }
    public void shoot()
    {
        if (shootTimer<=0)
        {
            shootTimer = timeBetweenFire;
            EjectProjectile();
            if (BoatLevel ==1)
            {
                foreach (var a in Animators)
                    a.SetBool("shoot", true);
            }
        }
    }
    private void EjectProjectile()
    {
        GameObject ammo = ProjectilePrefab[BoatLevel];
        switch (GunLevel)
        {
            case 0:
                for (int i = 0; i < 2; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[0][i].transform.position, GunPos[0][i].transform.rotation,ammoParent.transform);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[0][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[1][i].transform.position, GunPos[1][i].transform.rotation, ammoParent.transform);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[1][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            case 2:
                for (int i = 0; i < 6; i++)
                {
                    GameObject p1 = Instantiate(ammo, GunPos[2][i].transform.position, GunPos[2][i].transform.rotation, ammoParent.transform);
                    p1.GetComponent<Rigidbody>().velocity = GunPos[2][i].transform.forward * p1.GetComponent<Ammo>().speed;
                }
                break;
            default:
                break;
        }
        return;
    }

    public void Upgrade()
    {
        BoatLevelParents[BoatLevel].SetActive(false);
        GunLevelParents[GunLevel].SetActive(false);      
        if (GunLevel < 2)
            GunLevel += 1;
        else if (BoatLevel < 2)
        {
            BoatLevel += 1;
            GunLevel = 0;
        }
        GunLevelParents[0] = BoatLevelParents[BoatLevel].transform.Find("u1").gameObject;
        GunLevelParents[1] = BoatLevelParents[BoatLevel].transform.Find("u2").gameObject;
        GunLevelParents[2] = BoatLevelParents[BoatLevel].transform.Find("u3").gameObject;
        BoatLevelParents[BoatLevel].SetActive(true);
        GunLevelParents[GunLevel].SetActive(true);
    }

    public void Hurt(float damage)
    {
        if (!immune)
            health -= damage;
        healthFillImage.fillAmount = health / 100.0f;
        return;
    }

    public void Heal()
    {
        health += 30f;
        health = Mathf.Clamp(health, 0f, 100f);
        return;
    }

    public void ShieldUp()
    {
        StartCoroutine("ShieldToggle");
        return;
    }


    IEnumerator ShieldToggle()
    {
        if (shieldtimer > 0)
            yield return new WaitForSeconds(shieldtimer);
        transform.Find("sheild").gameObject.SetActive(true);
        Debug.Log("should be active");
        immune = true;
        shieldtimer += 10f;
        yield return new WaitForSeconds(shieldtimer);
        transform.Find("sheild").gameObject.SetActive(false);
        immune = false;
    }

    IEnumerator  GameOver()
    {
        foreach (var c in this.GetComponents<Collider>())
        {
            c.enabled= false;
        } 
        gameOverImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        FindObjectOfType<LevelController>().GoHome();
    }
}