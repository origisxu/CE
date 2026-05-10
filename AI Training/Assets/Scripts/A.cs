using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRoundManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text roundText;

    public  int currentRound = 0;
    public   int maxRound = 7;

    void Start()
    {
        sendButton.onClick.AddListener(OnSendMessage);
        UpdateRoundUI();
    }

    void Update()
    {
        if (inputField != null && inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            OnSendMessage();
        }
    }

    void OnSendMessage()
    {
        if (currentRound >= maxRound) return;
        
        string message = inputField.text;
        if (string.IsNullOrEmpty(message)) return;
        
        currentRound++;
        UpdateRoundUI();
        
        inputField.text = "";
        
        Debug.Log($"第{currentRound}轮，内容：{message}");
        
        if (currentRound >= maxRound)
        {
            roundText.text = "对话已结束";
            sendButton.interactable = false;
            inputField.interactable = false;
        }
    }

    void UpdateRoundUI()
    {
        if (roundText != null)
        {
            roundText.text = $"剩余轮数：{7 - currentRound} / {7}";
        }
    }
}