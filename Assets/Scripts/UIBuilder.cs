using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    static readonly Color BG_WARM = new Color(0.965f, 0.953f, 0.933f);
    static readonly Color DARK_BROWN = new Color(0.22f, 0.15f, 0.09f);
    static readonly Color MED_BROWN = new Color(0.48f, 0.33f, 0.18f);
    static readonly Color LIGHT_BROWN = new Color(0.72f, 0.56f, 0.38f);
    static readonly Color ACCENT = new Color(0.56f, 0.42f, 0.24f);
    static readonly Color WHITE = new Color(1f, 0.99f, 0.97f);
    static readonly Color CARD_BG = new Color(1f, 0.98f, 0.95f);
    static readonly Color THUMB_BG = new Color(0.90f, 0.84f, 0.75f);
    static readonly Color TEXT_MUTED = new Color(0.55f, 0.45f, 0.35f);
    static readonly Color TEXT_LIGHT = new Color(0.70f, 0.60f, 0.50f);
    static readonly Color BORDER = new Color(0.72f, 0.56f, 0.38f, 0.2f);
    static readonly Color NAV_BG = new Color(0.22f, 0.15f, 0.09f);
    static readonly Color NAV_ACTIVE = new Color(0.90f, 0.75f, 0.50f);
    static readonly Color NAV_INACTIVE = new Color(0.55f, 0.45f, 0.35f);

    [Header("References")]
    public UIManager uiManager;

    [Header("Foto Furniture")]
    public Sprite fotoKasur;
    public Sprite fotoSofa;
    public Sprite fotoLoker;

    void Start() { }

    [ContextMenu("Build UI Now")]
    public void BuildUI()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        // ROOT
        GameObject root = NewPanel(canvas.gameObject, "HomePanel", BG_WARM);
        Stretch(root);

        // ── HEADER ──────────────────────────────────────────────
        GameObject header = NewPanel(root, "Header", DARK_BROWN);
        AnchorTop(header, 280);
        VLayout(header, 6, new RectOffset(50, 50, 55, 0), TextAnchor.UpperCenter, false);
        Label(header, "TOKO MEUBEL MUTIARA", 22, LIGHT_BROWN, 32, TextAlignmentOptions.Center);
        Label(header, "AR Furniture", 56, WHITE, 72, TextAlignmentOptions.Center);
        Label(header, "Visualisasi furnitur di ruanganmu", 22, TEXT_LIGHT, 36, TextAlignmentOptions.Center);
        GameObject headerLine = NewPanel(header, "HeaderLine", ACCENT);
        LayoutElement hl = headerLine.AddComponent<LayoutElement>();
        hl.preferredHeight = 3;
        hl.preferredWidth = 100;

        // ── CONTENT ─────────────────────────────────────────────
        GameObject content = NewPanel(root, "Content", BG_WARM);
        StretchWithMargins(content, 280, 110);
        VLayout(content, 22, new RectOffset(36, 36, 36, 36), TextAnchor.UpperCenter, true);

        // ── AR FEATURE CARD ─────────────────────────────────────
        GameObject arCard = NewPanel(content, "ARFeatureCard", CARD_BG);
        AddRoundBorder(arCard);
        FlexHeight(arCard, 420);
        VLayout(arCard, 0, new RectOffset(0, 0, 0, 0), TextAnchor.UpperCenter, true);

        // Preview SLIDESHOW area
        GameObject preview = NewPanel(arCard, "Preview", MED_BROWN);
        FlexHeight(preview, 200);
        // SlideShow script akan di-attach ke Preview ini
        SlideShow ss = preview.AddComponent<SlideShow>();
        ss.interval = 2.5f;
        if (fotoKasur != null && fotoSofa != null && fotoLoker != null)
            ss.sprites = new Sprite[] { fotoKasur, fotoSofa, fotoLoker };
        // Set image ke full stretch
        preview.GetComponent<Image>().preserveAspect = false;

        // Overlay label di bawah foto
        GameObject previewLabel = NewPanel(preview, "PreviewLabel", new Color(0, 0, 0, 0.35f));
        RectTransform plrt = previewLabel.GetComponent<RectTransform>();
        plrt.anchorMin = new Vector2(0, 0);
        plrt.anchorMax = new Vector2(1, 0);
        plrt.pivot = new Vector2(0.5f, 0);
        plrt.sizeDelta = new Vector2(0, 40);
        plrt.anchoredPosition = Vector2.zero;
        LabelStretch(previewLabel, "3 MODEL 3D", 20, NAV_ACTIVE, TextAlignmentOptions.Center);

        // Card body
        GameObject cardBody = NewPanel(arCard, "CardBody", CARD_BG);
        FlexHeight(cardBody, 220);
        VLayout(cardBody, 10, new RectOffset(36, 36, 20, 20), TextAnchor.UpperLeft, true);
        Label(cardBody, "FITUR UTAMA", 20, TEXT_MUTED, 28, TextAlignmentOptions.Left);
        Label(cardBody, "Lihat dalam 3D AR", 34, DARK_BROWN, 48, TextAlignmentOptions.Left);
        Label(cardBody, "Tempatkan furnitur di ruanganmu secara real-time", 20, TEXT_LIGHT, 34, TextAlignmentOptions.Left);
        GameObject startBtn = MakeButton(cardBody, "StartARButton", "Mulai AR  →", DARK_BROWN, NAV_ACTIVE, 27);
        FlexHeight(startBtn, 64);
        if (uiManager != null)
            startBtn.GetComponent<Button>().onClick.AddListener(() => uiManager.StartAR());

        // ── KOLEKSI LABEL ───────────────────────────────────────
        GameObject lblWrap = NewPanel(content, "KoleksiLabelWrap", new Color(0, 0, 0, 0));
        FlexHeight(lblWrap, 36);
        Label(lblWrap, "KOLEKSI FURNITUR", 22, ACCENT, 32, TextAlignmentOptions.Left);

        // ── FURNITURE CARDS ─────────────────────────────────────
        GameObject kasurCard = FurnitureCard(content, "KasurCard", "Kasur", "Spring bed • Ukuran Queen", fotoKasur);
        GameObject sofaCard = FurnitureCard(content, "SofaCard", "Sofa + Meja", "Set ruang tamu modern", fotoSofa);
        GameObject lokerCard = FurnitureCard(content, "LokerCard", "Loker", "Metal 6 pintu • Abu-abu", fotoLoker);

        if (uiManager != null)
        {
            kasurCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectKasur());
            sofaCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectSofa());
            lokerCard.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectLoker());
        }

        // ── BOTTOM NAV ──────────────────────────────────────────
        GameObject nav = NewPanel(root, "BottomNav", NAV_BG);
        AnchorBottom(nav, 110);
        HLayout(nav, 0, new RectOffset(16, 16, 16, 16), TextAnchor.MiddleCenter, true);
        NavButton(nav, "HomeBtn", "Home", NAV_ACTIVE);
        NavButton(nav, "ScanBtn", "Scan", NAV_INACTIVE);

        // ── BACK BUTTON ─────────────────────────────────────────
        GameObject backBtn = MakeButton(canvas.gameObject, "BackButton", "← Kembali", CARD_BG, DARK_BROWN, 26);
        RectTransform brt = backBtn.GetComponent<RectTransform>();
        brt.anchorMin = new Vector2(0, 1);
        brt.anchorMax = new Vector2(0, 1);
        brt.pivot = new Vector2(0, 1);
        brt.sizeDelta = new Vector2(200, 72);
        brt.anchoredPosition = new Vector2(30, -60);
        backBtn.SetActive(false);

        if (uiManager != null)
        {
            uiManager.homePanel = root;
            uiManager.backButton = backBtn;
            backBtn.GetComponent<Button>().onClick.AddListener(() => uiManager.BackToHome());
        }

        Debug.Log("UI Built dengan Slideshow!");
    }

    // ============ FURNITURE CARD ============
    GameObject FurnitureCard(GameObject parent, string name, string title, string subtitle, Sprite foto)
    {
        GameObject card = NewPanel(parent, name, CARD_BG);
        card.AddComponent<Button>();
        AddRoundBorder(card);
        FlexHeight(card, 125);
        HLayout(card, 16, new RectOffset(16, 16, 0, 0), TextAnchor.MiddleLeft, false);

        GameObject thumb = NewPanel(card, "Thumbnail", THUMB_BG);
        FixedSize(thumb, 90, 90);
        if (foto != null)
        {
            thumb.GetComponent<Image>().sprite = foto;
            thumb.GetComponent<Image>().color = Color.white;
            thumb.GetComponent<Image>().preserveAspect = true;
        }

        GameObject textWrap = NewPanel(card, "TextWrap", new Color(0, 0, 0, 0));
        FlexWidth(textWrap, 1);
        VLayout(textWrap, 5, new RectOffset(0, 0, 0, 0), TextAnchor.MiddleLeft, false);
        Label(textWrap, title, 27, DARK_BROWN, 36, TextAlignmentOptions.Left);
        Label(textWrap, subtitle, 18, TEXT_MUTED, 25, TextAlignmentOptions.Left);

        GameObject arrow = NewPanel(card, "Arrow", new Color(0, 0, 0, 0));
        FixedSize(arrow, 32, 32);
        LabelStretch(arrow, ">", 32, LIGHT_BROWN, TextAlignmentOptions.Center);

        return card;
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

    void LabelStretch(GameObject parent, string text, float size, Color color, TextAlignmentOptions align)
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

    GameObject MakeButton(GameObject parent, string name, string text, Color bg, Color textColor, float fontSize)
    {
        GameObject btn = new GameObject(name, typeof(RectTransform));
        btn.transform.SetParent(parent.transform, false);
        btn.AddComponent<Image>().color = bg;
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
        tmp.fontSize = 22;
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

    void FlexHeight(GameObject g, float h)
    {
        LayoutElement le = g.GetComponent<LayoutElement>() ?? g.AddComponent<LayoutElement>();
        le.preferredHeight = h; le.flexibleWidth = 1;
    }

    void FlexWidth(GameObject g, float flex)
    {
        LayoutElement le = g.GetComponent<LayoutElement>() ?? g.AddComponent<LayoutElement>();
        le.flexibleWidth = flex;
    }

    void FixedSize(GameObject g, float w, float h)
    {
        LayoutElement le = g.GetComponent<LayoutElement>() ?? g.AddComponent<LayoutElement>();
        le.preferredWidth = w; le.preferredHeight = h; le.flexibleWidth = 0;
    }

    void AddRoundBorder(GameObject g)
    {
        Outline o = g.AddComponent<Outline>();
        o.effectColor = BORDER;
        o.effectDistance = new Vector2(1.5f, -1.5f);
    }

    void Stretch(GameObject g)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero; rt.anchoredPosition = Vector2.zero;
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

    void StretchWithMargins(GameObject g, float top, float bottom)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(0, bottom); rt.offsetMax = new Vector2(0, -top);
    }
}