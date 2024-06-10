using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;

public class ChatGPTManager : MonoBehaviour
{

    public OnResponseEvent OnResponse;

    [System.Serializable]

    public class OnResponseEvent : UnityEvent<string>{}


    private OpenAIApi openAI = new OpenAIApi(); // Corregido aquí
    private List<ChatMessage> messages = new List<ChatMessage>();


    // Start is called before the first frame update
    
    public async void AskChatGPT(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            Debug.Log("¿En qué te puedo ayudar?");
            OnResponse.Invoke("¿En qué te puedo ayudar?");
            return;
        }
    
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText + "Tiene que ser muy muy muy resumida y de un solo parrafo y debe tener inicio, nudo y final.";
        newMessage.Role = "user";
    
        messages.Add(newMessage);
    
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";
    
        var response = await openAI.CreateChatCompletion(request);
    
        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);
    
            OnResponse.Invoke(chatResponse.Content);
        }
    }




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}