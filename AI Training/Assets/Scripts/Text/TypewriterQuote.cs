using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterQuote : MonoBehaviour
{
    public Text quoteText;
    public float fadeDuration = 1f;      // 渐变时间（淡入和淡出）
    public float displayDuration = 3f;   // 显示停留时间

    private string[] quotes = {
        "逻辑会带你从A点到B点，想象力将带你去任何地方。——爱因斯坦",
        "有效的沟通取决于说话者所说的话被对方理解的方式。",
        "清晰的表达是清晰思考的结果。",
        "辩论的目的不是赢，而是找到真相。",
        "三思而后行。",
        "言之有理，行之有据。",
        "逻辑是思考的骨架，表达是思想的外衣。"
    };

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(PlayQuotesLoop());
    }

    IEnumerator PlayQuotesLoop()
    {
        while (true)
        {
            // 设置文字内容
            quoteText.text = quotes[currentIndex];

            // 开始时完全透明
            SetAlpha(0f);

            // 淡入
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

            // 停留显示
            yield return new WaitForSeconds(displayDuration);

            // 淡出
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

            // 切换到下一句
            currentIndex++;
            if (currentIndex >= quotes.Length)
            {
                currentIndex = 0;
            }
        }
    }

    // 渐变协程：从 startAlpha 到 endAlpha，持续 duration 秒
    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color color = quoteText.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            color.a = alpha;
            quoteText.color = color;
            yield return null;
        }

        // 确保最终值精确
        color.a = endAlpha;
        quoteText.color = color;
    }

    // 辅助方法：直接设置透明度
    void SetAlpha(float alpha)
    {
        Color color = quoteText.color;
        color.a = alpha;
        quoteText.color = color;
    }
}