using ArenaDuel.Core;
using ArenaDuel.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ArenaDuel.UI
{
    public sealed class HeroSelectUI : MonoBehaviour
    {
        public HeroDefinition ember;
        void Start()
        {
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            cam.transform.SetPositionAndRotation(new Vector3(0, 1.15f, -4.5f), Quaternion.identity); cam.backgroundColor = new Color(.018f, .02f, .035f); cam.clearFlags = CameraClearFlags.SolidColor;
            if (ember && ember.characterPrefab) Instantiate(ember.characterPrefab, new Vector3(-1.4f, 0, 0), Quaternion.Euler(0, 165, 0)).AddComponent<ShowcaseSpinner>();
            Light light = new GameObject("HeroLight", typeof(Light)).GetComponent<Light>(); light.type = LightType.Directional; light.intensity = 2f; light.transform.rotation = Quaternion.Euler(35, -30, 0);

            Canvas canvas = UIFactory.CreateCanvas("HeroSelectCanvas");
            RectTransform card = UIFactory.PanelObject(canvas.transform, "Details", UIFactory.Panel, new Vector2(.56f, .08f), new Vector2(.95f, .92f), Vector2.zero, Vector2.zero);
            string stats = ember ? $"EMBER\n\n{ember.description}\n\nHP  {ember.maxHealth:0}\nATTACK  {ember.attack:0}\nDEFENSE  {ember.defense:0}\nSPEED  {ember.movementSpeed:0.0}\n\nBASIC  {ember.basicAttack.displayName}\nSKILL 1  {ember.skill1.displayName}\nSKILL 2  {ember.skill2.displayName}\nULTIMATE  {ember.ultimate.displayName}" : "EMBER";
            UIFactory.Text(card, stats, 28, TextAnchor.UpperLeft, Color.white);
            Button select = UIFactory.Button(card, "SELECT EMBER", Select, UIFactory.Ember); RectTransform sr = select.GetComponent<RectTransform>(); sr.anchorMin = new Vector2(.08f, .07f); sr.anchorMax = new Vector2(.92f, .17f); sr.offsetMin = sr.offsetMax = Vector2.zero;
            Button back = UIFactory.Button(canvas.transform, "BACK", () => SceneManager.LoadScene("MainMenu"), new Color(.15f, .17f, .23f)); RectTransform br = back.GetComponent<RectTransform>(); br.anchorMin = new Vector2(.03f, .85f); br.anchorMax = new Vector2(.14f, .94f); br.offsetMin = br.offsetMax = Vector2.zero;
        }

        void Select() { SaveService.Data.selectedHero = "ember"; SaveService.Save(); SceneManager.LoadScene("MainMenu"); }
    }
}
