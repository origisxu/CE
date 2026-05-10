using UnityEngine;

public class ScoreCalculator_6 : MonoBehaviour
{
    // 求助场景评分逻辑（玩家是请求帮助方）
    public int CalculateHelpScore(string playerMessage)
    {
        int score = 0;

        // 太短扣分（描述不清）
        if (playerMessage.Length < 8)
        {
            return -10;
        }

        // 礼貌用语加分
        if (ContainsAny(playerMessage, new[]
        {
            "请问", "您好", "你好", "谢谢", "感谢",
            "麻烦", "打扰", "请教", "拜托"
        }))
        {
            score += 10;
        }

        // 清晰描述问题加分
        if (ContainsAny(playerMessage, new[]
        {
            "问题", "遇到", "情况", "出错", "报错",
            "不知道", "不明白", "为什么", "怎么"
        }))
        {
            score += 10;
        }

        // 提供详细信息加分
        if (ContainsAny(playerMessage, new[]
        {
            "因为", "所以", "之前", "然后", "结果",
            "具体", "详细", "比如", "例如"
        }))
        {
            score += 10;
        }

        // 展示尝试过程加分（说明自己努力过）
        if (ContainsAny(playerMessage, new[]
        {
            "试过", "查过", "百度过", "搜索过", "找过",
            "尝试", "努力", "自己", "先做了"
        }))
        {
            score += 10;
        }

        // 态度诚恳加分
        if (ContainsAny(playerMessage, new[]
        {
            "求助", "帮忙", "帮帮我", "请求", "希望",
            "能不能", "可不可以"
        }))
        {
            score += 10;
        }

        // 不礼貌扣分
        if (ContainsAny(playerMessage, new[]
        {
            "快点", "马上", "现在", "立刻", "必须",
            "要求", "命令"
        }))
        {
            score -= 20;
        }

        // 抱怨指责扣分
        if (ContainsAny(playerMessage, new[]
        {
            "垃圾", "烂", "差劲", "不行", "不好用",
            "什么破", "搞什么"
        }))
        {
            score -= 20;
        }

        // 字数多加分（描述详细）
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