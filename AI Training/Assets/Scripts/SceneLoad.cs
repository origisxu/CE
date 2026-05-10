using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoad : MonoBehaviour
{

    public GameObject eventobj;
    public Button btn_to_task2;
    public Button btn_to_task1;
    public Button btn_to_task3;
    public Button btn_to_task4;
    public Button btn_to_task5;
    public Button btn_to_task6;

    public Animator anim;
    void Start()
    {
        GameObject.DontDestroyOnLoad(eventobj);

        btn_to_task2.onClick.AddListener(LoadScene_task2);
        btn_to_task1.onClick.AddListener(LoadSecne_task1);
        btn_to_task3.onClick.AddListener(LoadSecne_task3);
        btn_to_task4.onClick.AddListener(LoadSecne_task4);
        btn_to_task5.onClick.AddListener(LoadSecne_task5);
        btn_to_task6.onClick.AddListener(LoadSecne_task6);
    }

    private void LoadScene_task2()
    {
        StartCoroutine(LoadScene(2));
    }

    private void LoadSecne_task1()
    {
        StartCoroutine(LoadScene(2));
    }

    private void LoadSecne_task3()
    {
        StartCoroutine(LoadScene(2));
    }

    private void LoadSecne_task4()
    {
        StartCoroutine(LoadScene(2));
    }

    private void LoadSecne_task5()
    {
        StartCoroutine(LoadScene(2));
    }

    private void LoadSecne_task6()
    {
        StartCoroutine(LoadScene(2));
    }


    IEnumerator LoadScene(int index)
    {
        anim.SetBool("FadeIn",true);
        anim.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1);

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.completed += OnLoadScene;
    }

    private void OnLoadScene(AsyncOperation obj)
    {
        if (anim != null)  // 加上这行判断
        {
            anim.SetBool("FadeIn", false);
            anim.SetBool("FadeOut", true);
        }
    }
}
