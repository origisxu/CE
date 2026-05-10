using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator_5 : MonoBehaviour
{
    // 请教场景评分逻辑（玩家是被请教方）
    public int CalculateAdviceScore(string playerMessage)
    {
        int score = 0;

        // 太短扣分（建议太敷衍）
        if (playerMessage.Length < 8)
        {
            return -10;
        }

        // 专业建议加分
        if (ContainsAny(playerMessage, new[]
        {
            "建议", "推荐", "可以", "应该", "最好",
            "方案", "方法", "步骤", "流程"
        }))
        {
            score += 10;
        }

        // 具体详细加分
        if (ContainsAny(playerMessage, new[]
        {
            "首先", "其次", "然后", "最后", "第一步",
            "第二步", "注意", "提醒", "关键"
        }))
        {
            score += 10;
        }

        // 经验分享加分
        if (ContainsAny(playerMessage, new[]
        {
            "经验", "之前", "遇到过", "曾经", "以前",
            "我做过", "我试过", "教训"
        }))
        {
            score += 10;
        }

        // 鼓励指导加分
        if (ContainsAny(playerMessage, new[]
        {
            "加油", "相信你", "你可以的", "没问题",
            "慢慢来", "别担心", "放心"
        }))
        {
            score += 10;
        }

        // 反问了解情况加分（好的指导者会先了解情况）
        if (ContainsAny(playerMessage, new[]
        {
            "什么情况", "具体是", "详细说说", "比如",
            "举个例子", "哪个方面"
        }))
        {
            score += 10;
        }

        // 无效建议扣分
        if (ContainsAny(playerMessage, new[]
        {
            "不知道", "不会", "别问我", "自己查",
            "随便", "都行", "看你"
        }))
        {
            score -= 20;
        }

        // 态度不好扣分
        if (ContainsAny(playerMessage, new[]
        {
            "这么简单", "这都不会", "笨", "蠢",
            "浪费时间", "懒得说"
        }))
        {
            score -= 25;
        }

        // 字数多加分（详细建议）
        if (playerMessage.Length > 30)
        {
            score += 5;
        }

        return Mathf.Clamp(score, -15, 35);
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
