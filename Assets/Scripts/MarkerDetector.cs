using UnityEngine;
using Vuforia;

public class MarkerDetector : MonoBehaviour
{
    [Header("UI Panel yang mau disembunyikan saat marker kedeteksi")]
    public GameObject instructionPanel;

    [Header("Image Targets yang dipantau")]
    public ObserverBehaviour[] imageTargets;

    void Start()
    {
        // Subscribe ke event tiap image target
        foreach (var target in imageTargets)
        {
            if (target != null)
                target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Cek apakah ADA marker yang tracking
        bool anyTracking = false;
        foreach (var target in imageTargets)
        {
            if (target != null)
            {
                var s = target.TargetStatus.Status;
                if (s == Status.TRACKED || s == Status.EXTENDED_TRACKED)
                {
                    anyTracking = true;
                    break;
                }
            }
        }

        // Show/hide instruction panel
        if (instructionPanel != null)
            instructionPanel.SetActive(!anyTracking);
    }

    void OnDestroy()
    {
        foreach (var target in imageTargets)
        {
            if (target != null)
                target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
