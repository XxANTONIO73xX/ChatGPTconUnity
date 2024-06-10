using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextPageSwitcher : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public Button nextButton;
    public Button prevButton;
    private int currentPage = 1;

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

    public void NextPage()
    {
        if (currentPage < textMeshProUGUI.textInfo.pageCount)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        textMeshProUGUI.pageToDisplay = currentPage;
        UpdatePageButtons();
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

        Debug.Log("Current Page: " + currentPage);
        Debug.Log("Page Count: " + pageCount);

        prevButton.interactable = currentPage > 1;
        nextButton.interactable = currentPage < pageCount;
    }
}
