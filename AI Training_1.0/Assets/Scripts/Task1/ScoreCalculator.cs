using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public int CalculatrScore(string playerMessage)
    {
        int score = 0;

        if (playerMessage.Length < 5)
        {
            return -5;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "因为", "所以", "如果", "那么", "虽然", "但是", "首先", "其次"
        }))
        {
            score += 15;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "数据显示", "研究表明", "据统计", "例如", "比如", "就像"
        }))
        {
            score += 15;
        }

        if (ContainsAny(playerMessage, new[] 
        {
            "我理解", "你说得对", "有道理", "确实", "没错"
        }))
        {
            score += 10;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "不如", "建议", "可以试试", "考虑", "方案"
        }))
        {
            score += 10;
        }

        if (ContainsAny(playerMessage, new[] 
        {
            "问题在于", "漏洞", "矛盾", "不合理", "自相矛盾"
        }))
        {
            score += 15;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "傻", "笨", "蠢", "白痴", "有病"
        }))
        {
            score -= 20;
        }

        if (playerMessage.Length > 10)
        {
            score += 5;
        }

        return Mathf.Clamp(score, -5, 25);
    }

    private bool ContainsAny(string text, string[] keywords)
    {
        foreach (string keyword in keywords)
        {
            if (text.Contains(keyword))
                return true;
        }
        return false;
    }
}
