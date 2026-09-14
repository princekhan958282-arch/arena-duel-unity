using UnityEngine;

namespace ArenaDuel.Data
{
    [CreateAssetMenu(menuName = "Arena Duel/Hero Definition")]
    public sealed class HeroDefinition : ScriptableObject
    {
        public string heroId = "ember";
        public string displayName = "Ember";
        [TextArea] public string description = "A relentless arena duelist who turns heat and momentum into pressure.";
        public float maxHealth = 1000f;
        public float attack = 100f;
        public float defense = 25f;
        public float movementSpeed = 5.5f;
        public float acceleration = 20f;
        public float jumpForce = 7.5f;
        public GameObject characterPrefab;
        public Sprite portrait;
        public SkillDefinition basicAttack;
        public SkillDefinition skill1;
        public SkillDefinition skill2;
        public SkillDefinition ultimate;
    }
}
