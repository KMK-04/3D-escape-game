using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;

public class AIChatManager : MonoBehaviour
{
    private const string API_URL = "https://api.perplexity.ai/chat/completions";

    public ChatManager chatManager;
    private string ChatLog;
    public ContextData data;

    [TextArea]
    public string apiKey = "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"; // 여기에 실제 API 키 입력
    public string Context;
    public string AItext;

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class Payload
    {
        public string model;
        public Message[] messages;
        public int max_tokens;
        public float temperature;
        public bool stream = false;
    }

    void Awake()
    {
        Context = data.Context;
    }

    public static List<string> SplitSmart(string input)
    {
        List<string> result = new List<string>();

        if (string.IsNullOrEmpty(input))
            return result;
        string cleanedInput = Regex.Replace(input, @"\$.*?\$", "");
        string sentencePattern = @"(?<=[.?!])\s+";
        string[] sentences = Regex.Split(cleanedInput, sentencePattern);
        foreach (string s in sentences)
        {
            if (!string.IsNullOrWhiteSpace(s))
                result.Add(s.Trim());
        }
        return result;
    }

    // item.txt 파일에서 텍스트 읽기
    private string ReadItemText()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/item.txt");
        if (File.Exists(path))
        {
            return File.ReadAllText(path, Encoding.UTF8).Trim();
        }
        else
        {
            Debug.LogWarning("item.txt 파일이 존재하지 않습니다.");
            return "";
        }
    }

    public void SendMessageToGPT(string userInput)
    {
        ChatLog += "\n너: " + userInput;
        StartCoroutine(SendChatRequest(ChatLog, userInput));
    }

    IEnumerator SendChatRequest(string chatLog, string userMessage)
    {
        string itemText = ReadItemText(); // item.txt 내용 읽기
        string prompt = chatManager.Prompts;
        string name = chatManager.OtherName;
        string systemPrompt = Context + " \n 당신은 " + name + "이라는 사람이다. 이 사람의 특징은" + prompt + "(이)다.";

        // userMessage 앞에 item.txt 내용 추가
        string fullUserMessage = $"아이템:{itemText} {userMessage}";

        Debug.Log(systemPrompt);
        Payload payload = new Payload()
        {
            model = "sonar-pro",
            messages = new Message[]
            {
                new Message { role = "system", content = systemPrompt },
                new Message { role = "user", content = fullUserMessage }
            },
            max_tokens = 150,
            temperature = 0.7f,
            stream = false
        };

        string jsonData = JsonUtility.ToJson(payload);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest request = new UnityWebRequest(API_URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(postData),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API 요청 실패: " + request.error);
            Debug.LogError("응답 내용: " + request.downloadHandler.text);
        }
        else
        {
            var jsonResponse = JSON.Parse(request.downloadHandler.text);
            Debug.Log(ChatLog);
            AItext = jsonResponse["choices"][0]["message"]["content"];
            ChatLog += "\n나: " + AItext;
            var response = SplitSmart(AItext);
            for (int i = 0; i < response.Count; i++)
            {
                string aiResponse = response[i];
                chatManager.ReceiveMessage(aiResponse);

                int nextLength = (i + 1 < response.Count) ? response[i + 1].Length : 0;
                yield return new WaitForSeconds(0.3f + (nextLength / 10f));
            }
        }
    }

    public void OnSendButtonClicked(string text)
    {
        SendMessageToGPT(text);
    }
}
