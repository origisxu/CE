using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator_4 : MonoBehaviour
{
    // Java后端面试评分逻辑
    public int CalculateBackendScore(string playerMessage)
    {
        int score = 0;

        // 太短扣分
        if (playerMessage.Length < 5)
        {
            return -5;
        }

        // 后端技术关键词加分
        if (ContainsAny(playerMessage, new[]
        {
            "Spring Boot", "Spring Cloud", "微服务", "分布式",
            "Dubbo", "Zookeeper", "消息队列", "RabbitMQ", "Kafka"
        }))
        {
            score += 10;
        }

        // 数据库知识加分
        if (ContainsAny(playerMessage, new[]
        {
            "MySQL", "Redis", "MongoDB", "数据库优化", "索引",
            "事务", "锁", "分库分表", "读写分离"
        }))
        {
            score += 8;
        }

        // Java高级特性加分
        if (ContainsAny(playerMessage, new[]
        {
            "多线程", "并发", "JVM", "垃圾回收", "类加载",
            "反射", "泛型", "集合框架"
        }))
        {
            score += 8;
        }

        // 系统设计加分
        if (ContainsAny(playerMessage, new[]
        {
            "高并发", "高可用", "负载均衡", "缓存", "降级",
            "限流", "熔断", "容灾"
        }))
        {
            score += 10;
        }

        // 项目架构加分
        if (ContainsAny(playerMessage, new[]
        {
            "架构设计", "技术选型", "系统设计", "方案设计",
            "性能优化", "代码重构"
        }))
        {
            score += 8;
        }

        // 问题回答不专业扣分
        if (ContainsAny(playerMessage, new[]
        {
            "不知道", "不会", "没学过", "没做过", "不清楚"
        }))
        {
            score -= 15;
        }

        // 经验不足扣分
        if (ContainsAny(playerMessage, new[]
        {
            "刚毕业", "没有经验", "实习生", "初学者"
        }))
        {
            score -= 10;
        }

        // 字数多加分
        if (playerMessage.Length > 20)
        {
            score += 5;
        }

        return Mathf.Clamp(score, -10, 35);
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
