using ArenaDuel.AI;
using ArenaDuel.CameraSystem;
using ArenaDuel.Characters;
using ArenaDuel.Data;
using ArenaDuel.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArenaDuel.Core
{
    public sealed class ArenaMatchController : MonoBehaviour
    {
        public HeroDefinition ember;
        public GameObject fighterPrefab;
        public float matchDuration = 120f;
        public FighterController Player { get; private set; }
        public FighterController Enemy { get; private set; }
        public float RemainingTime { get; private set; }
        public bool MatchEnded { get; private set; }
        BattleHUD hud;

        void Start()
        {
            BuildArena();
            BuildFighters();
            RemainingTime = matchDuration;
            hud = gameObject.AddComponent<BattleHUD>();
            hud.Configure(this, Player, Enemy);
        }

        void Update()
        {
            if (MatchEnded) return;
            RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
            if (RemainingTime <= 0f)
            {
                if (Player.Health.Current >= Enemy.Health.Current) EndMatch(true, "TIME");
                else EndMatch(false, "TIME");
            }
        }

        void BuildFighters()
        {
            Player = Spawn("Player Ember", new Vector3(-3.5f, .05f, 0), Quaternion.Euler(0, 90, 0));
            Enemy = Spawn("Enemy Ember", new Vector3(3.5f, .05f, 0), Quaternion.Euler(0, -90, 0));
            PlayerInputController playerInput = Player.gameObject.AddComponent<PlayerInputController>(); Player.SetInputSource(playerInput);
            ArenaAIController ai = Enemy.gameObject.AddComponent<ArenaAIController>(); ai.owner = Enemy; ai.target = Player; Enemy.SetInputSource(ai);
            Player.Health.Died += _ => EndMatch(false, "KNOCKOUT");
            Enemy.Health.Died += _ => EndMatch(true, "KNOCKOUT");

            UnityEngine.Camera cam = UnityEngine.Camera.main;
            BattleCameraController battleCamera = cam.gameObject.AddComponent<BattleCameraController>(); battleCamera.target = Player.transform;
            Player.SetCamera(cam.transform); Enemy.SetCamera(cam.transform);
        }

        FighterController Spawn(string objectName, Vector3 position, Quaternion rotation)
        {
            GameObject go = Instantiate(fighterPrefab, position, rotation); go.name = objectName;
            FighterController fighter = go.GetComponent<FighterController>(); fighter.Configure(ember);
            return fighter;
        }

        void BuildArena()
        {
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            cam.transform.SetPositionAndRotation(new Vector3(-7f, 6.5f, -8f), Quaternion.Euler(24, 40, 0));
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.025f, .02f, .035f); cam.fieldOfView = 55f;

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cylinder); floor.name = "ArenaFloor"; floor.transform.localScale = new Vector3(8.5f, .18f, 8.5f); floor.transform.position = new Vector3(0, -.2f, 0);
            floor.GetComponent<Renderer>().material.color = new Color(.11f, .105f, .13f);
            for (int i = 0; i < 16; i++)
            {
                float a = i * Mathf.PI * 2f / 16f;
                GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube); pillar.name = "BoundaryPillar";
                pillar.transform.position = new Vector3(Mathf.Cos(a) * 13f, 1.25f, Mathf.Sin(a) * 13f);
                pillar.transform.localScale = new Vector3(.45f, 2.5f, .45f); pillar.GetComponent<Renderer>().material.color = new Color(.16f, .12f, .12f);
            }
            Light sun = new GameObject("ArenaSun", typeof(Light)).GetComponent<Light>(); sun.type = LightType.Directional; sun.intensity = 1.6f; sun.color = new Color(1f, .7f, .55f); sun.shadows = LightShadows.Soft; sun.transform.rotation = Quaternion.Euler(48, -30, 0);
            RenderSettings.ambientLight = new Color(.18f, .18f, .25f);
        }

        void EndMatch(bool playerWon, string reason)
        {
            if (MatchEnded) return;
            MatchEnded = true;
            Player.enabled = false; Enemy.enabled = false;
            hud.ShowResult(playerWon ? "VICTORY" : "DEFEAT", reason,
                () => SceneManager.LoadScene("Arena"), () => SceneManager.LoadScene("MainMenu"));
        }
    }
}
