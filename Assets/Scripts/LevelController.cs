using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;
    public Transform[] spawnTrans;
    public int maxEnemyCount = 3;
    public int enemyCount;
    public int killCount;
    [SerializeField]
    private int GameLevel = 0;
    public Text killCountText;
    public Canvas healthCanvas;
    public GameController GC;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        GC = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        //difficulty setting
        if (killCount > 5)
            GameLevel = 2;
        else if (killCount > 3)
            GameLevel = 1;
        else
            GameLevel = 0;
        maxEnemyCount = GameLevel * 2 + 3;

        //spawn enemy
        while (enemyCount< maxEnemyCount)
        {
            Transform trans = spawnTrans[Random.Range(0,spawnTrans.Length)];
            trans.position += new Vector3(Random.Range(-40.0f,40.0f ) ,0.0f , Random.Range(-40.0f, 40.0f));
            var obj = GameObject.Instantiate(EnemyPrefab, trans.position, Quaternion.identity);
            obj.GetComponent<EnemyController>().init(Random.Range(0,GameLevel+1), healthCanvas);
            enemyCount++;
        }
        killCountText.text = "Kills: " + killCount;
    }

    public void GoHome()
    {
        GC.LoadMainMenu();
    }
}
