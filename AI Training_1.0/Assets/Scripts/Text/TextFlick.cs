using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlick : MonoBehaviour
{
    public float speed = 2f;

    private Text text;
    private float timer;

    void Start()
    {
        text=GetComponent<Text>();
    }

    void Update()
    {
        float alpha = 0.3f + (Mathf.Sin(timer * speed) + 1f) / 2f * 0.7f;
        Color color = text.color;
        color.a = alpha;
        text.color = color;

        timer += Time.deltaTime;
    }
}
