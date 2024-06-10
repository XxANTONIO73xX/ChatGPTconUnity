using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextExtractor : MonoBehaviour
{
    public Text uiText;
    public float interval = 3f; // Intervalo de 3 segundos
    public string[] actions = new string[] { "girar", "mover" };
    public string[] directions = new string[] { "derecha", "izquierda", "adelante", "atrás", "arriba", "abajo" };
    public string[] times = new string[] { "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
    public string[] objectNames = new string[] { "cubo rojo", "cubo azul" };

    private string matchedAction;
    private string matchedDirection;
    private string matchedTime;
    private string matchedObjectName;

    public ObjectMotion objectMotionScript; // Referencia pública a ObjectMotion

    void Start()
    {
        if (uiText == null)
        {
            uiText = GetComponent<Text>();
            if (uiText == null)
            {
                Debug.LogError("Text component is not assigned and could not be found on the GameObject.");
                return;
            }
        }
        StartCoroutine(PrintTextCoroutine());
    }

    IEnumerator PrintTextCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log(uiText.text);
            MatchWords();
        }
    }

    void MatchWords()
    {
        string text = uiText.text.ToLower();

        matchedAction = FindMatch(text, actions);
        matchedDirection = FindMatch(text, directions);
        matchedTime = FindMatch(text, times);
        matchedObjectName = FindMatch(text, objectNames);
        
        // Enviar los valores coincidentes al script ObjectMotion
        if (objectMotionScript != null)
        {
            float timeInSeconds = ConvertTimeToSeconds(matchedTime);
            objectMotionScript.MadeMotionAction(matchedObjectName, matchedAction, timeInSeconds, matchedDirection);
        }
    }

    string FindMatch(string text, string[] words)
    {
        foreach (string word in words)
        {
            if (text.Contains(word.ToLower()))
            {
                return word;
            }
        }
        return null;
    }

    float ConvertTimeToSeconds(string timeString)
    {
        switch (timeString)
        {
            case "uno": return 1.0f;
            case "dos": return 2.0f;
            case "tres": return 3.0f;
            case "cuatro": return 4.0f;
            case "cinco": return 5.0f;
            case "seis": return 6.0f;
            case "siete": return 7.0f;
            case "ocho": return 8.0f;
            case "nueve": return 9.0f;
            default: return 1.0f; // Valor por defecto si no se encuentra coincidencia
        }
    }
}
