using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // 配置变量
    public Config config;
    public DeepSeekAPIManager apiManager;
    public TMP_InputField userInput;
    public TMP_Text chatDisplay;

    // 状态变量
    private bool isFirstMessage = true;
    private bool isTyping = false;
    private string currentTypingMessage = "";
    private int currentTypingIndex = 0;

    public Button sendButton;

    private ITaskHandler taskHandler;

    void Start()
    {
        chatDisplay.text = Config.chatdisplay;

        MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        // 找出实现了 ITaskHandler 的那个
        foreach (var behaviour in allBehaviours)
        {
            if (behaviour is ITaskHandler handler)
            {
                taskHandler = handler;
                break;
            }
        }


        if (taskHandler == null)
        {
            Debug.LogError("未找到实现了 ITaskHandler 的脚本！");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(userInput.text))
        {
            OnSendButtonClick();
        }


        if (isTyping && !string.IsNullOrEmpty(currentTypingMessage))
        {
            UpdateTypingEffect();
        }
    }

    public void OnSendButtonClick()
    {
        string userMessage = userInput.text + "禁止在括号里写你的动作和神态!记住你的性格特点";
        if (string.IsNullOrEmpty(userMessage)) return;

        if (taskHandler != null && !taskHandler.IsGameOver())
        {
            taskHandler.OnPlayerMessage(userMessage);
        }

        chatDisplay.text += $"\n你: {userInput.text}\n\n";
        userInput.text = "";

        string apiMessage = userMessage;
        if (isFirstMessage)
        {
            apiMessage = config.xingge + "现在开始第一个对话。请随机选择一个场景开场,禁止在括号里写你的动作和神态，直接说话即可！听我开始讲话，你不需要回复我这句话！直接代入这个角色！！！！" + userMessage;
            isFirstMessage = false;
        }

        apiManager.SendChatMessage(apiMessage, (reply) =>
        {
            if (!string.IsNullOrEmpty(reply))
            {
                StartTypingEffect($"{config.ainame}: {reply}");
            }
            else
            {
                chatDisplay.text += $"\n{config.ainame}: 请求失败，请检查API设置";
            }
        });
    }

    // 开始打字效果（不立即修改文本）
    private void StartTypingEffect(string message)
    {
        isTyping = true;
        currentTypingMessage = message;
        currentTypingIndex = 0;
    }

    // 每帧更新打字效果
    private void UpdateTypingEffect()
    {
        if (currentTypingIndex < currentTypingMessage.Length)
        {
            // 添加下一个字符到显示文本
            chatDisplay.text += currentTypingMessage[currentTypingIndex];
            currentTypingIndex++;

        }
        else
        {
            isTyping = false;
            currentTypingMessage = "";
            chatDisplay.text += "\n";
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        userInput.interactable = enabled;
        if (sendButton != null)
            sendButton.interactable = enabled;
    }
}




