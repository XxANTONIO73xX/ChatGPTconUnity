using System.Collections;
using UnityEngine;

public class ObjectMotion : MonoBehaviour
{
    public string objectoName;
    public string action;
    public float time;
    public string direction;

    void Start()
    {
        // Puedes inicializar aquí si es necesario
    }

    public void MadeMotionAction(string objectoName, string action, float time, string direction)
    {
        GameObject obj = GameObject.Find(objectoName);
        if (obj != null)
        {
            if (action == "girar")
            {
                StartCoroutine(MadeRotation(obj, direction, time));
            }
            else if (action == "mover")
            {
                StartCoroutine(MadeMovement(obj, direction, time));
            }
        }
        else
        {
            Debug.LogError($"Object with name '{objectoName}' not found.");
        }
    }

    IEnumerator MadeRotation(GameObject obj, string direction, float time)
    {
        float elapsedTime = 0;
        Vector3 rotationAxis = Vector3.zero;

        switch (direction)
        {
            case "derecha":
                rotationAxis = Vector3.up;
                break;
            case "izquierda":
                rotationAxis = Vector3.down;
                break;
            case "adelante":
                rotationAxis = Vector3.right;
                break;
            case "atrás":
                rotationAxis = Vector3.left;
                break;
            case "arriba":
                rotationAxis = Vector3.forward;
                break;
            case "abajo":
                rotationAxis = Vector3.back;
                break;
            default:
                Debug.LogError("Invalid direction for rotation.");
                yield break;
        }

        while (elapsedTime < time)
        {
            obj.transform.Rotate(rotationAxis, (360 / time) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MadeMovement(GameObject obj, string direction, float time)
    {
        float elapsedTime = 0;
        Vector3 initialPosition = obj.transform.position;
        Vector3 targetPosition = initialPosition;

        switch (direction)
        {
            case "derecha":
                targetPosition += new Vector3(5.0f, 0, 0);
                break;
            case "izquierda":
                targetPosition += new Vector3(-5.0f, 0, 0);
                break;
            case "arriba":
                targetPosition += new Vector3(0, 5.0f, 0);
                break;
            case "abajo":
                targetPosition += new Vector3(0, -5.0f, 0);
                break;
            case "adelante":
                targetPosition += new Vector3(0, 0, 5.0f);
                break;
            case "atrás":
                targetPosition += new Vector3(0, 0, -5.0f);
                break;
            default:
                Debug.LogError("Invalid direction for movement.");
                yield break;
        }

        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure final position is exact
    }

    void Update()
    {
        // Puedes utilizar Update si necesitas escuchar eventos o condiciones adicionales
    }
}
