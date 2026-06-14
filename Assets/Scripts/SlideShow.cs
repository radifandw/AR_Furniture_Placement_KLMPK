using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideShow : MonoBehaviour
{
    [Header("Foto Furniture")]
    public Sprite[] sprites;

    [Header("Settings")]
    public float interval = 2.5f;

    Image img;
    int current = 0;

    void Start()
    {
        img = GetComponent<Image>();
        if (sprites == null || sprites.Length == 0) return;
        img.sprite = sprites[0];
        img.preserveAspect = true;
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            current = (current + 1) % sprites.Length;
            if (img != null && sprites[current] != null)
                img.sprite = sprites[current];
        }
    }
}