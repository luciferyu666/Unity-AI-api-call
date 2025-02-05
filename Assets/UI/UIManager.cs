using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField inputField;
    public Button sendButton;
    public Text responseText;

    private void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendMessage);
        }
    }

    private void OnSendMessage()
    {
        if (string.IsNullOrEmpty(inputField.text))
            return;

        string userMessage = inputField.text;
        // 呼叫 AIService
        AIService.Instance.SendMessageToAI(userMessage, OnAIResponse);
    }

    private void OnAIResponse(string aiResponse)
    {
        // 顯示在 UI 上
        if (responseText != null)
        {
            responseText.text = aiResponse;
        }
    }
}
