using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public void RotateObjectY(string objectName, float duration)
    {
        GameObject targetObject = GameObject.Find(objectName);
        if (targetObject != null)
        {
            StartCoroutine(RotateOverTime(targetObject, duration));
        }
        else
        {
            Debug.LogWarning("Objeto no encontrado: " + objectName);
        }
    }

    private System.Collections.IEnumerator RotateOverTime(GameObject targetObject, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            targetObject.transform.Rotate(0, 360 * (Time.deltaTime / duration), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
