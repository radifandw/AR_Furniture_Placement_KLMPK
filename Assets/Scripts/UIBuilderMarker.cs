using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIBuilderMarker : MonoBehaviour
{
    // ============ EARTHY COLOR PALETTE ============
    static readonly Color BG_WARM = new Color(0.965f, 0.953f, 0.933f);
    static readonly Color DARK_BROWN = new Color(0.22f, 0.15f, 0.09f);
    static readonly Color LIGHT_BROWN = new Color(0.72f, 0.56f, 0.38f);
    static readonly Color ACCENT = new Color(0.56f, 0.42f, 0.24f);
    static readonly Color WHITE = new Color(1f, 0.99f, 0.97f);
    static readonly Color CARD_BG = new Color(1f, 0.98f, 0.95f);
    static readonly Color TEXT_MUTED = new Color(0.55f, 0.45f, 0.35f);
    static readonly Color TEXT_LIGHT = new Color(0.70f, 0.60f, 0.50f);
    static readonly Color NAV_BG = new Color(0.22f, 0.15f, 0.09f);
    static readonly Color NAV_ACTIVE = new Color(0.90f, 0.75f, 0.50f);
    static readonly Color NAV_INACTIVE = new Color(0.55f, 0.45f, 0.35f);

    [ContextMenu("Build Marker UI Now")]
    public void BuildUI()
    {
        // ── BUAT CANVAS ─────────────────────────────────────────
        Canvas existing = FindAnyObjectByType<Canvas>();
        GameObject canvasGO;

        if (existing != null)
        {
            canvasGO = existing.gameObject;
        }
        else
        {
            canvasGO = new GameObject("Canvas", typeof(RectTransform));
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // di atas AR
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 2400);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Pastikan EventSystem ada
        if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // ── HEADER ──────────────────────────────────────────────
        GameObject header = NewPanel(canvasGO, "Header", DARK_BROWN);
        AnchorTop(header, 220);
        VLayout(header, 6, new RectOffset(50, 50, 50, 0), TextAnchor.UpperCenter, false);
        Label(header, "TOKO MEUBEL MUTIARA", 22, LIGHT_BROWN, 32, TextAlignmentOptions.Center);
        Label(header, "Mode Marker", 52, WHITE, 70, TextAlignmentOptions.Center);
        Label(header, "Scan marker untuk lihat furniture", 22, TEXT_LIGHT, 34, TextAlignmentOptions.Center);

        // Garis dekorasi
        GameObject headerLine = NewPanel(header, "HeaderLine", ACCENT);
        LayoutElement hl = headerLine.AddComponent<LayoutElement>();
        hl.preferredHeight = 3;
        hl.preferredWidth = 100;

        // ── INSTRUKSI CARD (tengah, semi-transparan) ────────────
        GameObject instructionWrap = NewPanel(canvasGO, "InstructionWrap", new Color(0, 0, 0, 0));
        RectTransform iwRT = instructionWrap.GetComponent<RectTransform>();
        iwRT.anchorMin = new Vector2(0.5f, 0.5f);
        iwRT.anchorMax = new Vector2(0.5f, 0.5f);
        iwRT.pivot = new Vector2(0.5f, 0.5f);
        iwRT.sizeDelta = new Vector2(900, 280);
        iwRT.anchoredPosition = new Vector2(0, 200);

        // Background card
        instructionWrap.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        VLayout(instructionWrap, 14, new RectOffset(40, 40, 40, 40), TextAnchor.MiddleCenter, true);

        Label(instructionWrap, "⬡", 70, NAV_ACTIVE, 90, TextAlignmentOptions.Center);
        Label(instructionWrap, "Arahkan kamera ke marker", 32, WHITE, 44, TextAlignmentOptions.Center);
        Label(instructionWrap, "Pastikan marker tampak jelas dan tidak terhalang", 20, new Color(0.85f, 0.85f, 0.85f), 30, TextAlignmentOptions.Center);

        // ── BOTTOM NAV ──────────────────────────────────────────
        GameObject nav = NewPanel(canvasGO, "BottomNav", NAV_BG);
        AnchorBottom(nav, 110);
        HLayout(nav, 0, new RectOffset(16, 16, 16, 16), TextAnchor.MiddleCenter, true);
        GameObject homeBtn = NavButton(nav, "HomeBtn", "Home", NAV_INACTIVE);
        GameObject scanBtn = NavButton(nav, "ScanBtn", "Scan", NAV_ACTIVE);

        // Sambungkan HomeBtn → balik ke SampleScene
        homeBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });

        Debug.Log("Marker UI Built!");
    }

    // ============ BUILDERS ============
    GameObject NewPanel(GameObject parent, string name, Color color)
    {
        GameObject g = new GameObject(name, typeof(RectTransform));
        g.transform.SetParent(parent.transform, false);
        Image img = g.AddComponent<Image>();
        img.color = color;
        return g;
    }

    void Label(GameObject parent, string text, float size, Color color, float fixedHeight, TextAlignmentOptions align)
    {
        GameObject g = new GameObject("Text_" + text, typeof(RectTransform));
        g.transform.SetParent(parent.transform, false);
        TextMeshProUGUI tmp = g.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = align;
        tmp.textWrappingMode = TMPro.TextWrappingModes.Normal;
        LayoutElement le = g.AddComponent<LayoutElement>();
        le.preferredHeight = fixedHeight;
    }

    GameObject NavButton(GameObject parent, string name, string label, Color color)
    {
        GameObject btn = NewPanel(parent, name, new Color(1, 1, 1, 0));
        btn.AddComponent<Button>();
        LayoutElement le = btn.AddComponent<LayoutElement>();
        le.flexibleWidth = 1;
        GameObject txt = new GameObject("Label", typeof(RectTransform));
        txt.transform.SetParent(btn.transform, false);
        TextMeshProUGUI tmp = txt.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 22;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        RectTransform rt = txt.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        return btn;
    }

    // ============ LAYOUT HELPERS ============
    void VLayout(GameObject g, float spacing, RectOffset pad, TextAnchor align, bool expandW)
    {
        VerticalLayoutGroup v = g.AddComponent<VerticalLayoutGroup>();
        v.spacing = spacing; v.padding = pad; v.childAlignment = align;
        v.childControlWidth = true; v.childControlHeight = true;
        v.childForceExpandWidth = expandW; v.childForceExpandHeight = false;
    }

    void HLayout(GameObject g, float spacing, RectOffset pad, TextAnchor align, bool expandW)
    {
        HorizontalLayoutGroup h = g.AddComponent<HorizontalLayoutGroup>();
        h.spacing = spacing; h.padding = pad; h.childAlignment = align;
        h.childControlWidth = true; h.childControlHeight = true;
        h.childForceExpandWidth = expandW; h.childForceExpandHeight = false;
    }

    void AnchorTop(GameObject g, float height)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1); rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(0, height); rt.anchoredPosition = Vector2.zero;
    }

    void AnchorBottom(GameObject g, float height)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0); rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(0, height); rt.anchoredPosition = Vector2.zero;
    }
}