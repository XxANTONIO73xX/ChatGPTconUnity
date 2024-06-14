using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TextPageSwitcher : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public Button nextButton;
    public Button prevButton;
    private int currentPage = 1;
    private bool isNavigating = false; // Añadido para evitar múltiples clics
    public float coolDownTime = 0.5f; // Tiempo de espera entre clics

    void Start()
    {
        if (textMeshProUGUI == null)
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            if (textMeshProUGUI == null)
            {
                Debug.LogError("TextMeshProUGUI component is not assigned and could not be found on the GameObject.");
                return;
            }
        }

        if (nextButton == null)
        {
            Debug.LogError("Next Button is not assigned.");
            return;
        }

        if (prevButton == null)
        {
            Debug.LogError("Prev Button is not assigned.");
            return;
        }

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);

        // Forzar la actualización del texto para calcular correctamente el pageCount
        textMeshProUGUI.ForceMeshUpdate();

        Debug.Log("Text Page Count at Start: " + textMeshProUGUI.textInfo.pageCount);

        UpdatePageButtons();
    }

    public void UpdateContent(string newText)
    {
        textMeshProUGUI.text = newText;
        textMeshProUGUI.ForceMeshUpdate();
        currentPage = 1; // Resetear a la primera página
        UpdatePage();
    }

    public void NextPage()
    {
        if (isNavigating) return;
        StartCoroutine(NavigateToNextPage());
    }

    public void PreviousPage()
    {
        if (isNavigating) return;
        StartCoroutine(NavigateToPreviousPage());
    }

    private IEnumerator NavigateToNextPage()
    {
        isNavigating = true;

        int pageCount = textMeshProUGUI.textInfo.pageCount;

        if (currentPage < pageCount)
        {
            currentPage++;
            UpdatePage();
        }

        yield return new WaitForSeconds(coolDownTime);
        isNavigating = false;
    }

    private IEnumerator NavigateToPreviousPage()
    {
        isNavigating = true;

        if (currentPage > 1)
        {
            currentPage--;
            UpdatePage();
        }

        yield return new WaitForSeconds(coolDownTime);
        isNavigating = false;
    }

    private void UpdatePage()
    {
        textMeshProUGUI.pageToDisplay = currentPage;
        UpdatePageButtons();
        Debug.Log("Current Page: " + currentPage);
    }

    private void UpdatePageButtons()
    {
        if (prevButton == null || nextButton == null)
        {
            Debug.LogError("Buttons are not assigned properly.");
            return;
        }

        int pageCount = textMeshProUGUI.textInfo.pageCount;

        if (pageCount == 0)
        {
            Debug.LogError("Page count is zero. Make sure the text is properly assigned and long enough to span multiple pages.");
            return;
        }

        prevButton.interactable = currentPage > 1;
        nextButton.interactable = currentPage < pageCount;
    }
}
