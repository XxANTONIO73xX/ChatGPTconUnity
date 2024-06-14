using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using TMPro; // Asegúrate de incluir esto para trabajar con TextMeshProUGUI

public class ChatGPTManager : MonoBehaviour
{
    public OnResponseEvent OnResponse;

    [System.Serializable]
    public class OnResponseEvent : UnityEvent<string> { }

    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();
    private string lastResponse;

    public TextMeshProUGUI responseText; // Referencia al componente TextMeshProUGUI en la UI
    public TextPageSwitcher pageSwitcher; // Referencia al TextPageSwitcher

    // Start is called before the first frame update
    void Start()
    {
        if (responseText != null)
        {
            lastResponse = responseText.text; // Inicializar lastResponse con el texto actual del componente TextMeshProUGUI
        }
        else
        {
            lastResponse = "¿En qué te puedo ayudar?"; // Valor predeterminado en caso de que responseText no esté asignado
        }
    }

    public async void AskChatGPT(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            Debug.Log(lastResponse);
            OnResponse.Invoke(lastResponse);
            return;
        }

        ChatMessage newMessage = new ChatMessage
        {
            Content = newText + " Tiene que ser muy muy muy resumida y de un solo párrafo y debe tener inicio, nudo y final.",
            Role = "user"
        };

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest
        {
            Messages = messages,
            Model = "gpt-3.5-turbo"
        };

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);

            lastResponse = chatResponse.Content; // Actualizar el último mensaje de respuesta
            OnResponse.Invoke(chatResponse.Content);

            // Notificar al TextPageSwitcher sobre el nuevo contenido
            if (pageSwitcher != null)
            {
                pageSwitcher.UpdateContent(responseText.text);
            }
        }
    }

    void Update()
    {
    }
}
