using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UIBuilder : MonoBehaviour
{
    static readonly Color DARK = new Color(0.07f, 0.07f, 0.07f);
    static readonly Color KREM = new Color(0.973f, 0.969f, 0.957f);
    static readonly Color WHITE = Color.white;
    static readonly Color GRAY_TEXT = new Color(0.53f, 0.53f, 0.53f);
    static readonly Color GRAY_LIGHT = new Color(0.6f, 0.6f, 0.6f);
    static readonly Color BORDER = new Color(0, 0, 0, 0.09f);
    static readonly Color ICON_BG = new Color(0.94f, 0.94f, 0.94f);

    [Header("References")]
    public UIManager uiManager;

    void Start()
    {
       // BuildUI();
    }

    [ContextMenu("Build UI Now")]
    public void BuildUI()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        // ROOT
        GameObject root = NewPanel(canvas.gameObject, "HomePanel", KREM);
        Stretch(root);

        // HEADER
        GameObject header = NewPanel(root, "Header", DARK);
        AnchorTop(header, 360);
        VLayout(header, 6, new RectOffset(50, 50, 80, 0), TextAnchor.UpperCenter, false);
        Label(header, "WELCOME TO", 26, GRAY_TEXT, 36, TextAlignmentOptions.Center);
        Label(header, "AR Furniture", 66, WHITE, 90, TextAlignmentOptions.Center);
        Label(header, "Visualisasi furnitur di ruanganmu", 30, new Color(0.67f, 0.67f, 0.67f), 44, TextAlignmentOptions.Center);

        // CONTENT
        GameObject content = NewPanel(root, "Content", KREM);
        StretchWithMargins(content, 360, 130);
        VLayout(content, 36, new RectOffset(40, 40, 40, 40), TextAnchor.UpperCenter, true);

        // AR FEATURE CARD
        GameObject arCard = NewPanel(content, "ARFeatureCard", WHITE);
        AddBorder(arCard);
        FlexHeight(arCard, 520);
        VLayout(arCard, 0, new RectOffset(0, 0, 0, 0), TextAnchor.UpperCenter, true);

        GameObject preview = NewPanel(arCard, "Preview", new Color(0.1f, 0.1f, 0.1f));
        FlexHeight(preview, 230);
        LabelStretch(preview, "[ 3 MODEL 3D ]", 30, GRAY_LIGHT, TextAlignmentOptions.Center);

        GameObject cardBody = NewPanel(arCard, "CardBody", WHITE);
        FlexHeight(cardBody, 290);
        VLayout(cardBody, 10, new RectOffset(40, 40, 30, 30), TextAnchor.UpperLeft, true);
        Label(cardBody, "FITUR UTAMA", 22, GRAY_TEXT, 32, TextAlignmentOptions.Left);
        Label(cardBody, "Lihat dalam 3D AR", 40, DARK, 56, TextAlignmentOptions.Left);
        Label(cardBody, "Tempatkan furnitur di ruanganmu secara real-time", 24, GRAY_LIGHT, 40, TextAlignmentOptions.Left);
        GameObject startBtn = MakeButton(cardBody, "StartARButton", "Mulai AR  >", DARK, WHITE, 34);
        FlexHeight(startBtn, 80);

        // Auto-sambungkan tombol StartAR ke UIManager
        if (uiManager != null)
        {
            startBtn.GetComponent<Button>().onClick.AddListener(() => uiManager.StartAR());
        }

        // KOLEKSI LABEL
        GameObject lblWrap = NewPanel(content, "KoleksiLabelWrap", new Color(0, 0, 0, 0));
        FlexHeight(lblWrap, 36);
        LabelStretch(lblWrap, "KOLEKSI FURNITUR", 24, GRAY_TEXT, TextAlignmentOptions.Left);

        // GRID FURNITURE
        GameObject grid = NewPanel(content, "FurnitureGrid", new Color(0, 0, 0, 0));
        FlexHeight(grid, 360);
        GridLayoutGroup glg = grid.AddComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(450, 165);
        glg.spacing = new Vector2(20, 20);
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = 2;
        glg.childAlignment = TextAnchor.UpperCenter;

        // Card furniture + auto-sambungkan ke UIManager
        GameObject kasurCard = FurnitureCard(grid, "KasurCard", "Kasur", "Spring bed");
        GameObject sofaCard = FurnitureCard(grid, "SofaCard", "Sofa + Meja", "Set ruang tamu");
        GameObject lokerCard = FurnitureCard(grid, "LokerCard", "Loker", "Metal 6 pintu");

        if (uiManager != null)
        {
            kasurCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectKasur());
            sofaCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectSofa());
            lokerCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectLoker());
        }

        // INFO PRESISI
        GameObject info = NewPanel(content, "InfoCard", WHITE);
        AddBorder(info);
        FlexHeight(info, 110);
        HLayout(info, 16, new RectOffset(30, 30, 0, 0), TextAnchor.MiddleLeft, false);
        GameObject infoIcon = NewPanel(info, "InfoIcon", ICON_BG);
        FixedSize(infoIcon, 60, 60);
        GameObject infoTextWrap = NewPanel(info, "InfoTextWrap", new Color(0, 0, 0, 0));
        FlexWidth(infoTextWrap, 1);
        VLayout(infoTextWrap, 2, new RectOffset(0, 0, 0, 0), TextAnchor.MiddleLeft, false);
        Label(infoTextWrap, "Ukuran presisi", 24, DARK, 32, TextAlignmentOptions.Left);
        Label(infoTextWrap, "Skala 1:1 dengan dunia nyata", 20, GRAY_LIGHT, 28, TextAlignmentOptions.Left);

        // BOTTOM NAV
        GameObject nav = NewPanel(root, "BottomNav", WHITE);
        AnchorBottom(nav, 130);
        AddTopBorder(nav);
        HLayout(nav, 0, new RectOffset(20, 20, 20, 20), TextAnchor.MiddleCenter, true);
        NavButton(nav, "HomeBtn", "Home", DARK);
        NavButton(nav, "ARBtn", "AR", new Color(0.73f, 0.73f, 0.73f));
        NavButton(nav, "SettingsBtn", "Pengaturan", new Color(0.73f, 0.73f, 0.73f));

        // TOMBOL BACK (di luar HomePanel, buat mode AR)
        GameObject backBtn = MakeButton(canvas.gameObject, "BackButton", "← Kembali", WHITE, DARK, 28);
        RectTransform brt = backBtn.GetComponent<RectTransform>();
        brt.anchorMin = new Vector2(0, 1);
        brt.anchorMax = new Vector2(0, 1);
        brt.pivot = new Vector2(0, 1);
        brt.sizeDelta = new Vector2(220, 80);
        brt.anchoredPosition = new Vector2(30, -60);
        backBtn.SetActive(false); // disembunyikan default

        if (uiManager != null)
        {
            uiManager.homePanel = root;
            uiManager.backButton = backBtn;
            backBtn.GetComponent<Button>().onClick.AddListener(() => uiManager.BackToHome());
        }

        Debug.Log("UI Built + tombol tersambung otomatis!");
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

    void Label(GameObject parent, string text, float size, Color color,
        float fixedHeight, TextAlignmentOptions align)
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

    void LabelStretch(GameObject parent, string text, float size, Color color,
        TextAlignmentOptions align)
    {
        GameObject g = new GameObject("Text_" + text, typeof(RectTransform));
        g.transform.SetParent(parent.transform, false);
        TextMeshProUGUI tmp = g.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = align;
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }

    GameObject MakeButton(GameObject parent, string name, string text,
        Color bg, Color textColor, float fontSize)
    {
        GameObject btn = new GameObject(name, typeof(RectTransform));
        btn.transform.SetParent(parent.transform, false);
        Image img = btn.AddComponent<Image>();
        img.color = bg;
        btn.AddComponent<Button>();
        GameObject txt = new GameObject("Text", typeof(RectTransform));
        txt.transform.SetParent(btn.transform, false);
        TextMeshProUGUI tmp = txt.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = textColor;
        tmp.alignment = TextAlignmentOptions.Center;
        RectTransform rt = txt.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        return btn;
    }

    GameObject FurnitureCard(GameObject parent, string name, string title, string subtitle)
    {
        GameObject card = NewPanel(parent, name, WHITE);
        card.AddComponent<Button>();
        AddBorder(card);

        GameObject icon = NewPanel(card, "IconBox", ICON_BG);
        RectTransform irt = icon.GetComponent<RectTransform>();
        irt.anchorMin = new Vector2(0.06f, 0.42f);
        irt.anchorMax = new Vector2(0.28f, 0.85f);
        irt.sizeDelta = Vector2.zero;

        GameObject t = new GameObject("Title", typeof(RectTransform));
        t.transform.SetParent(card.transform, false);
        TextMeshProUGUI tt = t.AddComponent<TextMeshProUGUI>();
        tt.text = title;
        tt.fontSize = 28;
        tt.color = DARK;
        tt.alignment = TextAlignmentOptions.Left;
        RectTransform trt = t.GetComponent<RectTransform>();
        trt.anchorMin = new Vector2(0.06f, 0.24f);
        trt.anchorMax = new Vector2(0.94f, 0.42f);
        trt.sizeDelta = Vector2.zero;

        GameObject s = new GameObject("Subtitle", typeof(RectTransform));
        s.transform.SetParent(card.transform, false);
        TextMeshProUGUI st = s.AddComponent<TextMeshProUGUI>();
        st.text = subtitle;
        st.fontSize = 21;
        st.color = GRAY_LIGHT;
        st.alignment = TextAlignmentOptions.Left;
        RectTransform srt = s.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0.06f, 0.1f);
        srt.anchorMax = new Vector2(0.94f, 0.24f);
        srt.sizeDelta = Vector2.zero;

        return card;
    }

    void NavButton(GameObject parent, string name, string label, Color color)
    {
        GameObject btn = NewPanel(parent, name, new Color(1, 1, 1, 0));
        btn.AddComponent<Button>();
        LayoutElement le = btn.AddComponent<LayoutElement>();
        le.flexibleWidth = 1;
        GameObject txt = new GameObject("Label", typeof(RectTransform));
        txt.transform.SetParent(btn.transform, false);
        TextMeshProUGUI tmp = txt.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 23;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        RectTransform rt = txt.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }

    // ============ LAYOUT HELPERS ============

    void VLayout(GameObject g, float spacing, RectOffset pad, TextAnchor align, bool expandW)
    {
        VerticalLayoutGroup v = g.AddComponent<VerticalLayoutGroup>();
        v.spacing = spacing;
        v.padding = pad;
        v.childAlignment = align;
        v.childControlWidth = true;
        v.childControlHeight = true;
        v.childForceExpandWidth = expandW;
        v.childForceExpandHeight = false;
    }

    void HLayout(GameObject g, float spacing, RectOffset pad, TextAnchor align, bool expandW)
    {
        HorizontalLayoutGroup h = g.AddComponent<HorizontalLayoutGroup>();
        h.spacing = spacing;
        h.padding = pad;
        h.childAlignment = align;
        h.childControlWidth = true;
        h.childControlHeight = true;
        h.childForceExpandWidth = expandW;
        h.childForceExpandHeight = false;
    }

    void FlexHeight(GameObject g, float h)
    {
        LayoutElement le = g.GetComponent<LayoutElement>();
        if (le == null) le = g.AddComponent<LayoutElement>();
        le.preferredHeight = h;
        le.flexibleWidth = 1;
    }

    void FlexWidth(GameObject g, float flex)
    {
        LayoutElement le = g.GetComponent<LayoutElement>();
        if (le == null) le = g.AddComponent<LayoutElement>();
        le.flexibleWidth = flex;
    }

    void FixedSize(GameObject g, float w, float h)
    {
        LayoutElement le = g.GetComponent<LayoutElement>();
        if (le == null) le = g.AddComponent<LayoutElement>();
        le.preferredWidth = w;
        le.preferredHeight = h;
        le.flexibleWidth = 0;
    }

    void AddBorder(GameObject g)
    {
        Outline o = g.AddComponent<Outline>();
        o.effectColor = BORDER;
        o.effectDistance = new Vector2(1.5f, -1.5f);
    }

    void AddTopBorder(GameObject g)
    {
        GameObject line = NewPanel(g, "TopBorder", new Color(0, 0, 0, 0.08f));
        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(0, 1.5f);
        rt.anchoredPosition = Vector2.zero;
        line.transform.SetAsFirstSibling();
    }

    void Stretch(GameObject g)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }

    void AnchorTop(GameObject g, float height)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.sizeDelta = new Vector2(0, height);
        rt.anchoredPosition = Vector2.zero;
    }

    void AnchorBottom(GameObject g, float height)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(0, height);
        rt.anchoredPosition = Vector2.zero;
    }

    void StretchWithMargins(GameObject g, float top, float bottom)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(0, bottom);
        rt.offsetMax = new Vector2(0, -top);
    }
}