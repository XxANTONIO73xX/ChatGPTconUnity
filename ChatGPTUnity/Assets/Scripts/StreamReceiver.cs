using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class StreamReceiver : MonoBehaviour
{
    private const string streamUrl = "http://192.168.1.95:5000/stream";  // Cambia a la IP de tu servidor Flask
    private List<GameObject> lineList = new List<GameObject>();
    private DD_DataDiagram m_DataDiagram;
    private bool isStreaming = false;
    private float startTime;

    void Start()
    {
        GameObject dd = GameObject.Find("DataDiagram");
        if (null == dd)
        {
            Debug.LogWarning("Cannot find a GameObject named DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();

        if (m_DataDiagram == null)
        {
            Debug.LogWarning("Cannot find a component DD_DataDiagram on DataDiagram GameObject");
            return;
        }

        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };

        AddALine();
        StartCoroutine(StartStream());
    }

    void OnDestroy()
    {
        isStreaming = false;
    }

    private void AddALine()
    {
        if (null == m_DataDiagram)
            return;

        Color color = Color.red;  // Puedes cambiar el color según tu preferencia
        GameObject line = m_DataDiagram.AddLine("ECG", color);
        if (null != line)
            lineList.Add(line);
    }

    private IEnumerator StartStream()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(streamUrl))
        {
            request.SetRequestHeader("Accept", "text/event-stream");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error starting stream: " + request.error);
                yield break;
            }

            isStreaming = true;
            startTime = Time.time;

            StringBuilder sb = new StringBuilder();
            while (isStreaming && !request.isDone)
            {
                string newData = request.downloadHandler.text;
                if (!string.IsNullOrEmpty(newData))
                {
                    sb.Append(newData);
                    string[] lines = sb.ToString().Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        if (lines[i].StartsWith("data:"))
                        {
                            string jsonData = lines[i].Substring(5);
                            ProcessData(jsonData);
                        }
                    }

                    sb.Clear();
                    if (lines.Length > 0)
                    {
                        sb.Append(lines[lines.Length - 1]);
                    }
                }

                yield return null;
            }
        }
    }

    private void ProcessData(string jsonData)
    {
        Debug.Log("Received data: " + jsonData);

        DataModel data = JsonUtility.FromJson<DataModel>(jsonData);
        Debug.Log("ECG Value: " + data.ecg_value);
        Debug.Log("HR Value: " + data.hr_value);

        // Graficar el valor del ECG en m_DataDiagram
        float elapsedTime = Time.time - startTime;
        foreach (GameObject line in lineList)
        {
            if (line != null)
            {
                m_DataDiagram.InputPoint(line, new Vector2(elapsedTime, data.ecg_value));
            }
        }
    }

    [System.Serializable]
    private class DataModel
    {
        public float ecg_value;
        public int hr_value;
    }
}
