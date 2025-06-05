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
        string[] words = input.Split(' ');
        string buffer = "";
        List<string> tempChunks = new List<string>();
        foreach (string word in words) {
            if ((buffer + " " + word).Trim().Length <= 30) {
                buffer = (buffer + " " + word).Trim();
            }
            else {
                if (!string.IsNullOrEmpty(buffer)) tempChunks.Add(buffer);
                buffer = word;
            }
        }
        if (!string.IsNullOrEmpty(buffer)) tempChunks.Add(buffer);
        string sentencePattern = @"(?<=[.?!])\s+";
        foreach (string chunk in tempChunks) {
            string[] sentences = Regex.Split(chunk, sentencePattern);
            foreach (string s in sentences) {
                if (!string.IsNullOrWhiteSpace(s))
                    result.Add(s.Trim());
            }
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
        string systemPrompt = "60자 이내로 대답해. 우리가 나눈 전체 대화 내용을 보여줄테니, 이 다음에 맞는 대답을 하도록 해. \"나: \"는 너가 보낸 메세지고 \"너\"는 내가 보낸 메세지야. 메세지를 보낼 때는 참고자료를 최대한 보내지 말고 대화하듯이 자연스러운 말투로 해야해. 보낼때는\"나:\" 이런거 안보내도 괜찮아."
         + "지금은 " + Context + "이런 상황이야. 너는 " + name + "이라는 사람이야. 이 사람의 특징은" + prompt + "(이)야.";
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
