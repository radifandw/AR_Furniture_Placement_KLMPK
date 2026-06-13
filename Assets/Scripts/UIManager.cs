using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject homePanel;
    public GameObject backButton;

    [Header("AR Reference")]
    public FurniturePlacement furniturePlacement;

    void Start()
    {
        ShowHome();
    }

    public void ShowHome()
    {
        if (homePanel != null) homePanel.SetActive(true);
        if (backButton != null) backButton.SetActive(false);
    }

    public void StartAR()
    {
        if (homePanel != null) homePanel.SetActive(false);
        if (backButton != null) backButton.SetActive(true);
    }

    public void SelectKasur()
    {
        if (furniturePlacement != null)
            furniturePlacement.ChangeFurniture(0);
        StartAR();
    }

    public void SelectSofa()
    {
        if (furniturePlacement != null)
            furniturePlacement.ChangeFurniture(1);
        StartAR();
    }

    public void SelectLoker()
    {
        if (furniturePlacement != null)
            furniturePlacement.ChangeFurniture(2);
        StartAR();
    }

    public void BackToHome()
    {
        ShowHome();
        if (furniturePlacement != null)
            furniturePlacement.DeleteFurniture();
    }
}