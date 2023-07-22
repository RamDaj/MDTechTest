using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanelInteract;

    private void Start()
    {
        //_mainCam = Camera.main;
        _uiPanelInteract.SetActive(false);
    }


    public bool isDisplayed = false;

    public void SetUpInteract()
    {
        _uiPanelInteract.SetActive(true);
        isDisplayed = true;
    }

    public void CloseInteract()
    {
        _uiPanelInteract.SetActive(false);
        isDisplayed = false;
    }
}
