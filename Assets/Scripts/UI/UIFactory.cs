using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ArenaDuel.UI
{
    public static class UIFactory
    {
        public static readonly Color Ink = new Color(0.035f, 0.045f, 0.07f, 0.96f);
        public static readonly Color Panel = new Color(0.08f, 0.09f, 0.13f, 0.94f);
        public static readonly Color Ember = new Color(1f, 0.28f, 0.055f, 1f);
        public static readonly Color Gold = new Color(0.88f, 0.62f, 0.25f, 1f);
        public static Font Font => Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        public static Canvas CreateCanvas(string name = "Canvas")
        {
            GameObject go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080); scaler.matchWidthOrHeight = 0.5f;
            if (!Object.FindFirstObjectByType<EventSystem>()) new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            return canvas;
        }

        public static RectTransform PanelObject(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false); Image image = go.GetComponent<Image>(); image.color = color;
            RectTransform rt = go.GetComponent<RectTransform>(); rt.anchorMin = anchorMin; rt.anchorMax = anchorMax; rt.offsetMin = offsetMin; rt.offsetMax = offsetMax;
            return rt;
        }

        public static Text Text(Transform parent, string text, int size, TextAnchor alignment, Color color)
        {
            GameObject go = new GameObject("Text", typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
            Text label = go.GetComponent<Text>(); label.text = text; label.font = Font; label.fontSize = size; label.alignment = alignment; label.color = color;
            label.horizontalOverflow = HorizontalWrapMode.Wrap; label.verticalOverflow = VerticalWrapMode.Overflow;
            RectTransform rt = label.rectTransform; rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = new Vector2(18, 8); rt.offsetMax = new Vector2(-18, -8);
            return label;
        }

        public static Button Button(Transform parent, string label, UnityAction action, Color? color = null)
        {
            GameObject go = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button)); go.transform.SetParent(parent, false);
            Image image = go.GetComponent<Image>(); image.color = color ?? Ember;
            Button button = go.GetComponent<Button>(); button.targetGraphic = image; button.onClick.AddListener(action);
            ColorBlock cb = button.colors; cb.highlightedColor = Color.Lerp(image.color, Color.white, 0.18f); cb.pressedColor = Color.Lerp(image.color, Color.black, 0.25f); button.colors = cb;
            Text(go.transform, label, 28, TextAnchor.MiddleCenter, Color.white);
            return button;
        }

        public static void Stretch(RectTransform rt) { rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero; }
    }
}
