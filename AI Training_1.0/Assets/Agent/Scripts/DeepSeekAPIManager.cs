using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class DeepSeekAPIManager : MonoBehaviour
{
    public Config config;
    private const string API_URL = "https://api.deepseek.com/v1/chat/completions";

    private List<ApiMessage> conversationHistory = new List<ApiMessage>();


    [System.Serializable]
    public class ApiMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ApiRequest
    {
        public string model;
        public ApiMessage[] messages;
        public float temperature;
        public int max_tokens;
    }

    [System.Serializable]
    public class ApiChoice
    {
        public ApiMessage message;
    }

    [System.Serializable]
    public class ApiResponse
    {
        public ApiChoice[] choices;
    }

    public void SendChatMessage(string userMessage, System.Action<string> callback)
    {
        // 添加用户消息到历史
        conversationHistory.Add(new ApiMessage
        {
            role = "user",
            content = userMessage
        });

        // 确保不超过最大历史长度
        TrimConversationHistory();

        // 修正：移除userMessage参数，因为现在从conversationHistory获取
        StartCoroutine(SendRequestCoroutine(callback));
    }


    private IEnumerator SendRequestCoroutine(System.Action<string> callback)
    {
        ApiRequest requestData = new ApiRequest
        {
            model = config.model,
            messages = conversationHistory.ToArray(), // 发送完整对话历史
            temperature = 0.7f,
            max_tokens = 1024
        };

        string jsonData = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + Config.apikey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("API原始响应: " + request.downloadHandler.text); // 添加调试日志

            ApiResponse response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
            if (response.choices != null && response.choices.Length > 0)
            {
                // 先不过滤，直接获取原始回复
                string reply = response.choices[0].message.content;
                Debug.Log("原始回复内容: " + reply);

                // 添加AI回复到历史
                conversationHistory.Add(new ApiMessage
                {
                    role = "assistant",
                    content = reply // 注意：这里使用未过滤的内容
                });

                callback(reply);
            }
            else
            {
                Debug.LogError("API返回数据异常");
                callback(null);
            }
        }
        else
        {
            Debug.LogError($"API请求失败: {request.error}");
            callback(null);
        }

        request.Dispose();
    }

    private void TrimConversationHistory()
    {
        // 最大消息数 = 配置的maxHistoryLength × 2（一问一答为一组）
        int maxMessages = config.maxHistoryLength * 2;

        // 保留最近的N条消息（但至少保留1条）
        while (conversationHistory.Count > maxMessages && maxMessages > 0)
        {
            conversationHistory.RemoveAt(0); // 移除最旧的消息
        }
    }

    // 过滤**符号的方法
    private string FilterAsterisks(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // 使用正则表达式替换所有**符号
        return Regex.Replace(input, @"\*{2}", "");
    }
}