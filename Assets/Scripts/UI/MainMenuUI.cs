using ArenaDuel.Core;
using ArenaDuel.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ArenaDuel.UI
{
    public sealed class MainMenuUI : MonoBehaviour
    {
        public HeroDefinition ember;
        GameObject modal;

        void Start()
        {
            CreateShowcase();
            Canvas canvas = UIFactory.CreateCanvas("MainMenuCanvas");
            RectTransform shade = UIFactory.PanelObject(canvas.transform, "Backdrop", new Color(0.01f, 0.015f, 0.03f, 0.45f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            shade.SetAsFirstSibling();

            RectTransform top = UIFactory.PanelObject(canvas.transform, "TopBar", UIFactory.Ink, new Vector2(0, .88f), Vector2.one, Vector2.zero, Vector2.zero);
            UIFactory.Text(top, "  ARENA DUEL     PLAYER 01  •  LEVEL 1", 26, TextAnchor.MiddleLeft, Color.white);
            Button settings = UIFactory.Button(top, "SETTINGS", ShowSettings, new Color(.18f, .2f, .28f));
            RectTransform sr = settings.GetComponent<RectTransform>(); sr.anchorMin = new Vector2(.84f, .18f); sr.anchorMax = new Vector2(.98f, .82f); sr.offsetMin = sr.offsetMax = Vector2.zero;

            RectTransform heroCard = UIFactory.PanelObject(canvas.transform, "HeroCard", new Color(.05f, .04f, .05f, .65f), new Vector2(.06f, .13f), new Vector2(.55f, .82f), Vector2.zero, Vector2.zero);
            Text heroText = UIFactory.Text(heroCard, "EMBER\n\nTHE FLAMEBOUND DUELIST", 42, TextAnchor.LowerLeft, Color.white); heroText.fontStyle = FontStyle.Bold;

            RectTransform actions = UIFactory.PanelObject(canvas.transform, "Actions", UIFactory.Panel, new Vector2(.68f, .16f), new Vector2(.95f, .82f), Vector2.zero, Vector2.zero);
            AddAction(actions, "BATTLE", .72f, () => SceneManager.LoadScene("Arena"), UIFactory.Ember);
            AddAction(actions, "HEROES", .50f, () => SceneManager.LoadScene("HeroSelect"), UIFactory.Gold);
            AddAction(actions, "OPEN WORLD", .28f, ShowComingSoon, new Color(.18f, .2f, .28f));
            AddAction(actions, "SETTINGS", .06f, ShowSettings, new Color(.18f, .2f, .28f));
        }

        void AddAction(RectTransform parent, string label, float y, UnityEngine.Events.UnityAction action, Color color)
        {
            Button b = UIFactory.Button(parent, label, action, color); RectTransform r = b.GetComponent<RectTransform>();
            r.anchorMin = new Vector2(.08f, y); r.anchorMax = new Vector2(.92f, y + .16f); r.offsetMin = r.offsetMax = Vector2.zero;
        }

        void CreateShowcase()
        {
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            if (!cam) { cam = new GameObject("Main Camera", typeof(UnityEngine.Camera), typeof(AudioListener)).GetComponent<UnityEngine.Camera>(); cam.tag = "MainCamera"; }
            cam.transform.SetPositionAndRotation(new Vector3(0, 1.25f, -5f), Quaternion.identity); cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.025f, .018f, .025f);
            GameObject model = ember && ember.characterPrefab ? Instantiate(ember.characterPrefab, new Vector3(-.65f, 0, 0), Quaternion.Euler(0, 165f, 0)) : GameObject.CreatePrimitive(PrimitiveType.Capsule);
            model.name = "EmberShowcase"; model.AddComponent<ShowcaseSpinner>();
            Light key = new GameObject("ShowcaseKey", typeof(Light)).GetComponent<Light>(); key.type = LightType.Directional; key.intensity = 2.1f; key.color = new Color(1f, .62f, .42f); key.transform.rotation = Quaternion.Euler(35, -35, 0);
        }

        void ShowComingSoon() => ShowModal("OPEN WORLD", "COMING SOON\n\nA larger world with exploration, enemies, quests, and future progression is planned for a later update.");

        void ShowSettings()
        {
            ShowModal("SETTINGS", "QUALITY: " + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\n\nTap CHANGE QUALITY to cycle Low, Medium, High and Ultra.");
            Button quality = UIFactory.Button(modal.transform, "CHANGE QUALITY", () => { SaveService.Data.qualityLevel = (SaveService.Data.qualityLevel + 1) % Mathf.Max(1, QualitySettings.names.Length); SaveService.Save(); HideModal(); ShowSettings(); }, UIFactory.Gold);
            RectTransform r = quality.GetComponent<RectTransform>(); r.anchorMin = new Vector2(.2f, .16f); r.anchorMax = new Vector2(.8f, .28f); r.offsetMin = r.offsetMax = Vector2.zero;
        }

        void ShowModal(string title, string body)
        {
            if (modal) Destroy(modal);
            modal = UIFactory.PanelObject(FindFirstObjectByType<Canvas>().transform, "Modal", new Color(0, 0, 0, .8f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero).gameObject;
            RectTransform card = UIFactory.PanelObject(modal.transform, "Card", UIFactory.Panel, new Vector2(.28f, .23f), new Vector2(.72f, .77f), Vector2.zero, Vector2.zero);
            Text text = UIFactory.Text(card, title + "\n\n" + body, 31, TextAnchor.UpperCenter, Color.white); text.fontStyle = FontStyle.Bold;
            Button close = UIFactory.Button(card, "CLOSE", HideModal, UIFactory.Ember); RectTransform r = close.GetComponent<RectTransform>(); r.anchorMin = new Vector2(.3f, .04f); r.anchorMax = new Vector2(.7f, .16f); r.offsetMin = r.offsetMax = Vector2.zero;
        }

        void HideModal() { if (modal) Destroy(modal); }
    }

    public sealed class ShowcaseSpinner : MonoBehaviour { void Update() => transform.Rotate(0, 10f * Time.deltaTime, 0, Space.World); }
}
