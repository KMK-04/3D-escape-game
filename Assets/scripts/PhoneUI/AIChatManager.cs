using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


public class AIChatManager : MonoBehaviour {
    private const string API_URL = "https://api.perplexity.ai/chat/completions";

    public ChatManager chatManager;
    private string ChatLog;

    [TextArea]
    public string apiKey = "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"; // 여기에 실제 API 키 입력
    public string Context;
    public string AItext;


    [System.Serializable]
    public class Message {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class Payload {
        public string model;
        public Message[] messages;
        public int max_tokens;
        public float temperature;
        public bool stream = false;
    }

    void Awake() {
        GameObject phoneObj = GameObject.Find("Phone");
        if (phoneObj != null) {
            Phone phone = phoneObj.GetComponent<Phone>();
            if (phone != null) {
                Context = phone.context;
                Debug.Log(Context);
            }
            else {
                Debug.LogError("Phone 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else {
            Debug.LogError("\"Phone\" 오브젝트를 찾을 수 없습니다.");
        }
    }
    public static List<string> SplitSmart(string input) {
        List<string> result = new List<string>();

        if (string.IsNullOrEmpty(input))
            return result;
        string cleanedInput = Regex.Replace(input, @"\$.*?\$", "");
        string sentencePattern = @"(?<=[.?!])\s+";
        string[] sentences = Regex.Split(cleanedInput, sentencePattern);
        foreach (string s in sentences) {
            if (!string.IsNullOrWhiteSpace(s))
                result.Add(s.Trim());
        }
        return result;
    }
    public void SendMessageToGPT(string userInput) {
        ChatLog += "너: " + userInput;
        StartCoroutine(SendChatRequest(ChatLog));
    }
    IEnumerator SendChatRequest(string userMessage) {
        string prompt = chatManager.Prompts;
        string name = chatManager.OtherName;
        string systemPrompt = Context + " \n 당신은 " + name + "이라는 사람이다. 이 사람의 특징은" + prompt + "(이)다.";
        Debug.Log(systemPrompt);
        Payload payload = new Payload() {
            model = "sonar-pro", // 필요에 따라 "sonar-pro"로 변경 가능
            messages = new Message[]
            {
                new Message { role = "system", content = systemPrompt },
                new Message { role = "user", content = userMessage }
            },
            max_tokens = 150,
            temperature = 0.7f,
            stream = false
        };

        string jsonData = JsonUtility.ToJson(payload);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest request = new UnityWebRequest(API_URL, "POST") {
            uploadHandler = new UploadHandlerRaw(postData),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError("API 요청 실패: " + request.error);
            Debug.LogError("응답 내용: " + request.downloadHandler.text);
        }
        else {
            var jsonResponse = JSON.Parse(request.downloadHandler.text);
            AItext = jsonResponse["choices"][0]["message"]["content"];
            ChatLog += "\n나: " + AItext;
            var response = SplitSmart(AItext);
            foreach (string aiResponse in response) {
                chatManager.ReceiveMessage(aiResponse);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void OnSendButtonClicked(string text) {
        SendMessageToGPT(text);
    }
}
