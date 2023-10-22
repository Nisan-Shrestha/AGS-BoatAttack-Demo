using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject LoadParent;
    public Image LoadFillImage;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LoadParent = GameObject.FindGameObjectWithTag("loading1");
            LoadParent.SetActive(false);
        }
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;
        if (currentName == null)
        {
            currentName = "Replaced";
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LoadParent = GameObject.FindGameObjectWithTag("loading1");
            LoadFillImage = GameObject.FindGameObjectWithTag("loading2").GetComponent<Image>();
            LoadParent.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadMainMenu()
    {
        StartCoroutine(LoadYourAsyncScene(0));
    }

    public void LoadDemoLevel()
    {
        StartCoroutine(LoadYourAsyncScene(1));
    }

    public void LoadMainLevel()
    {
        StartCoroutine(LoadYourAsyncScene(2));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadYourAsyncScene(int buildIndex)
    {
        Transform temp = null;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            temp = LoadParent.transform.Find("wheel");
            LoadParent.SetActive(true);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex,LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (temp != null)
                {
                    temp.eulerAngles += new Vector3(0, 0, 3);
                }
                float progressValue =  Mathf.Clamp01(asyncLoad.progress / .9f);
                LoadFillImage.fillAmount = progressValue;
            }
            yield return null;

        }
    }

}
