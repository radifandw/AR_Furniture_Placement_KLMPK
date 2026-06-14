using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject homePanel;
    public GameObject backButton;

    [Header("AR Reference")]
    public FurniturePlacement furniturePlacement;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip voiceKasur;
    public AudioClip voiceSofa;
    public AudioClip voiceLoker;

    void Start()
    {
        ShowHome();
    }

    public void ShowHome()
    {
        if (homePanel != null) homePanel.SetActive(true);
        if (backButton != null) backButton.SetActive(false);
        StopVoice();
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
        PlayVoice(voiceKasur);
    }

    public void SelectSofa()
    {
        if (furniturePlacement != null)
            furniturePlacement.ChangeFurniture(1);
        StartAR();
        PlayVoice(voiceSofa);
    }

    public void SelectLoker()
    {
        if (furniturePlacement != null)
            furniturePlacement.ChangeFurniture(2);
        StartAR();
        PlayVoice(voiceLoker);
    }

    public void BackToHome()
    {
        ShowHome();
        if (furniturePlacement != null)
            furniturePlacement.DeleteFurniture();
    }

    void PlayVoice(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

    void StopVoice()
    {
        if (audioSource != null) audioSource.Stop();
    }
}