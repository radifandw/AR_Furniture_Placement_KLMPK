using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GoToMarkerless()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMarker()
    {
        SceneManager.LoadScene("SceneMarker");
    }
}