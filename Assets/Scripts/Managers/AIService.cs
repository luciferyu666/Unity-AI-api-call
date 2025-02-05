using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class AIService : MonoBehaviour
{
    private static AIService _instance;
    public static AIService Instance
    {
        get
        {
            if (_instance == null)
            {
                var aiObj = new GameObject("AIService");
                _instance = aiObj.AddComponent<AIService>();
                DontDestroyOnLoad(aiObj);
            }
            return _instance;
        }
    }

    [Header("AI API Settings")]
    [SerializeField] private string apiEndpoint = "https://api.openai.com/v1/chat/completions"; 
    [SerializeField] private string apiKey = "YOUR_API_KEY";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 呼叫 AI API，傳入玩家訊息，並取得回應
    /// </summary>
    public void SendMessageToAI(string userMessage, Action<string> onResponseReceived)
    {
        StartCoroutine(SendRequest(userMessage, onResponseReceived));
    }

    private IEnumerator SendRequest(string userMessage, Action<string> onResponseReceived)
    {
        // API 所需的 JSON 格式 (以 ChatGPT 為例)
        // 如果使用其他 AI 生成式 API，請依照官方文件修改
        var requestData = new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
                new { role = "user", content = userMessage }
            }
        };

        string jsonBody = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 設定 Header
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("AI API Error: " + request.error);
                onResponseReceived?.Invoke("Error: " + request.error);
            }
            else
            {
                Debug.Log("AI API Response: " + request.downloadHandler.text);
                
                // 實際情況需解析 JSON，以下僅示意
                // OpenAI 回傳的內容可用 JSON 反序列化取出
                string aiResponse = ParseAIResponse(request.downloadHandler.text);
                onResponseReceived?.Invoke(aiResponse);
            }
        }
    }

    /// <summary>
    /// 範例：解析 AI 回應
    /// 請依照實際 JSON 結構使用 JSON 解析套件（如 Newtonsoft.Json）取出回應文字
    /// 這裡僅示意性地回傳整段文字
    /// </summary>
    private string ParseAIResponse(string jsonString)
    {
        // TODO: 解析回應中的 content
        // e.g. dynamic data = JsonConvert.DeserializeObject(jsonString);
        // return data.choices[0].message.content;
        return jsonString;
    }
}
