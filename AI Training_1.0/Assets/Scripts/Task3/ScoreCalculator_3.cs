using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator_3 : MonoBehaviour
{
   public int CalculateFrontendScore(string playerMessage)
    {
        int score = 0;

        if (playerMessage.Length < 5)
        {
            return -5;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "HTML", "CSS", "JavaScript", "Vue", "React", "Angular",
            "响应式", "组件", "状态管理", "路由", "Webpack", "Babel"
        }))
        {
            score += 7;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "Java", "Spring", "MVC", "注解", "依赖注入", "AOP",
            "MyBatis", "Hibernate", "JPA"
        }))
        {
            score += 3;
        }

        if (ContainsAny(playerMessage, new[]
      {
            "项目经验", "开发过", "独立完成", "负责", "参与开发",
            "上线项目", "优化性能"
        }))
        {
            score += 15;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "学习", "研究", "掌握", "了解", "熟悉", "精通"
        }))
        {
            score += 5;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "团队", "协作", "沟通", "配合", "帮助", "分享"
        }))
        {
            score += 10;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "不知道", "不会", "没学过", "没做过", "不清楚"
        }))
        {
            score -= 15;
        }

        if (ContainsAny(playerMessage, new[]
        {
            "可能吧", "也许", "大概", "好像是", "应该是"
        }))
        {
            score -= 5;
        }

        if (playerMessage.Length > 20)
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
