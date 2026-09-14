using System;
using ArenaDuel.Characters;
using ArenaDuel.Core;
using ArenaDuel.Data;
using ArenaDuel.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaDuel.UI
{
    public sealed class BattleHUD : MonoBehaviour
    {
        ArenaMatchController match;
        FighterController player, enemy;
        Image playerFill, enemyFill;
        Text timer;
        Text comboText;
        Text[] cooldownLabels = new Text[4];
        SkillDefinition[] skills;
        GameObject resultOverlay;

        public void Configure(ArenaMatchController controller, FighterController playerFighter, FighterController enemyFighter)
        {
            match = controller; player = playerFighter; enemy = enemyFighter;
            new GameObject("MobileInput", typeof(MobileInputController));
            Canvas canvas = UIFactory.CreateCanvas("BattleHUD");
            CreateTop(canvas.transform);
            CreateJoystick(canvas.transform);
            skills = new[] { player.hero.basicAttack, player.hero.skill1, player.hero.skill2, player.hero.ultimate };
            CreateActions(canvas.transform);
            RectTransform combo = UIFactory.PanelObject(canvas.transform, "Combo", new Color(0,0,0,.35f), new Vector2(.4f,.65f), new Vector2(.6f,.74f), Vector2.zero, Vector2.zero);
            comboText = UIFactory.Text(combo, "", 30, TextAnchor.MiddleCenter, UIFactory.Gold);
            ComboSystem tracker = player.GetComponent<ComboSystem>(); if (tracker) tracker.Changed += count => comboText.text = count > 1 ? count + " HIT COMBO" : "";
        }

        void CreateTop(Transform parent)
        {
            RectTransform top = UIFactory.PanelObject(parent, "MatchTop", new Color(.025f, .03f, .05f, .88f), new Vector2(.03f, .86f), new Vector2(.97f, .98f), Vector2.zero, Vector2.zero);
            playerFill = CreateHealth(top, new Vector2(.03f, .22f), new Vector2(.39f, .72f), UIFactory.Ember, false);
            enemyFill = CreateHealth(top, new Vector2(.61f, .22f), new Vector2(.97f, .72f), new Color(.65f, .08f, .08f), true);
            RectTransform timeBox = UIFactory.PanelObject(top, "Timer", UIFactory.Panel, new Vector2(.43f, .1f), new Vector2(.57f, .9f), Vector2.zero, Vector2.zero);
            timer = UIFactory.Text(timeBox, "2:00", 34, TextAnchor.MiddleCenter, Color.white); timer.fontStyle = FontStyle.Bold;
            Text names = UIFactory.Text(top, "EMBER                                                                                         EMBER", 19, TextAnchor.UpperCenter, new Color(1,1,1,.8f));
        }

        Image CreateHealth(Transform parent, Vector2 min, Vector2 max, Color color, bool reverse)
        {
            RectTransform bg = UIFactory.PanelObject(parent, "Health", new Color(.12f, .12f, .15f), min, max, Vector2.zero, Vector2.zero);
            RectTransform fillRt = UIFactory.PanelObject(bg, "Fill", color, Vector2.zero, Vector2.one, new Vector2(4,4), new Vector2(-4,-4));
            Image fill = fillRt.GetComponent<Image>(); fill.type = Image.Type.Filled; fill.fillMethod = Image.FillMethod.Horizontal; fill.fillOrigin = reverse ? 1 : 0; fill.fillAmount = 1f; return fill;
        }

        void CreateJoystick(Transform parent)
        {
            RectTransform baseRt = UIFactory.PanelObject(parent, "Joystick", new Color(.13f,.14f,.18f,.72f), new Vector2(.035f,.06f), new Vector2(.235f,.37f), Vector2.zero, Vector2.zero);
            baseRt.GetComponent<Image>().sprite = null;
            RectTransform knob = UIFactory.PanelObject(baseRt, "Knob", new Color(1f,.32f,.08f,.85f), new Vector2(.34f,.34f), new Vector2(.66f,.66f), Vector2.zero, Vector2.zero);
            VirtualJoystick joystick = baseRt.gameObject.AddComponent<VirtualJoystick>(); joystick.knob = knob;
        }

        void CreateActions(Transform parent)
        {
            CreateAction(parent, "ATTACK", new Vector2(.82f,.09f), new Vector2(.94f,.29f), () => MobileInputController.Instance.PressBasic(), 0, UIFactory.Ember);
            CreateAction(parent, "SKILL 1", new Vector2(.69f,.12f), new Vector2(.79f,.29f), () => MobileInputController.Instance.PressSkill1(), 1, UIFactory.Gold);
            CreateAction(parent, "SKILL 2", new Vector2(.76f,.33f), new Vector2(.86f,.50f), () => MobileInputController.Instance.PressSkill2(), 2, UIFactory.Gold);
            CreateAction(parent, "ULT", new Vector2(.88f,.35f), new Vector2(.97f,.53f), () => MobileInputController.Instance.PressUltimate(), 3, new Color(.62f,.12f,.72f));
            Button jump = UIFactory.Button(parent, "JUMP", () => MobileInputController.Instance.PressJump(), new Color(.15f,.3f,.4f)); RectTransform jr = jump.GetComponent<RectTransform>(); jr.anchorMin = new Vector2(.58f,.06f); jr.anchorMax = new Vector2(.67f,.22f); jr.offsetMin = jr.offsetMax = Vector2.zero;
        }

        void CreateAction(Transform parent, string label, Vector2 min, Vector2 max, UnityEngine.Events.UnityAction action, int index, Color color)
        {
            Button button = UIFactory.Button(parent, label, action, color); RectTransform r = button.GetComponent<RectTransform>(); r.anchorMin = min; r.anchorMax = max; r.offsetMin = r.offsetMax = Vector2.zero;
            cooldownLabels[index] = UIFactory.Text(button.transform, "", 22, TextAnchor.LowerCenter, Color.white);
        }

        void Update()
        {
            if (!match || !player || !enemy) return;
            playerFill.fillAmount = player.Health.Normalized; enemyFill.fillAmount = enemy.Health.Normalized;
            int seconds = Mathf.CeilToInt(match.RemainingTime); timer.text = $"{seconds / 60}:{seconds % 60:00}";
            for (int i = 0; i < skills.Length; i++)
            {
                float left = player.Combat.Cooldowns.Remaining(skills[i].skillId);
                cooldownLabels[i].text = left > 0.05f ? left.ToString("0.0") : "";
            }
        }

        public void ShowResult(string heading, string reason, Action rematch, Action menu)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            resultOverlay = UIFactory.PanelObject(canvas.transform, "Result", new Color(0,0,0,.82f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero).gameObject;
            RectTransform card = UIFactory.PanelObject(resultOverlay.transform, "ResultCard", UIFactory.Panel, new Vector2(.32f,.23f), new Vector2(.68f,.77f), Vector2.zero, Vector2.zero);
            Text title = UIFactory.Text(card, heading + "\n\n" + reason, 50, TextAnchor.UpperCenter, heading == "VICTORY" ? UIFactory.Gold : new Color(.9f,.2f,.2f)); title.fontStyle = FontStyle.Bold;
            Button again = UIFactory.Button(card, "REMATCH", () => rematch(), UIFactory.Ember); RectTransform ar = again.GetComponent<RectTransform>(); ar.anchorMin = new Vector2(.1f,.12f); ar.anchorMax = new Vector2(.47f,.28f); ar.offsetMin = ar.offsetMax = Vector2.zero;
            Button back = UIFactory.Button(card, "MAIN MENU", () => menu(), new Color(.18f,.2f,.28f)); RectTransform br = back.GetComponent<RectTransform>(); br.anchorMin = new Vector2(.53f,.12f); br.anchorMax = new Vector2(.9f,.28f); br.offsetMin = br.offsetMax = Vector2.zero;
        }
    }
}
