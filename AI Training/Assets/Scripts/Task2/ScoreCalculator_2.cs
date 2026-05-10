using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator_2 : MonoBehaviour
{
    // 愤怒主题的评分逻辑
    public int CalculateAngryScore(string playerMessage)
    {
        int score = 0;

        // 太短扣分
        if (playerMessage.Length < 5)
        {
            return -10;
        }

        // 安抚话语加分
        if (ContainsAny(playerMessage, new[]
        {
            "冷静", "别生气", "息怒", "消消气", "放松"
        }))
        {
            score += 20;
        }

        // 理解对方加分
        if (ContainsAny(playerMessage, new[]
        {
            "我理解", "我知道", "你说得对", "有道理", "我懂"
        }))
        {
            score += 15;
        }

        // 道歉加分
        if (ContainsAny(playerMessage, new[]
        {
            "对不起", "抱歉", "是我的错", "不好意思"
        }))
        {
            score += 15;
        }

        // 建议解决方案加分
        if (ContainsAny(playerMessage, new[]
        {
            "不如", "建议", "可以试试", "要不", "方案"
        }))
        {
            score += 10;
        }

        // 火上浇油扣分
        if (ContainsAny(playerMessage, new[]
        {
            "你错了", "不对", "你才", "活该", "自找"
        }))
        {
            score -= 25;
        }

        // 骂人扣分
        if (ContainsAny(playerMessage, new[]
        {
            "傻", "笨", "蠢", "白痴", "有病", "滚"
        }))
        {
            score -= 30;
        }

        // 字数多加分
        if (playerMessage.Length > 10)
        {
            score += 5;
        }

        return Mathf.Clamp(score, -10, 30);
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