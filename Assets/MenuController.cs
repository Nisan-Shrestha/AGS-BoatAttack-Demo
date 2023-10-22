using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (gc == null)
        {
            gc = FindObjectOfType<GameController>();
        }
    }


    public void DemoLevel()
    {
        gc.LoadDemoLevel();
    }

    public void MainLevel()
    {
        gc.LoadMainLevel();
    }
    public void Exit()
    {
        gc.ExitGame();
    }






}
